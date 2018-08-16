namespace TddXt.XFluentAssert.EqualityAssertions
{
  using TddXt.XFluentAssert.AssertionConstraints;
  using TddXt.XFluentAssert.ValueActivation;

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