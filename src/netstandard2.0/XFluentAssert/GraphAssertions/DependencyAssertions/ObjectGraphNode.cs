
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;

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
    private readonly ObjectGraphNodeFactory _objectGraphNodeFactory;

    private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly;

    public ObjectGraphNode(
      object target,
      string name,
      IReadOnlyList<IObjectGraphNode> path,
      ObjectGraphNodeFactory objectGraphNodeFactory)
    {
      _target = target;
      _path = path;
      _name = name;
      _path = path.Concat(new[] { this }).ToList();
      _objectGraphNodeFactory = objectGraphNodeFactory;
    }

    public void CollectPathsInto(ObjectGraphPaths objectGraphPaths)
    {
      var children = RetrievePropertiesAndFields(_target);
      if (children.Any())
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
        .Select(fieldInfo => _objectGraphNodeFactory.From(_path.ToList(), fieldInfo.GetValue(o), fieldInfo.Name));
    }

    private IEnumerable<IObjectGraphNode> PropertyNodes(object o)
    {
      var type = o.GetType();
      var propertyInfos = type.GetProperties(BindingFlags);
      var enumerable = propertyInfos
          .Where(p => !p.GetIndexParameters().Any()).ToList();
      var objectGraphNodes = enumerable
          .Select(propertyInfo => _objectGraphNodeFactory.From(_path.ToList(), PropertyValueOrThrow(o, propertyInfo), propertyInfo.Name)).ToList();
      return objectGraphNodes;
    }

    private static object PropertyValueOrThrow(object o, PropertyInfo propertyInfo)
    {
      object? propertyValue = null;
      propertyInfo.Invoking(p => propertyValue = p.GetValue(o)).Should().NotThrow();
      return propertyValue;
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
}
