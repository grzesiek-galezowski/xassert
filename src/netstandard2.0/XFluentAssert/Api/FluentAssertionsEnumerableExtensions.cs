using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;

namespace TddXt.XFluentAssert.Api;

public static class FluentAssertionsEnumerableExtensions
{
  public static AndConstraint<TAssertions> Be<TAssertions, TElement>(
    this GenericCollectionAssertions<IEnumerable<TElement>, TElement, TAssertions> assertions, params TElement[] expected)
    where TAssertions : GenericCollectionAssertions<IEnumerable<TElement>, TElement, TAssertions>
  {
    return assertions.BeEquivalentTo(expected, options => options.WithStrictOrdering());
  }

  public static AndConstraint<TAssertions> Be<TAssertions, TElement>(
    this GenericCollectionAssertions<IEnumerable<TElement>, TElement, TAssertions> assertions, IEnumerable<TElement> expected,
    string because = "",
    params object[] becauseArgs)
    where TAssertions : GenericCollectionAssertions<IEnumerable<TElement>, TElement, TAssertions>
  {
    return assertions.BeEquivalentTo(expected, options => options
      .WithStrictOrdering(), because, becauseArgs);
  }

  public static AndConstraint<TAssertions> BeDeeplyEqualTo<TAssertions, TElement>(
    this GenericCollectionAssertions<IEnumerable<TElement>, TElement, TAssertions> assertions, params TElement[] expected)
    where TAssertions : GenericCollectionAssertions<IEnumerable<TElement>, TElement, TAssertions>
  {
    return assertions.BeEquivalentTo(expected, options => options
      .WithStrictOrdering()
      .IncludingNestedObjects());
  }

  public static AndConstraint<TAssertions> BeDeeplyEqualTo<TAssertions, TElement>(
    this GenericCollectionAssertions<IEnumerable<TElement>, TElement, TAssertions> assertions, IEnumerable<TElement> expected,
    string because = "",
    params object[] becauseArgs)
    where TAssertions : GenericCollectionAssertions<IEnumerable<TElement>, TElement, TAssertions>
  {
    return assertions.BeEquivalentTo(expected, options => options
      .WithStrictOrdering()
      .IncludingNestedObjects(), because, becauseArgs);
  }
}