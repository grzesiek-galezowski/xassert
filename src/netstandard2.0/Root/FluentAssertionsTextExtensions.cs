namespace TddXt.XFluentAssert.Root
{
  using System;

  using FluentAssertions;
  using FluentAssertions.Primitives;

  using TddXt.XFluentAssert.Root.SimpleAssertions;

  public static class FluentAssertionsTextExtensions
  {
    public static AndConstraint<CharAssertions> BeUppercase(this CharAssertions assertions)
    {
      char c = assertions.CharSubject;
      FluentAssertionsCharExtensions.Should(c).Be(Char.ToUpperInvariant(c));
      return new AndConstraint<CharAssertions>(assertions);
    }

    public static AndConstraint<CharAssertions> BeLowercase(this CharAssertions assertions)
    {
      char c = assertions.CharSubject;
      FluentAssertionsCharExtensions.Should(c).Be(Char.ToLowerInvariant(c));
      return new AndConstraint<CharAssertions>(assertions);
    }

    public static AndConstraint<StringAssertions> BeUppercase(this StringAssertions assertions)
    {
      string str = assertions.Subject;
      str.Should().Be(str.ToUpperInvariant());
      return new AndConstraint<StringAssertions>(assertions);
    }

    public static AndConstraint<StringAssertions> BeLowercase(this StringAssertions assertions)
    {
      string str = assertions.Subject;
      str.Should().Be(str.ToLowerInvariant());
      return new AndConstraint<StringAssertions>(assertions);
    }
  }
}