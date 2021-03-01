using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;

namespace TddXt.XFluentAssert.Api
{
  public static class FluentAssertionsEnumerableExtensions
  {
    public static AndConstraint<TAssertions> Be<TAssertions, TElement>(
        this CollectionAssertions<IEnumerable<TElement>, TAssertions> assertions, params TElement[] expected)
        where TAssertions : CollectionAssertions<IEnumerable<TElement>, TAssertions>
    {
      return assertions.BeEquivalentTo(expected, options => options.WithStrictOrdering());
    }

    public static AndConstraint<TAssertions> Be<TAssertions, TElement>(
        this CollectionAssertions<IEnumerable<TElement>, TAssertions> assertions, IEnumerable<TElement> expected,
        string because = "",
        params object[] becauseArgs)
        where TAssertions : CollectionAssertions<IEnumerable<TElement>, TAssertions>
    {
      return assertions.BeEquivalentTo(expected, options => options
          .WithStrictOrdering(), because, becauseArgs);
    }

    public static AndConstraint<TAssertions> BeDeeplyEqualTo<TAssertions, TElement>(
        this CollectionAssertions<IEnumerable<TElement>, TAssertions> assertions, params TElement[] expected)
        where TAssertions : CollectionAssertions<IEnumerable<TElement>, TAssertions>
    {
      return assertions.BeEquivalentTo(expected, options => options
          .WithStrictOrdering()
          .IncludingNestedObjects());
    }

    public static AndConstraint<TAssertions> BeDeeplyEqualTo<TAssertions, TElement>(
        this CollectionAssertions<IEnumerable<TElement>, TAssertions> assertions, IEnumerable<TElement> expected,
        string because = "",
        params object[] becauseArgs)
        where TAssertions : CollectionAssertions<IEnumerable<TElement>, TAssertions>
    {
      return assertions.BeEquivalentTo(expected, options => options
          .WithStrictOrdering()
          .IncludingNestedObjects(), because, becauseArgs);
    }
  }
}