
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  public interface IObjectGraphNode
  {
    void CollectPathsInto(ObjectGraphPaths objectGraphPaths);
    bool IsOf(Type type);
    bool ValueIsEqualTo<T>(T value);
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

    private static IObjectGraphNode From(
      FieldInfo fieldMetadata,
      object declaringObject,
      IReadOnlyList<IObjectGraphNode> path,
      Action<string> log)
    {
      var target = fieldMetadata.GetValue(declaringObject);
      if (target == null)
      {
        return new NullNode(path);
      }
      else if (target.GetType().Namespace.Contains("Castle.Proxies"))
      {
        return new SpecialCaseTerminalNode(target, path);
      }
      else
      {
        return new ObjectGraphNode(target, fieldMetadata.Name, path, log);
      }
    }

    private static IObjectGraphNode From(
      PropertyInfo propertyMetadata,
      object declaringObject,
      IReadOnlyList<IObjectGraphNode> path,
      Action<string> log)
    {
      return new ObjectGraphNode(propertyMetadata.GetValue(declaringObject), propertyMetadata.Name, path, log);
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
        .Select(fieldInfo => From(fieldInfo, o, _path.ToList(), _log));
    }

    private IEnumerable<IObjectGraphNode> PropertyNodes(object o)
    {
      return o.GetType().GetProperties(BindingFlags)
        .Where(p => !p.GetIndexParameters().Any())
        .Select(propertyInfo => From(propertyInfo, o, _path.ToList(), _log));
    }

    public override string ToString()
    {
      return _name + "(" + _target.GetType().Name + ")";
    }

    public bool IsOf(Type type)
    {
      return _target.GetType() == type;
    }

    public bool ValueIsEqualTo<T>(T value)
    {
      return Equals(_target, value);
    }
  }

  internal class SpecialCaseTerminalNode : IObjectGraphNode
  {
    private readonly object _target;
    private readonly IReadOnlyList<IObjectGraphNode> _path;

    public SpecialCaseTerminalNode(object target, IReadOnlyList<IObjectGraphNode> path)
    {
      _target = target;
      _path = path.Concat(new [] {this}).ToList();
    }

    public void CollectPathsInto(ObjectGraphPaths objectGraphPaths)
    {
      objectGraphPaths.Add(new ObjectGraphPath(_path));
    }

    public bool IsOf(Type type)
    {
      return object.Equals(_target.GetType(), type);
    }

    public bool ValueIsEqualTo<T>(T value)
    {
      return object.Equals(_target, value);
    }

    public override string ToString()
    {
      return _target.ToString();
    }
  }

  internal class NullNode : IObjectGraphNode
  {
    private readonly IReadOnlyList<IObjectGraphNode> _path;

    public NullNode(IReadOnlyList<IObjectGraphNode> path)
    {
      _path = path;
      _path = path.Concat(new[] { this }).ToList();
    }

    public void CollectPathsInto(ObjectGraphPaths objectGraphPaths)
    {
      objectGraphPaths.Add(new ObjectGraphPath(_path)); //bug check if such path already exists
    }

    public bool IsOf(Type type)
    {
      return false;
    }

    public bool ValueIsEqualTo<T>(T value)
    {
      return value == null;
    }

    public override string ToString()
    {
      return "null";
    }
  }
}
