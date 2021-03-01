namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  public interface ITerminalNodeCondition
  {
    bool Evaluate(object target);
  }

  public class TerminalNodeTypeCondition<T> : ITerminalNodeCondition
  {
    public bool Evaluate(object target)
    {
      return target is T;
    }
  }
}