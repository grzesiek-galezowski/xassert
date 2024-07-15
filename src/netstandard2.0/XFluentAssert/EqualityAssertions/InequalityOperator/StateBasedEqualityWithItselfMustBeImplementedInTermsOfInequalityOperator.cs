using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.InequalityOperator;

internal class StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator<T>(
  Func<T>[] equalInstances,
  Func<T>[] otherInstances) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory in equalInstances.Concat(otherInstances))
    {
      var instance = factory();
      RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.False(Are.NotEqualInTermsOfInEqualityOperator(typeof(T), instance, instance),
            "a != a should return false", violations),
        "a != a should return false", violations);
        
    }
  }
}