using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions2.InequalityOperator
{
  public class StateBasedEqualityMustBeImplementedInTermsOfInequalityOperator<T> : IConstraint
  {
    private readonly Func<T>[] _equalInstances;

    public StateBasedEqualityMustBeImplementedInTermsOfInequalityOperator(Func<T>[] equalInstances)
    {
      _equalInstances = equalInstances;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      foreach (var factory in _equalInstances)
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
}