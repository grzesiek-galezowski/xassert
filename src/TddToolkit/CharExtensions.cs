namespace TddEbook.TddToolkit
{
  public static class CharExtensions
  {
    public static CharAssertions Should(this char c)
    {
      return new CharAssertions(c);
    }
  }
}