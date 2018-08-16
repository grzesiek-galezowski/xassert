namespace TddXt.XFluentAssert.AssertionConstraints
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using FluentAssertions;

  public class ConstraintsViolations : IConstraintsViolations
  {
    private readonly List<string> _violations = new List<string>();

    public static ConstraintsViolations Empty()
    {
      return new ConstraintsViolations();
    }

    public void AssertNone()
    {
      _violations.Any().Should().BeFalse("The type should not fail any value type assertions, but failed: " 
                                         + Environment.NewLine + MessageContainingAll(_violations) + Environment.NewLine);
    }

    public void Add(string violationDetails)
    {
      _violations.Add(violationDetails);
    }

    private static string MessageContainingAll(IEnumerable<string> violations)
    {
      return violations.Any() ? violations.Aggregate((a, b) => a + Environment.NewLine + b) : "No violations.";
    }

  }
}