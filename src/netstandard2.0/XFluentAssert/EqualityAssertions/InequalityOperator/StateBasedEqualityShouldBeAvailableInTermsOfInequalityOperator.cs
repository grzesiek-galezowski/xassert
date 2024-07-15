using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.InequalityOperator;

internal class StateBasedEqualityShouldBeAvailableInTermsOfInequalityOperator(Type type) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    EqualityExistenceAssertions.AssertInequalityOperatorIsDefinedFor(type);
  }
}