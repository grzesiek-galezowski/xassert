using System;
using System.Collections.Generic;
using TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

namespace TddXt.XFluentAssert.Api;

public class DependsOnAssertionsOptions
{
  private static IList<ITerminalNodeCondition> DefaultTerminalNodeConditions()
  {
    return new List<ITerminalNodeCondition>
    {
      new TerminalNodeNamespaceCondition("Castle.Proxies"),
      new TerminalNodeTypeCondition<DateTime>(),
      new TerminalNodeTypeCondition<TimeSpan>(),
      new TerminalNodeTypeCondition<Type>(),
      new TerminalNodeTypeCondition<string>()
    };
  }

  internal IList<ITerminalNodeCondition> TerminalNodeConditions { get; } = DefaultTerminalNodeConditions();

  public DependsOnAssertionsOptions SkipType<T1>()
  {
    TerminalNodeConditions.Add(new TerminalNodeTypeCondition<T1>());
    return this;
  }

  public DependsOnAssertionsOptions Skip<T1>(T1 instance)
  {
    TerminalNodeConditions.Add(new TerminalNodeEqualityCondition(instance));
    return this;
  }
}