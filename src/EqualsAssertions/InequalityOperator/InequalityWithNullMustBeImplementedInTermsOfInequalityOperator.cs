using System;
using AssertionConstraints;
using TddEbook.TddToolkit.ImplementationDetails;
using TddEbook.TddToolkit.ImplementationDetails.Common;

namespace TddEbook.TddToolkit.Helpers.Constraints.InequalityOperator
{
  public class InequalityWithNullMustBeImplementedInTermsOfInequalityOperator : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly Func<Type, object, object, bool> _areNotEqualInTermsOfInEqualityOperator;

    public InequalityWithNullMustBeImplementedInTermsOfInequalityOperator(
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
        RecordedAssertions.True(_areNotEqualInTermsOfInEqualityOperator(_activator.TargetType, instance1, null), 
          "a != null should return true", violations),
          "a != null should return true", violations);
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(_areNotEqualInTermsOfInEqualityOperator(_activator.TargetType, null, instance1), 
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