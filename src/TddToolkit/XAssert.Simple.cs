using FluentAssertions;

namespace TddEbook.TddToolkit
{
  public partial class XAssert
  {
    public static void IsUpperCase(string str)
    {
      str.Should().Be(str.ToUpperInvariant());
    }

    public static void IsLowerCase(string str)
    {
      str.Should().Be(str.ToLowerInvariant());
    }

    public static void IsLowerCase(char c)
    {
      c.Should().Be(char.ToLowerInvariant(c));
    }

    public static void IsUpperCase(char c)
    {
      c.Should().Be(char.ToUpperInvariant(c));
    }
  }
}
