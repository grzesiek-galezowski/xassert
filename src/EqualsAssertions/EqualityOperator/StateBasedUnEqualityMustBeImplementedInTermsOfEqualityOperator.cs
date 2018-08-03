using System;
using System.Linq;
using AssertionConstraints;
using ValueActivation;

namespace EqualsAssertions.EqualityOperator
{
  public class StateBasedUnEqualityMustBeImplementedInTermsOfEqualityOperator
    : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly int[] _indexesOfConstructorArgumentsToSkip;
    private readonly Func<Type, object, object, bool> _equalInTermsOfEqualityOperator;

    public StateBasedUnEqualityMustBeImplementedInTermsOfEqualityOperator(
      ValueObjectActivator activator, int[] indexesOfConstructorArgumentsToSkip, 
      Func<Type, object, object, bool> equalInTermsOfEqualityOperator)
    {
      _activator = activator;
      _indexesOfConstructorArgumentsToSkip = indexesOfConstructorArgumentsToSkip;
      _equalInTermsOfEqualityOperator = equalInTermsOfEqualityOperator;
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
            RecordedAssertions.False(_equalInTermsOfEqualityOperator(_activator.TargetType, instance1, instance2), "a == b should return false if both are created with different argument" + currentParamIndex, violations),
            "a == b should return false if both are created with different argument" + currentParamIndex, violations
          );
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(_equalInTermsOfEqualityOperator(_activator.TargetType, instance1, instance2), "b == a should return false if both are created with different argument" + currentParamIndex, violations),
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