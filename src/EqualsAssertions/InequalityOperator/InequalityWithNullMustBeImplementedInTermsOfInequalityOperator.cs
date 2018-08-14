using TddEbook.TddToolkit;
using ValueActivation;

namespace EqualsAssertions.InequalityOperator
{
  using TddXt.XAssert.AssertionConstraints;

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
        RecordedAssertions.True(Are.NotEqualInTermsOfInEqualityOperator(
            _activator.TargetType, instance1, null), 
          "a != null should return true", violations),
          "a != null should return true", violations);
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(Are.NotEqualInTermsOfInEqualityOperator(
            _activator.TargetType, null, instance1), 
          "null != a should return true", violations),
          "null != a should return true", violations);
      
      //TODO remove? 
      object null1 = null;
      object null2 = null;

      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(null1 != null2, 
          "null != null should be false", violations),
          "null != null should be false", violations);
    }
  }
}