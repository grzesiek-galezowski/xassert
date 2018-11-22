using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Primitives;
using TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

namespace TddXt.XFluentAssert.Root
{
  public static class ObjectDependencyAssertions
  {
    public static AndConstraint<ObjectAssertions> DependOn<T>(this ObjectAssertions o)
    {
      var objectTreePaths = new ObjectGraphPaths();
      new ObjectGraphNode(o.Subject, "Root", new List<IObjectGraphNode>(), str => { })
        .CollectPathsInto(objectTreePaths);
      objectTreePaths.AssertContainNonRootObjectOf(typeof(T));
      return new AndConstraint<ObjectAssertions>(o);
    }

    /*public static AndConstraint<ObjectAssertions> DependOn<T>(this ObjectAssertions o, T value)
    {
      var objectTreePaths = new ObjectGraphPaths();
      new ObjectGraphNode(o.Subject, "Root", new List<IObjectGraphNode>(), str => { }).CollectPathsInto(objectTreePaths);
      objectTreePaths.AssertContainNonRoot(value);
      return new AndConstraint<ObjectAssertions>(o);
    }*/

    public static AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>> DependOn<TAssertions, TDependency, TThisType>(
      this ReferenceTypeAssertions<TThisType, TAssertions> o,
      TDependency value)
      where TAssertions : ReferenceTypeAssertions<TThisType, TAssertions>
    {
      var objectTreePaths = new ObjectGraphPaths();
      new ObjectGraphNode(o.Subject, "Root", new List<IObjectGraphNode>(), str => { }).CollectPathsInto(objectTreePaths);
      objectTreePaths.AssertContainNonRoot(value);
      return new AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>>(o);

    }

    public static AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>>
      DependOnTypeChain<TThisType, TAssertions>(
        this ReferenceTypeAssertions<TThisType, TAssertions> o,
        params Type[] types)
      where TAssertions : ReferenceTypeAssertions<TThisType, TAssertions>
    {
      var objectTreePaths = new ObjectGraphPaths();
      new ObjectGraphNode(o.Subject, "Root", new List<ObjectGraphNode>(), str => { }).CollectPathsInto(objectTreePaths);
      objectTreePaths.AssertContainNonRootTypeSubPath(types);
      return new AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>>(o);
    }

    public static AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>>
      DependOnChain<TThisType, TAssertions>(
        this ReferenceTypeAssertions<TThisType, TAssertions> o,
        params object[] values)
      where TAssertions : ReferenceTypeAssertions<TThisType, TAssertions>
    {
      var objectTreePaths = new ObjectGraphPaths();
      new ObjectGraphNode(o.Subject, "Root", new List<ObjectGraphNode>(), str => { }).CollectPathsInto(objectTreePaths);
      objectTreePaths.AssertContainNonRootSubPath(values);
      return new AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>>(o);
    }


  }
}