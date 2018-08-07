using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Types;
using GraphAssertions;

namespace TddEbook.TddToolkit
{
  public static class FluentAssertionExtensions
  {
    public static AndConstraint<ObjectAssertions> BeLike(this ObjectAssertions o, object expected)
    {
      var comparison = ObjectGraph.Comparison();
      var result = comparison.Compare(expected, o.Subject);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ObjectAssertions> NotBeLike(this ObjectAssertions o, object expected)
    {
      var comparison = ObjectGraph.Comparison();
      var result = comparison.Compare(expected, o.Subject);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ObjectAssertions> BeLike<T>(
      this ObjectAssertions o,
      object expected, 
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      XAssert.Alike(expected, o.Subject);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<TypeAssertions> BehaveLikeValue(this TypeAssertions o)
    {
      XAssert.IsValue(o.Subject);
      return new AndConstraint<TypeAssertions>(o);
    }
  }
}
