using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.EqualityOperator;

internal class StateBasedEqualityShouldBeAvailableInTermsOfEqualityOperator(Type type) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    EqualityExistenceAssertions.AssertEqualityOperatorIsDefinedFor(type);
  }
}