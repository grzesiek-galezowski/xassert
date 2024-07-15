using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.EqualityOperator;

internal class StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator<T>(Func<T>[] equalInstances) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory1 in equalInstances)
    {
      foreach (var factory2 in equalInstances)
      {
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(Are.EqualInTermsOfEqualityOperator(typeof(T), factory1(), factory2()),
              "a == b should return true if both are created with the same arguments that define equality", violations),
          "a == b should return true if both are created with the same arguments that define equality", violations
        );

        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(Are.EqualInTermsOfEqualityOperator(typeof(T), factory2(), factory1()),
              "b == a should return true if both are created with the same arguments that define equality", violations),
          "b == a should return true if both are created with the same arguments that define equality", violations
        );
      }
    }

  }
}