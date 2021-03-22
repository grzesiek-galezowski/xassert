using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Primitives;
using TddXt.XFluentAssert.GraphAssertions;
using TddXt.XFluentAssert.LockAssertions;

namespace TddXt.XFluentAssert.Api
{

  public static class FluentAssertionsObjectExtensions
  {
    public static AndConstraint<ObjectAssertions> NotBeLike<T>(
      this ObjectAssertions o,
      T expected,
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, (T)o.Subject, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ReferenceTypeAssertions<T, TAssertions>> NotBeLike<T, TAssertions>(
      this ReferenceTypeAssertions<T, TAssertions> o,
      T expected,
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
      where TAssertions : ReferenceTypeAssertions<T, TAssertions>
    {
      var result = ObjectGraph.Compare(expected, o.Subject, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
      return new AndConstraint<ReferenceTypeAssertions<T, TAssertions>>(o);
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

    public static AndConstraint<ReferenceTypeAssertions<T, TAssertions>> BeLike<T, TAssertions>(
      this ReferenceTypeAssertions<T, TAssertions> o,
      T expected,
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
      where TAssertions : ReferenceTypeAssertions<T, TAssertions>
    {
      var result = ObjectGraph.Compare(expected, o.Subject, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
      return new AndConstraint<ReferenceTypeAssertions<T, TAssertions>>(o);
    }


    public static void SynchronizeAccessTo<T>(
      this ObjectAssertions assertions,
      Action<T> callToCheck,
      XFluentAssert.LockAssertions.ILockAssertions lockAssertions,
      T wrappedObjectMock)
      where T : class
    {
      SynchronizationAssertions.Synchronizes(
        (T)assertions.Subject,
        callToCheck,
        lockAssertions,
        wrappedObjectMock);
    }

    public static void SynchronizeAccessTo<T, TAssertions>(
      this ReferenceTypeAssertions<T, TAssertions> assertions,
      Action<T> callToCheck,
      XFluentAssert.LockAssertions.ILockAssertions lockAssertions,
      T wrappedObjectMock)
      where T : class
      where TAssertions : ReferenceTypeAssertions<T, TAssertions>
    {
      SynchronizationAssertions.Synchronizes(
        assertions.Subject,
        callToCheck,
        lockAssertions,
        wrappedObjectMock);
    }

    public static void SynchronizeAccessTo<T, TReturn>(
      this ObjectAssertions assertions,
      Func<T, TReturn> callToCheck,
      XFluentAssert.LockAssertions.ILockAssertions lockAssertions,
      T wrappedObjectMock)
      where T : class
    {
      SynchronizationAssertions.Synchronizes(
        (T)assertions.Subject,
        callToCheck,
        lockAssertions,
        wrappedObjectMock);
    }

    public static void SynchronizeAccessTo<T, TReturn, TAssertions>(
      this ReferenceTypeAssertions<T, TAssertions> assertions,
      Func<T, TReturn> callToCheck,
      XFluentAssert.LockAssertions.ILockAssertions lockAssertions,
      T wrappedObjectMock)
      where T : class
      where TAssertions : ReferenceTypeAssertions<T, TAssertions>
    {
      SynchronizationAssertions.Synchronizes(
        assertions.Subject,
        callToCheck,
        lockAssertions,
        wrappedObjectMock);
    }
  }
}