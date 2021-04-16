using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions2.EqualityOperator
{
  public class StateBasedInequalityMustBeImplementedInTermsOfEqualityOperator<T>
    : IConstraint
  {
    private readonly Func<T>[] _equalInstances;
    private readonly Func<T>[] _otherInstances;

    public StateBasedInequalityMustBeImplementedInTermsOfEqualityOperator(
      Func<T>[] equalInstances, 
      Func<T>[] otherInstances)
    {
      _equalInstances = equalInstances;
      _otherInstances = otherInstances;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      foreach (var factory1 in _equalInstances)
      {
        foreach (var factory2 in _otherInstances)
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
}