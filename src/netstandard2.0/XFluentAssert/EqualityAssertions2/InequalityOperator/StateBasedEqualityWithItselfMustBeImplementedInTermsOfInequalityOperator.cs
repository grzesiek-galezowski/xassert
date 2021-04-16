using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions2.InequalityOperator
{
  public class StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator<T>
    : IConstraint
  {
    private readonly Func<T>[] _equalInstances;
    private readonly Func<T>[] _otherInstances;

    public StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator(
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
          RecordedAssertions.False(Are.NotEqualInTermsOfInEqualityOperator(typeof(T), instance, instance),
            "a != a should return false", violations),
            "a != a should return false", violations);
        
      }
    }
  }
}