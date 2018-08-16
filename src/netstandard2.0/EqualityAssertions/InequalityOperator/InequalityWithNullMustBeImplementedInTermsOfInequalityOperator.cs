namespace TddXt.XFluentAssert.EqualityAssertions.InequalityOperator
{
  using TddXt.XFluentAssert.AssertionConstraints;
  using TddXt.XFluentAssert.ValueActivation;

  public class InequalityWithNullMustBeImplementedInTermsOfInequalityOperator : IConstraint
  {
    private readonly ValueObjectActivator _activator;

    public InequalityWithNullMustBeImplementedInTermsOfInequalityOperator(
      ValueObjectActivator activator)
    {
      _activator = activator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(
          Are.NotEqualInTermsOfInEqualityOperator(_activator.TargetType, instance1, null), 
          "a != null should return true", violations),
          "a != null should return true", violations);
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(
          Are.NotEqualInTermsOfInEqualityOperator(_activator.TargetType, null, instance1), 
          "null != a should return true", violations),
        "null != a should return true", violations);
    }
  }
}