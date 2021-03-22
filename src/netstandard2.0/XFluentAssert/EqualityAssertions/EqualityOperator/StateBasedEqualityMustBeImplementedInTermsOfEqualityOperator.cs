﻿using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions.EqualityOperator
{
  public class StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator : IConstraint
  {
    private readonly ValueObjectActivator _activator;

    public StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator(
      ValueObjectActivator activator)
    {
      _activator = activator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      var instance2 = _activator.CreateInstanceAsValueObjectWithPreviousParameters();

      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(Are.EqualInTermsOfEqualityOperator(_activator.TargetType, instance1, instance2),
          "a == b should return true if both are created with the same arguments", violations),
        "a == b should return true if both are created with the same arguments", violations
      );

      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(Are.EqualInTermsOfEqualityOperator(_activator.TargetType, instance2, instance1),
          "b == a should return true if both are created with the same arguments", violations),
        "b == a should return true if both are created with the same arguments", violations
      );
    }
  }

}