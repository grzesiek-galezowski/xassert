namespace TddXt.XFluentAssert.Root
{
  using TddXt.XFluentAssert.Root.SimpleAssertions;

  public static class FluentAssertionsCharExtensions
  {
    public static CharAssertions Should(this char c)
    {
      return new CharAssertions(c);
    }
  }
}