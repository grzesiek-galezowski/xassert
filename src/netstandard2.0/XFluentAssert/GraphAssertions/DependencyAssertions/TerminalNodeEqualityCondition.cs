namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

internal class TerminalNodeEqualityCondition : ITerminalNodeCondition
{
  private readonly object _instance;

  public TerminalNodeEqualityCondition(object instance)
  {
    _instance = instance;
  }

  public bool Evaluate(object target)
  {
    return Equals(_instance, target);
  }

}