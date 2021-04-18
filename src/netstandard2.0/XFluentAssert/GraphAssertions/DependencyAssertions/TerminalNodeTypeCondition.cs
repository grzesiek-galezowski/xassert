namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  internal interface ITerminalNodeCondition
  {
    bool Evaluate(object target);
  }

  internal class TerminalNodeTypeCondition<T> : ITerminalNodeCondition
  {
    public bool Evaluate(object target)
    {
      return target is T;
    }
  }
}