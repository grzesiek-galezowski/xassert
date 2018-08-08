using System;
using AssertionConstraints;
using TddEbook.TddToolkit;

namespace EqualsAssertions.EqualityOperator
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
