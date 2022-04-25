using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

internal class ObjectGraphNodeFactory
{
  private readonly IEnumerable<ITerminalNodeCondition> _terminalNodeConditions;

  public ObjectGraphNodeFactory(
    Action<string> log,
    IEnumerable<ITerminalNodeCondition> terminalNodeConditions)
  {
    _terminalNodeConditions = terminalNodeConditions;
  }

  public IObjectGraphNode From(
    IReadOnlyList<IObjectGraphNode> path,
    object target,
    string targetHolderName)
  {
    if (target == null)
    {
      return new NullNode(path);
    }

    if (target.GetType().GetTypeInfo().IsArray)
    {
      return new ArrayNode(
        target,
        targetHolderName,
        path, this);
    }

    if (IsSpecialTerminalCase(target, path, _terminalNodeConditions))
    {
      return new SpecialCaseTerminalNode(target, targetHolderName, path);
    }

    return new ObjectGraphNode(target, targetHolderName, path, this);
  }

  private static bool IsSpecialTerminalCase(
    object target,
    IEnumerable<IObjectGraphNode> path,
    IEnumerable<ITerminalNodeCondition> terminalNodeConditions)
  {
    if (CycleDetected(target, path))
    {
      return true;
    }

    foreach (var condition in terminalNodeConditions)
    {
      if (condition.Evaluate(target))
      {
        return true;
      }
    }

    return false;
  }

  private static bool CycleDetected(object target, IEnumerable<IObjectGraphNode> path)
  {
    return !target.GetType().IsValueType && path.Any(node => node.ValueIsSameAs(target));
  }

  public ObjectGraphNode Root(object subject)
  {
    return new ObjectGraphNode(
      subject,
      "Root",
      new List<ObjectGraphNode>(),
      this);
  }
}