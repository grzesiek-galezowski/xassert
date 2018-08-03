using System;
using AssertionConstraints;
using TddEbook.TddToolkit.ImplementationDetails;
using TddEbook.TddToolkit.ImplementationDetails.Common;

namespace EqualsAssertions.EqualityOperator
{
  public class UnEqualityWithNullMustBeImplementedInTermsOfEqualityOperator : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private Func<Type, object, object, bool> _equalInTermsOfEqualityOperator;

    public UnEqualityWithNullMustBeImplementedInTermsOfEqualityOperator(ValueObjectActivator activator, Func<Type, object, object, bool> equalInTermsOfEqualityOperator)
    {
      _activator = activator;
      _equalInTermsOfEqualityOperator = equalInTermsOfEqualityOperator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(_equalInTermsOfEqualityOperator(_activator.TargetType, instance1, null), 
          "a == null should return false", violations), 
        "a == null should return false", violations);

      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(_equalInTermsOfEqualityOperator(_activator.TargetType, null, instance1),
          "null == a should return false", violations),
        "null == a should return false", violations);
    }
  }
}