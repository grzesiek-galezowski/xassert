using System;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions2.EqualityOperator
{
  public class StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator<T> : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly Func<T>[] _equalInstances;

    public StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator(Func<T>[] equalInstances)
    {
      _equalInstances = equalInstances;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      foreach (var instance1 in _equalInstances)
      {
        foreach (var instance2 in _equalInstances)
        {
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(Are.EqualInTermsOfEqualityOperator(_activator.TargetType, instance1, instance2),
              "a == b should return true if both are created with the same arguments that define equality", violations),
            "a == b should return true if both are created with the same arguments that define equality", violations
          );

          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(Are.EqualInTermsOfEqualityOperator(_activator.TargetType, instance2, instance1),
              "b == a should return true if both are created with the same arguments that define equality", violations),
            "b == a should return true if both are created with the same arguments that define equality", violations
          );
        }
      }

    }
  }

}