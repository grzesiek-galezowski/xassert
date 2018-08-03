using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace TddEbook.TddToolkit
{
  public partial class XAssert
  {
    public static void Equal<T>(T expected, T actual)
    {
      actual.Should().Be(expected);
    }
    public static void Equal(string expected, string actual)
    {
      actual.Should().Be(expected);
    }
    public static void Equal(int expected, int actual)
    {
      actual.Should().Be(expected);
    }

    public static void Equal<T>(T expected, T actual, string message)
    {
      actual.Should().Be(expected, "{0}", message);
    }

    public static void CollectionsEqual<T>
      (IEnumerable<T> expected, IEnumerable<T> actual)
    {
      actual.Count().Should().Be(expected.Count());
      actual.Should().ContainInOrder(expected);
    }

    public static void CollectionsEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
      actual.Should().BeEquivalentTo(expected);
    }
    public static void CollectionsNotEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
      actual.Should().BeEquivalentTo(expected);
    }

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
