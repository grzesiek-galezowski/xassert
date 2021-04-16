using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions2.EqualityOperator
{
  public class StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator<T>
    : IConstraint
  {
    private readonly Func<T>[] _equalInstances;
    private readonly Func<T>[] _otherInstances;

    public StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator(
      Func<T>[] equalInstances, 
      Func<T>[] otherInstances)
    {
      _equalInstances = equalInstances;
      _otherInstances = otherInstances;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      foreach (var factory in _equalInstances.Concat(_otherInstances))
      {
        var instance = factory();
        RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.True(Are.EqualInTermsOfEqualityOperator(
              typeof(T),
              instance,
              instance),
            "a == a should return true", violations), 
          "a == a should return true", violations);
      }

    }
  }
}