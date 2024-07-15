using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.EqualityOperator;

internal class StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator<T>(
  Func<T>[] equalInstances,
  Func<T>[] otherInstances) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory in equalInstances.Concat(otherInstances))
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