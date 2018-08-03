using System;
using System.Linq;
using AssertionConstraints;
using ValueActivation;

namespace EqualsAssertions.InequalityOperator
{
  public class StateBasedUnEqualityMustBeImplementedInTermsOfInequalityOperator
    : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly int[] _indexesOfConstructorArgumentsToSkip;
    private Func<Type, object, object, bool> _areNotEqualInTermsOfInEqualityOperator;

    public StateBasedUnEqualityMustBeImplementedInTermsOfInequalityOperator(
      ValueObjectActivator activator, int[] indexesOfConstructorArgumentsToSkip, 
      Func<Type, object, object, bool> areNotEqualInTermsOfInEqualityOperator)
    {
      _activator = activator;
      _indexesOfConstructorArgumentsToSkip = indexesOfConstructorArgumentsToSkip;
      _areNotEqualInTermsOfInEqualityOperator = areNotEqualInTermsOfInEqualityOperator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      for (var i = 0; i < _activator.GetConstructorParametersCount(); ++i)
      {
        if (ArgumentIsPartOfValueIdentity(i))
        {
          var instance2 = _activator.CreateInstanceAsValueObjectWithModifiedParameter(i);

          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(_areNotEqualInTermsOfInEqualityOperator(_activator.TargetType, instance1, instance2), 
              "a != b should return true if both are created with different argument" + i, violations),
              "a != b should return true if both are created with different argument" + i, violations);
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(_areNotEqualInTermsOfInEqualityOperator(_activator.TargetType, instance1, instance2), 
              "b != a should return true if both are created with different argument" + i, violations),
              "b != a should return true if both are created with different argument" + i, violations);
        }
      }
    }

    private bool ArgumentIsPartOfValueIdentity(int i)
    {
      return !_indexesOfConstructorArgumentsToSkip.Contains(i);
    }
  }
}