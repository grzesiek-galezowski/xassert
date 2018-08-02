﻿using TddEbook.TddToolkit.ImplementationDetails;
using TddEbook.TddToolkit.ImplementationDetails.Common;
using TddEbook.TddToolkit.ImplementationDetails.ConstraintAssertions;
using TddEbook.TddToolkit.ImplementationDetails.ConstraintAssertions.CustomCollections;

namespace TddEbook.TddToolkit.Helpers.Constraints.InequalityOperator
{
  public class StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator
    : IConstraint
  {
    private readonly ValueObjectActivator _activator;

    public StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator(ValueObjectActivator activator)
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