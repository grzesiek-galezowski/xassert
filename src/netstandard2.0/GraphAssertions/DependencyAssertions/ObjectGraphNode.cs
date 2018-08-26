
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  public class ObjectGraphNode
  {
    private readonly object _target;
    private readonly IReadOnlyList<ObjectGraphNode> _path;
    private readonly string _name;
    private readonly Action<string> _log;

    private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;

    public ObjectGraphNode(
      object target,
      string name,
      IReadOnlyList<ObjectGraphNode> path,
      Action<string> log)
    {
      _target = target;
      _path = path;
      _name = name;
      _log = log;
      _path = path.Concat(new[] { this }).ToList();
    }

    public static ObjectGraphNode From(
      FieldInfo fieldMetadata,
      object declaringObject,
      List<ObjectGraphNode> path,
      Action<string> log)
    {
      return new ObjectGraphNode(fieldMetadata.GetValue(declaringObject), fieldMetadata.Name, path, log);
    }

    public static ObjectGraphNode From(
      PropertyInfo propertyMetadata,
      object declaringObject,
      List<ObjectGraphNode> path,
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

    public List<ObjectGraphNode> RetrievePropertiesAndFields(object o)
    {
      var fieldNodes = FieldNodes(o);
      var propertyNodes = PropertyNodes(o);

      return fieldNodes.Concat(propertyNodes).ToList();
    }

    private IEnumerable<ObjectGraphNode> FieldNodes(object o)
    {
      return o.GetType().GetFields(BindingFlags)
        .Where(f => !(f.FieldType.IsValueType && f.FieldType == f.DeclaringType))
        .Select(fieldInfo => From(fieldInfo, o, _path.ToList(), _log));
    }

    private IEnumerable<ObjectGraphNode> PropertyNodes(object o)
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



}
