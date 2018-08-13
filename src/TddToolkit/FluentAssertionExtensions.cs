
namespace TddEbook.TddToolkit
{
using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Types;
using GraphAssertions;
using LockAssertions;
  public static class FluentAssertionExtensions
  {
    public static AndConstraint<ObjectAssertions> BeLike<T>(this ObjectAssertions o, T expected,
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, (T)o.Subject, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }
    public static AndConstraint<ObjectAssertions> BeLike<T>(this ObjectAssertions o, T expected)
    {
      var comparison = ObjectGraph.Comparison();
      var result = comparison.Compare(expected, o.Subject);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ObjectAssertions> NotBeLike<T>(this ObjectAssertions o, T expected)
    {
      var comparison = ObjectGraph.Comparison();
      var result = comparison.Compare(expected, o.Subject);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ObjectAssertions> NotBeLike<T>(
      this ObjectAssertions o,
      T expected,
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, (T)o.Subject, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ObjectAssertions> BeLike<T>(
      this ObjectAssertions o,
      T expected, 
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, (T)o.Subject, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<TypeAssertions> BehaveLikeValue(this TypeAssertions o)
    {
      XAssert.IsValue(o.Subject);
      return new AndConstraint<TypeAssertions>(o);
    }


    public static void SynchronizeAccessTo<T>(this ObjectAssertions assertions,
      Action<T> callToCheck,
      LockAssertions lockAssertions,
      T wrappedObjectMock) 
      where T : class
    {
      SynchronizationAssertions.Synchronizes(
        (T)assertions.Subject, 
        callToCheck, 
        lockAssertions, 
        wrappedObjectMock);
    }

    public static void SynchronizeAccessTo<T, TReturn>(this ObjectAssertions assertions,
      Func<T, TReturn> callToCheck,
      LockAssertions lockAssertions,
      T wrappedObjectMock)
      where T : class
    {
      SynchronizationAssertions.Synchronizes(
        (T)assertions.Subject,
        callToCheck,
        lockAssertions,
        wrappedObjectMock);
    }

  }
}
