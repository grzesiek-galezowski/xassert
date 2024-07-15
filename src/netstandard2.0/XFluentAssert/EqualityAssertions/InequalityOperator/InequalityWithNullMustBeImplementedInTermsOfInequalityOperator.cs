using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.InequalityOperator;

internal class InequalityWithNullMustBeImplementedInTermsOfInequalityOperator<T>(
  Func<T>[] equalInstances,
  Func<T>[] otherInstances) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    if (!typeof(T).IsValueType)
    {
      foreach (var factory in equalInstances.Concat(otherInstances))
      {
        var instance1 = factory();
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(
              Are.NotEqualInTermsOfInEqualityOperator(typeof(T), instance1, null),
              "a != null should return true", violations),
          "a != null should return true", violations);
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(
              Are.NotEqualInTermsOfInEqualityOperator(typeof(T), null, instance1),
              "null != a should return true", violations),
          "null != a should return true", violations);
      }
    }
  }
}