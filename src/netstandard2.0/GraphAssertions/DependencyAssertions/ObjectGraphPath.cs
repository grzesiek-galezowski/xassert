using System;
using System.Collections.Generic;
using System.Linq;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  public class ObjectGraphPath
  {
    //TODO support for recursion (direct or indirect) or reference to this
    //TODO  UT
    private readonly IReadOnlyList<IObjectGraphNode> _path;
    private IEnumerable<IObjectGraphNode> PathWithoutRoot => _path.Skip(1);

    public ObjectGraphPath(IReadOnlyList<IObjectGraphNode> path)
    {
      _path = path;
    }

    public bool ContainsNonRootInstanceOf(Type type)
    {
      return PathWithoutRoot.Any(node => node.IsOf(type));
    }


    public bool ContainsNonRootValueEqualTo(object value)
    {
      return PathWithoutRoot.Any(node => node.ValueIsEqualTo(value));
    }

    public override string ToString()
    {
      return string.Join("->", _path.Select(p => "[" + p.ToString() + "]"));
    }
  }
}