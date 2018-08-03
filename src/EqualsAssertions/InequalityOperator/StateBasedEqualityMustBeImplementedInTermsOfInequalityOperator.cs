using System;
using AssertionConstraints;
using TddEbook.TddToolkit.ImplementationDetails;
using TddEbook.TddToolkit.ImplementationDetails.Common;

namespace TddEbook.TddToolkit.Helpers.Constraints.InequalityOperator
{
  public class StateBasedEqualityMustBeImplementedInTermsOfInequalityOperator : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private Func<Type, object, object, bool> _areNotEqualInTermsOfInEqualityOperator;

    public StateBasedEqualityMustBeImplementedInTermsOfInequalityOperator(ValueObjectActivator activator, Func<Type, object, object, bool> areNotEqualInTermsOfInEqualityOperator)
    {
      _activator = activator;
      _areNotEqualInTermsOfInEqualityOperator = areNotEqualInTermsOfInEqualityOperator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      var instance2 = _activator.CreateInstanceAsValueObjectWithPreviousParameters();

      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(_areNotEqualInTermsOfInEqualityOperator(_activator.TargetType, instance1, instance2), 
          "a != b should return false if both are created with the same arguments", violations),
          "a != b should return false if both are created with the same arguments", violations);
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(_areNotEqualInTermsOfInEqualityOperator(_activator.TargetType, instance2, instance1), 
          "b != a should return false if both are created with the same arguments", violations),
          "b != a should return false if both are created with the same arguments", violations);
    }
  }
}