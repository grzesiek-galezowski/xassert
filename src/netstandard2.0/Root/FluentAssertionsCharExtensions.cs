using TddXt.XFluentAssertRoot.SimpleAssertions;
namespace TddXt.XFluentAssertRoot
{
  using SimpleAssertions;

  public static class FluentAssertionsCharExtensions
  {
    public static CharAssertions Should(this char c)
    {
      return new CharAssertions(c);
    }
  }
}