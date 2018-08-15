namespace TddXt.XAssert.EqualsAssertions.EqualityOperator
{
  using TddXt.XAssert.AssertionConstraints;
  using TddXt.XAssert.ValueActivation;

  public class StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator
    : IConstraint
  {
    private readonly ValueObjectActivator _activator;

    public StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator(
      ValueObjectActivator activator)
    {
      _activator = activator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(Are.EqualInTermsOfEqualityOperator(
            _activator.TargetType, 
            instance1, 
            instance1),
          "a == a should return true", violations), "a == a should return true", violations);
    }
  }
}