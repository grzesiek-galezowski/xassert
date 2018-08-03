using System;
using AssertionConstraints;
using ValueActivation;

namespace EqualsAssertions.EqualityOperator
{
  public class StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator
    : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly Func<Type, object, object, bool> _assertEqualInTermsOfEqualityOperator;

    public StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator(
      ValueObjectActivator activator, 
      Func<Type, object, object, bool> assertEqualInTermsOfEqualityOperator)
    {
      _activator = activator;
      _assertEqualInTermsOfEqualityOperator = assertEqualInTermsOfEqualityOperator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(_assertEqualInTermsOfEqualityOperator(
            _activator.TargetType, 
            instance1, 
            instance1),
          "a == a should return true", violations), "a == a should return true", violations);
    }
  }
}