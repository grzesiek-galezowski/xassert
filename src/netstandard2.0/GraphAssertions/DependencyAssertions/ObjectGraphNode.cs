
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using FluentAssertions.Primitives;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  public interface IObjectGraphNode
  {
    void CollectPathsInto(ObjectGraphPaths objectGraphPaths);
    bool IsOf(Type type);
    bool ValueIsEqualTo<T>(T value);
    bool ValueIsSameAs(object value);
  }

  public class ObjectGraphNode : IObjectGraphNode
  {
    private readonly object _target;
    private readonly IReadOnlyList<IObjectGraphNode> _path;
    private readonly string _name;
    private readonly Action<string> _log;
    private readonly IEnumerable<ITerminalNodeCondition> _terminalNodeConditions;

    private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;

    public ObjectGraphNode(
      object target,
      string name,
      IReadOnlyList<IObjectGraphNode> path,
      Action<string> log, 
      IEnumerable<ITerminalNodeCondition> terminalNodeConditions)
    {
      _target = target;
      _path = path;
      _name = name;
      _log = log;
      _path = path.Concat(new[] { this }).ToList();
      _terminalNodeConditions = terminalNodeConditions;
    }

    public static IObjectGraphNode From(
      IReadOnlyList<IObjectGraphNode> path,
      object target,
      string targetHolderName,
      Action<string> log, 
      IEnumerable<ITerminalNodeCondition> terminalNodeConditions)
    {
      if (target == null)
      {
        return new NullNode(path);
      }

      if (target.GetType().GetTypeInfo().IsArray)
      {
        return new ArrayNode(
          target, 
          targetHolderName, 
          path, 
          log, 
          terminalNodeConditions);
      }

      if (IsSpecialTerminalCase(target, path, terminalNodeConditions))
      {
        return new SpecialCaseTerminalNode(target, targetHolderName, path);
      }

      return new ObjectGraphNode(target, targetHolderName, path, log, terminalNodeConditions);
    }

    private static bool IsSpecialTerminalCase(
      object target, 
      IReadOnlyList<IObjectGraphNode> path, 
      IEnumerable<ITerminalNodeCondition> terminalNodeConditions)
    {
      if (CycleDetected(target, path))
      {
        return true;
      }

      if (target.GetType().Namespace.Contains("Castle.Proxies"))
      {
        return true;
      }

      foreach (var condition in terminalNodeConditions)
      {
        if (condition.Evaluate(target))
        {
          return true;
        }
      }

      return false;
    }

    private static bool CycleDetected(object target, IReadOnlyList<IObjectGraphNode> path)
    {
      return !target.GetType().IsValueType && path.Any(node => node.ValueIsSameAs(target));
    }

    public void CollectPathsInto(ObjectGraphPaths objectGraphPaths)
    {
      var children = RetrievePropertiesAndFields(_target);
      if(children.Any())
      {
        foreach (var child in children)
        {
          child.CollectPathsInto(objectGraphPaths);
        }
      }
      else
      {
        objectGraphPaths.Add(new ObjectGraphPath(_path));
      }
    }

    private List<IObjectGraphNode> RetrievePropertiesAndFields(object o)
    {
      var fieldNodes = FieldNodes(o);
      var propertyNodes = PropertyNodes(o);

      return fieldNodes.Concat(propertyNodes).ToList();
    }

    private IEnumerable<IObjectGraphNode> FieldNodes(object o)
    {
      return o.GetType().GetFields(BindingFlags)
        .Where(f => !(f.FieldType.IsValueType && f.FieldType == f.DeclaringType))
        .Select(fieldInfo => From(_path.ToList(), fieldInfo.GetValue(o), fieldInfo.Name, _log, _terminalNodeConditions));
    }

    private IEnumerable<IObjectGraphNode> PropertyNodes(object o)
    {
        var type = o.GetType();
        var propertyInfos = type.GetProperties(BindingFlags);
        var enumerable = propertyInfos
            .Where(p => !p.GetIndexParameters().Any()).ToList();
        var objectGraphNodes = enumerable
            .Select(propertyInfo => From(_path.ToList(), propertyInfo.GetValue(o), propertyInfo.Name, _log, 
              _terminalNodeConditions)).ToList();
        return objectGraphNodes;
    }

    public override string ToString()
    {
      return FormatFieldAndTypeName(_name, _target);
    }

    public static string FormatFieldAndTypeName(string fieldName, object fieldValue)
    {
      return fieldName + "(" + fieldValue.GetType().Name + ")";
    }

    public bool IsOf(Type type)
    {
      return _target.GetType() == type;
    }

    public bool ValueIsEqualTo<T>(T value)
    {
      return Equals(_target, value);
    }

    public bool ValueIsSameAs(object value)
    {
      return ReferenceEquals(_target, value);
    }

    public static ObjectGraphNode Root<TThisType, TAssertions>(ReferenceTypeAssertions<TThisType, TAssertions> o, Action<string> log, IEnumerable<ITerminalNodeCondition> terminalNodeConditions) where TAssertions : ReferenceTypeAssertions<TThisType, TAssertions>
    {
      return new ObjectGraphNode(
        o.Subject, 
        "Root", 
        new List<ObjectGraphNode>(), 
        log, 
        terminalNodeConditions);
    }
  }
}
