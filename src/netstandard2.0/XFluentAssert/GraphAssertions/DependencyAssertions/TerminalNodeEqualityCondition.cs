namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

internal class TerminalNodeEqualityCondition(object instance) : ITerminalNodeCondition
{
  public bool Evaluate(object target)
  {
    return Equals(instance, target);
  }

}