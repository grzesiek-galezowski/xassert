  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
  using FluentAssertions.Primitives;

  using FluentAssertions;
using TddXt.XFluentAssertRoot.SimpleAssertions;

namespace TddXt.XFluentAssertRoot
{

  using SimpleAssertions;

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

    public static AndConstraint<StringAssertions> ContainInOrder(this StringAssertions assertions, params string[] subtexts)
    {
      var subject = assertions.Subject;
      var indices = subtexts.Select(subtext => subject.IndexOf(subtext, StringComparison.Ordinal));

      indices.Should().NotContain(-1, subject);
      indices.Should().BeInAscendingOrder(subject);
      return new AndConstraint<StringAssertions>(assertions);
    }

    public static AndConstraint<StringAssertions> ContainExactlyOnce(this StringAssertions assertions, string substring)
    {
      var subject = assertions.Subject;
      IndexOfAll(subject, substring).Should().HaveCount(1,
        "\"" + subject + "\"" + " should contain exactly 1 occurence of " + "\"" + substring + "\"");
      return new AndConstraint<StringAssertions>(assertions);
    }

    private static IEnumerable<int> IndexOfAll(string sourceString, string subString)
    {
      var indices = new List<int>();
      var matchCollection = Regex.Matches(sourceString, Regex.Escape(subString));
      foreach (Match m in matchCollection)
      {
        indices.Add(m.Index);
      }

      return indices;
    }
  }
}