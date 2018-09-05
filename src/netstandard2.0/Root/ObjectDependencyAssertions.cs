using System.Collections.Generic;
using FluentAssertions.Primitives;
using TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

namespace TddXt.XFluentAssert.Root
{
  public static class ObjectDependencyAssertions
  {
    public static void DependOn<T>(this ObjectAssertions o)
    {
      var objectTreePaths = new ObjectGraphPaths();
      new ObjectGraphNode(o.Subject, "Root", new List<IObjectGraphNode>(), str => { }).CollectPathsInto(objectTreePaths);
      objectTreePaths.AssertContainNonRootObjectOf(typeof(T));
    }

    public static void DependOn<T>(this ObjectAssertions o, T value)
    {
      var objectTreePaths = new ObjectGraphPaths();
      new ObjectGraphNode(o.Subject, "Root", new List<IObjectGraphNode>(), str => { }).CollectPathsInto(objectTreePaths);
      objectTreePaths.AssertContainNonRoot(value);
    }

    public static void DependOn<TAssertions, TDependency, TThisType>(
      this ReferenceTypeAssertions<TThisType, TAssertions> o,
      TDependency value)
      where TAssertions : ReferenceTypeAssertions<TThisType, TAssertions>
    {
      var objectTreePaths = new ObjectGraphPaths();
      new ObjectGraphNode(o.Subject, "Root", new List<IObjectGraphNode>(), str => { }).CollectPathsInto(objectTreePaths);
      objectTreePaths.AssertContainNonRoot(value);
    }

  }
}