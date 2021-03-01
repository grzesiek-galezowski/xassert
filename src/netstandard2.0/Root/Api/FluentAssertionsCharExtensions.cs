using TddXt.XFluentAssert.Api.SimpleAssertions;

namespace TddXt.XFluentAssert.Api
{
  public static class FluentAssertionsCharExtensions
  {
    public static CharAssertions Should(this char c)
    {
      return new CharAssertions(c);
    }
  }
}