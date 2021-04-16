using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions2.InequalityOperator
{
  public class StateBasedEqualityShouldBeAvailableInTermsOfInequalityOperator : IConstraint
  {
    private readonly Type _type;

    public StateBasedEqualityShouldBeAvailableInTermsOfInequalityOperator(Type type)
    {
      _type = type;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      EqualityExistenceAssertions.AssertInequalityOperatorIsDefinedFor(_type);
    }
  }
}
