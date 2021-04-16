﻿using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions2.EqualityOperator
{
  public class StateBasedInequalityMustBeImplementedInTermsOfEqualityOperator
    : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly int[] _indexesOfConstructorArgumentsToSkip;

    public StateBasedInequalityMustBeImplementedInTermsOfEqualityOperator(
      ValueObjectActivator activator,
      int[] indexesOfConstructorArgumentsToSkip)
    {
      _activator = activator;
      _indexesOfConstructorArgumentsToSkip = indexesOfConstructorArgumentsToSkip;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      for (var i = 0; i < _activator.GetConstructorParametersCount(); ++i)
      {
        int currentParamIndex = i;
        if (ArgumentIsPartOfValueIdentity(i))
        {
          var instance2 = _activator.CreateInstanceAsValueObjectWithModifiedParameter(i);

          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(_activator.TargetType, instance1, instance2),
              "a == b should return false if both are created with different argument" + currentParamIndex, violations),
            "a == b should return false if both are created with different argument" + currentParamIndex, violations
          );
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(_activator.TargetType, instance1, instance2),
              "b == a should return false if both are created with different argument" + currentParamIndex, violations),
            "b == a should return false if both are created with different argument" + currentParamIndex, violations
          );
        }
      }
    }

    private bool ArgumentIsPartOfValueIdentity(int i)
    {
      return !_indexesOfConstructorArgumentsToSkip.Contains(i);
    }
  }
}