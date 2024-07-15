using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.InequalityOperator;

internal class StateBasedEqualityMustBeImplementedInTermsOfInequalityOperator<T>(Func<T>[] equalInstances) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory in equalInstances)
    {
      var instance1 = factory();
      var instance2 = factory();

      RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.False(Are.NotEqualInTermsOfInEqualityOperator(typeof(T), instance1, instance2),
            "a != b should return false for equal values", violations),
        "a != b should return false for equal values", violations);
      RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.False(Are.NotEqualInTermsOfInEqualityOperator(typeof(T), instance2, instance1),
            "b != a should return false for equal values", violations),
        "b != a should return false for equal values", violations);
    }
  }
}