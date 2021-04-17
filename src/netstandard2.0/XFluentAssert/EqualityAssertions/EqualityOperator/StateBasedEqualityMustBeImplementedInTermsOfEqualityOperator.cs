using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.EqualityOperator
{
  public class StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator<T> : IConstraint
  {
    private readonly Func<T>[] _equalInstances;

    public StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator(Func<T>[] equalInstances)
    {
      _equalInstances = equalInstances;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      foreach (var factory1 in _equalInstances)
      {
        foreach (var factory2 in _equalInstances)
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

}