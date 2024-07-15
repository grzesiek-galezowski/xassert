using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.EqualityOperator;

internal class StateBasedInequalityMustBeImplementedInTermsOfEqualityOperator<T>(
  Func<T>[] equalInstances,
  Func<T>[] otherInstances) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory1 in equalInstances)
    {
      foreach (var factory2 in otherInstances)
      {
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(typeof(T), factory1(), factory2()),
              "a == b should return false for not equal values", violations),
          "a == b should return false for not equal values", violations
        );
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(typeof(T), factory2(), factory1()),
              "b == a should return false for not equal values", violations),
          "b == a should return false for not equal values", violations
        );
      }
    }
  }
}