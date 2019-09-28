
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

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

    private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;

    public ObjectGraphNode(
      object target,
      string name,
      IReadOnlyList<IObjectGraphNode> path,
      Action<string> log)
    {
      _target = target;
      _path = path;
      _name = name;
      _log = log;
      _path = path.Concat(new[] { this }).ToList();
    }

    public static IObjectGraphNode From(
      IReadOnlyList<IObjectGraphNode> path,
      object target,
      string targetHolderName,
      Action<string> log)
    {
      if (target == null)
      {
        return new NullNode(path);
      }
      else if (target.GetType().GetTypeInfo().IsArray)
      {
        return new ArrayNode(target, targetHolderName, path, log);
      }
      else if (IsSpecialTerminalCase(target, path))
      {
        return new SpecialCaseTerminalNode(target, targetHolderName, path);
      }
      else
      {
        return new ObjectGraphNode(target, targetHolderName, path, log);
      }
    }

    private static bool IsSpecialTerminalCase(object target, IReadOnlyList<IObjectGraphNode> path)
    {
      if (CycleDetected(target, path))
      {
        return true;
      }

      if (target.GetType().Namespace.Contains("Castle.Proxies"))
      {
        return true;
      }

      if (target is DateTime)
      {
          return true;
      }

      if (target is TimeSpan)
      {
          return true;
      }

      if (target is string)
      {
          return true;
      }

      if (target is CancellationToken)
      {
        return false;
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
        .Select(fieldInfo => From(_path.ToList(), fieldInfo.GetValue(o), fieldInfo.Name, _log));
    }

    private IEnumerable<IObjectGraphNode> PropertyNodes(object o)
    {
        var type = o.GetType();
        var propertyInfos = type.GetProperties(BindingFlags);
        var enumerable = propertyInfos
            .Where(p => !p.GetIndexParameters().Any()).ToList();
        var objectGraphNodes = enumerable
            .Select(propertyInfo => From(_path.ToList(), propertyInfo.GetValue(o), propertyInfo.Name, _log)).ToList();
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
  }

  internal class ArrayNode : IObjectGraphNode
  {
    private readonly object _target;
    private readonly string _fieldName;
    private readonly IReadOnlyList<IObjectGraphNode> _path;
    private readonly Action<string> _log;

    public ArrayNode(object target, string fieldName, IReadOnlyList<IObjectGraphNode> path, Action<string> log)
    {
      _target = target;
      _fieldName = fieldName;
      _log = log;
      _path = path.Concat(new [] {this}).ToList();
    }

    public void CollectPathsInto(ObjectGraphPaths objectGraphPaths)
    {
      //todo consider extracting the items before passing them to this class instance
      var items = (_target as object[]).Select((o, i) => ObjectGraphNode.From(_path, o, "array element[" + i + "]", _log));
      if (items.Any())
      {
        foreach (var item in items)
        {
          item.CollectPathsInto(objectGraphPaths);
        }
      }
      else
      {
        objectGraphPaths.Add(new ObjectGraphPath(_path));
      }
    }

    public bool IsOf(Type type)
    {
      return _target.GetType() == type;
    }

    public bool ValueIsEqualTo<T>(T value)
    {
      //todo what if both are null?
      if ((object)value is object[] expectedArray && _target is object[] targetArray)
      {
        return expectedArray.SequenceEqual(targetArray);
      }
      else
      {
        return Equals(value, _target);
      }
    }

    public bool ValueIsSameAs(object value)
    {
      return ReferenceEquals(_target, value);
    }

    public override string ToString()
    {
      return ObjectGraphNode.FormatFieldAndTypeName(_fieldName, _target);
    }
  }
}
