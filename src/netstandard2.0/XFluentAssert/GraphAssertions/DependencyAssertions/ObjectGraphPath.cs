using System;
using System.Collections.Generic;
using System.Linq;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  internal class ObjectGraphPath
  {
    //TODO support for recursion (direct or indirect) or reference to this
    //TODO  UT
    private readonly IReadOnlyList<IObjectGraphNode> _path;
    private IReadOnlyList<IObjectGraphNode> PathWithoutRoot => _path.Skip(1).ToList();

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
      return string.Join("->", _path.Select(p => "[" + p + "]"));
    }

    public bool ContainsNonRootSubpath(object[] values)
    {
      for (var i = 0; i < PathWithoutRoot.Count(); ++i)
      {
        if (PathWithoutRoot[i].ValueIsEqualTo(values[0]) && PathWithoutRoot.Skip(i).Count() >= values.Length)
        {
          var collectionToCompare = PathWithoutRoot.Skip(i).Take(values.Length);
          if (collectionToCompare.SequenceEqual(values, new NodeToObjectComparer()))
          {
            return true;
          }
        }
      }

      return false;
    }

    public bool ContainsNonRootTypeSubpath(Type[] types)
    {
      for (var i = 0; i < PathWithoutRoot.Count(); ++i)
      {
        if (PathWithoutRoot[i].IsOf(types[0]) && PathWithoutRoot.Skip(i).Count() >= types.Length)
        {
          var collectionToCompare = PathWithoutRoot.Skip(i).Take(types.Length);
          if (collectionToCompare.SequenceEqual(types, new NodeToTypeComparer()))
          {
            return true;
          }
        }
      }

      return false;
    }
  }

  internal class NodeToTypeComparer : IEqualityComparer<object>
  {
    public new bool Equals(object x, object y)
    {
      return ((ObjectGraphNode)x).IsOf((Type)y);
    }

    public int GetHashCode(object obj)
    {
      return obj.GetHashCode();
    }
  }

  internal class NodeToObjectComparer : IEqualityComparer<object>
  {
    public new bool Equals(object x, object y)
    {
      return ((ObjectGraphNode)x).ValueIsEqualTo(y);
    }

    public int GetHashCode(object obj)
    {
      return obj.GetHashCode();
    }
  }
}