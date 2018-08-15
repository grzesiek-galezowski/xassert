﻿namespace TddXt.XAssert.EqualsAssertions
{
  using System.Linq;

  using TddXt.XAssert.AssertionConstraints;
  using TddXt.XAssert.ValueActivation;

  public class StateBasedUnEqualityMustBeImplementedInTermsOfEqualsMethod : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly int[] _indexesOfConstructorArgumentsToSkip;

    public StateBasedUnEqualityMustBeImplementedInTermsOfEqualsMethod(
      ValueObjectActivator activator, int[] indexesOfConstructorArgumentsToSkip)
    {
      _activator = activator;
      _indexesOfConstructorArgumentsToSkip = indexesOfConstructorArgumentsToSkip;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      for (var i = 0; i < _activator.GetConstructorParametersCount(); ++i)
      {
        var currentParamIndex = i;

        if (ArgumentIsPartOfValueIdentity(i))
        {
          var instance2 = _activator.CreateInstanceAsValueObjectWithModifiedParameter(i);

          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(instance1.Equals(instance2),
            "a.Equals(b) should return false if both are created with different argument" + currentParamIndex, violations),
            "a.Equals(b) should return false if both are created with different argument" + currentParamIndex, violations);
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(instance2.Equals(instance1),
            "b.Equals(a) should return false if both are created with different argument" + currentParamIndex, violations),
            "b.Equals(a) should return false if both are created with different argument" + currentParamIndex, violations);
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(instance1.Equals(instance2),
            "(object)a.Equals((object)b) should return false if both are created with different argument" + currentParamIndex, violations),
            "(object)a.Equals((object)b) should return false if both are created with different argument" + currentParamIndex, violations);
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(instance2.Equals(instance1),
            "(object)b.Equals((object)a) should return false if both are created with different argument" + currentParamIndex, violations),
            "(object)b.Equals((object)a) should return false if both are created with different argument" + currentParamIndex, violations);
        }
      }
    }

    private bool ArgumentIsPartOfValueIdentity(int i)
    {
      return !_indexesOfConstructorArgumentsToSkip.Contains(i);
    }
  }
}