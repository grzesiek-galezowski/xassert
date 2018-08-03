using System;
using AssertionConstraints;
using ValueActivation;

namespace EqualsAssertions.EqualityOperator
{
  public class StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly Func<Type, object, object, bool> _equalInTermsOfEqualityOperator;

    public StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator(
      ValueObjectActivator activator, 
      Func<Type, object, object, bool> equalInTermsOfEqualityOperator)
    {
      _activator = activator;
      _equalInTermsOfEqualityOperator = equalInTermsOfEqualityOperator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      var instance2 = _activator.CreateInstanceAsValueObjectWithPreviousParameters();

      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(_equalInTermsOfEqualityOperator(_activator.TargetType, instance1, instance2),
          "a == b should return true if both are created with the same arguments", violations),
        "a == b should return true if both are created with the same arguments", violations 
      );

      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(_equalInTermsOfEqualityOperator(_activator.TargetType, instance2, instance1),
          "b == a should return true if both are created with the same arguments", violations),
        "b == a should return true if both are created with the same arguments", violations
      );
    }
  }

}