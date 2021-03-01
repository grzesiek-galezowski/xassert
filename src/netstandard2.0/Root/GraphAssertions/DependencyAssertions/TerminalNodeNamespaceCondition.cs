namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  public class TerminalNodeNamespaceCondition : ITerminalNodeCondition
  {
    private readonly string _namespaceFragment;

    public TerminalNodeNamespaceCondition(string namespaceFragment)
    {
      _namespaceFragment = namespaceFragment;
    }

    public bool Evaluate(object target)
    {
      return target.GetType().Namespace.Contains(_namespaceFragment);
    }
  }
}

