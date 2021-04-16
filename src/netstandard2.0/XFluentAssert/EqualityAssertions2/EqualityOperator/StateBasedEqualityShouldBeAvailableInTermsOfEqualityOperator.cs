using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions2.EqualityOperator
{
  public class StateBasedEqualityShouldBeAvailableInTermsOfEqualityOperator
    : IConstraint
  {
    private readonly Type _type;

    public StateBasedEqualityShouldBeAvailableInTermsOfEqualityOperator(Type type)
    {
      _type = type;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      EqualityExistenceAssertions.AssertEqualityOperatorIsDefinedFor(_type);
    }
  }
}
