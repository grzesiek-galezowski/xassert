using System;
using System.Collections.Generic;
using System.Linq;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
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

    public bool ValueIsSameAs(object value)
    {
      return value == null;
    }

    public override string ToString()
    {
      return "null";
    }
  }
}