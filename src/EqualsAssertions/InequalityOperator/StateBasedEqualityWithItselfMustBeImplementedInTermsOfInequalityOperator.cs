using System;
using AssertionConstraints;
using TddEbook.TddToolkit.ImplementationDetails;
using TddEbook.TddToolkit.ImplementationDetails.Common;

namespace EqualsAssertions.InequalityOperator
{
  public class StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator
    : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private Func<Type, object, object, bool> _areNotEqualInTermsOfInEqualityOperator;

    public StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator(
      ValueObjectActivator activator, 
      Func<Type, object, object, bool> areNotEqualInTermsOfInEqualityOperator)
    {
      _activator = activator;
      _areNotEqualInTermsOfInEqualityOperator = areNotEqualInTermsOfInEqualityOperator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(_areNotEqualInTermsOfInEqualityOperator(_activator.TargetType, instance1, instance1), 
          "a != a should return false", violations),
          "a != a should return false", violations);
    }
  }
}