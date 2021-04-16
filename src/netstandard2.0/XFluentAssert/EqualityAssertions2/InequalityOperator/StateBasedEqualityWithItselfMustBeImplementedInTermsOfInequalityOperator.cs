using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions2.InequalityOperator
{
  public class StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator
    : IConstraint
  {
    private readonly ValueObjectActivator _activator;

    public StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator(
      ValueObjectActivator activator)
    {
      _activator = activator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(Are.NotEqualInTermsOfInEqualityOperator(_activator.TargetType, instance1, instance1),
          "a != a should return false", violations),
          "a != a should return false", violations);
    }
  }
}