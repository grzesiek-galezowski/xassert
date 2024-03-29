﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Primitives;
using TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

namespace TddXt.XFluentAssert.Api;

public static class ObjectDependencyAssertions
{
  private static readonly Action<string> NoLogging = str => { };

  public static AndConstraint<ObjectAssertions> DependOn<T>(this ObjectAssertions o)
  {
    return DependOn<T>(o, _ => _);
  }

  public static AndConstraint<ObjectAssertions> DependOn<T>(this ObjectAssertions o, 
    Func<DependsOnAssertionsOptions, DependsOnAssertionsOptions> optionsFunc)
  {
    var objectTreePaths = new ObjectGraphPaths();
    var dependsOnAssertionsOptions = optionsFunc(new DependsOnAssertionsOptions());
    new ObjectGraphNodeFactory(NoLogging, dependsOnAssertionsOptions.TerminalNodeConditions)
      .Root(o.Subject)
      .CollectPathsInto(objectTreePaths);
    objectTreePaths.AssertContainNonRootObjectOf(typeof(T));
    return new AndConstraint<ObjectAssertions>(o);
  }

  public static AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>> DependOn<
    TAssertions, TDependency, TThisType>(
    this ReferenceTypeAssertions<TThisType, TAssertions> o,
    TDependency value)
    where TAssertions : ReferenceTypeAssertions<TThisType, TAssertions>
  {
    var objectTreePaths = new ObjectGraphPaths();
    new ObjectGraphNodeFactory(NoLogging, DefaultTerminalNodeConditions()).Root(o.Subject)
      .CollectPathsInto(objectTreePaths);
    objectTreePaths.AssertContainNonRoot(value);
    return new AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>>(o);
  }

  public static AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>> DependOn<
    TAssertions, TDependency, TThisType>(
    this ReferenceTypeAssertions<TThisType, TAssertions> o,
    TDependency value,
    Func<DependsOnAssertionsOptions, DependsOnAssertionsOptions> optionsFunc)
    where TAssertions : ReferenceTypeAssertions<TThisType, TAssertions>
  {
    var options = optionsFunc(new DependsOnAssertionsOptions());
    var objectTreePaths = new ObjectGraphPaths();
    new ObjectGraphNodeFactory(NoLogging, options.TerminalNodeConditions).Root(o.Subject)
      .CollectPathsInto(objectTreePaths);
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
    new ObjectGraphNodeFactory(NoLogging, DefaultTerminalNodeConditions()).Root(o.Subject)
      .CollectPathsInto(objectTreePaths);
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
    new ObjectGraphNodeFactory(NoLogging, DefaultTerminalNodeConditions()).Root(o.Subject)
      .CollectPathsInto(objectTreePaths);
    objectTreePaths.AssertContainNonRootSubPath(values);
    return new AndConstraint<ReferenceTypeAssertions<TThisType, TAssertions>>(o);
  }

  private static IEnumerable<ITerminalNodeCondition> DefaultTerminalNodeConditions()
  {
    return new ITerminalNodeCondition[]
    {
      new TerminalNodeNamespaceCondition("Castle.Proxies"),
      new TerminalNodeTypeCondition<DateTime>(),
      new TerminalNodeTypeCondition<Type>(),
      new TerminalNodeTypeCondition<TimeSpan>(),
      new TerminalNodeTypeCondition<string>()
    };
  }

}