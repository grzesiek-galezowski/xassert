namespace TddXt.XAssert.EqualsAssertions
{
  using TddXt.XAssert.AssertionConstraints;
  using TddXt.XAssert.ValueActivation;

  public class UnEqualityWithNullMustBeImplementedInTermsOfEqualsMethod : IConstraint
  {
    private readonly ValueObjectActivator _activator;

    public UnEqualityWithNullMustBeImplementedInTermsOfEqualsMethod(ValueObjectActivator activator)
    {
      _activator = activator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(instance1.Equals(null), 
        "a.Equals(null) should return false", violations),
        "a.Equals(null) should return false", violations);
    }
  }
}