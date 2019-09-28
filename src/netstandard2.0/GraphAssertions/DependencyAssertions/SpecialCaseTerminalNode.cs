using System;
using System.Collections.Generic;
using System.Linq;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  internal class SpecialCaseTerminalNode : IObjectGraphNode
  {
    private readonly object _target;
    private readonly string _fieldName;
    private readonly IReadOnlyList<IObjectGraphNode> _path;

    //todo use the field name
    public SpecialCaseTerminalNode(object target, string fieldName, IEnumerable<IObjectGraphNode> path)
    {
      _target = target;
      _fieldName = fieldName;
      _path = path.Concat(new [] {this}).ToList();
    }

    public void CollectPathsInto(ObjectGraphPaths objectGraphPaths)
    {
      objectGraphPaths.Add(new ObjectGraphPath(_path));
    }

    public bool IsOf(Type type)
    {
      return Equals(_target.GetType(), type);
    }

    public bool ValueIsEqualTo<T>(T value)
    {
      return Equals(_target, value);
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