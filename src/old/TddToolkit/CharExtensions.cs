namespace TddXt.XAssert.TddEbook.TddToolkit
{
  using TddXt.XAssert.TddEbook.TddToolkit.SimpleAssertions;

  public static class CharExtensions
  {
    public static CharAssertions Should(this char c)
    {
      return new CharAssertions(c);
    }
  }
}