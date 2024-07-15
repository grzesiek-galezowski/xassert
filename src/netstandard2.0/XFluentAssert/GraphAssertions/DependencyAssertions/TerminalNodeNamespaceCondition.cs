namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

internal class TerminalNodeNamespaceCondition(string namespaceFragment) : ITerminalNodeCondition
{
  public bool Evaluate(object target)
  {
    return target.GetType().Namespace.Contains(namespaceFragment);
  }
}