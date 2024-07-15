using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.EqualityOperator;

internal class InequalityWithNullMustBeImplementedInTermsOfEqualityOperator<T>(
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
            RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(typeof(T), instance1, null),
              "a == null should return false", violations),
          "a == null should return false", violations);

        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(typeof(T), null, instance1),
              "null == a should return false", violations),
          "null == a should return false", violations);
      }
    }
  }
}