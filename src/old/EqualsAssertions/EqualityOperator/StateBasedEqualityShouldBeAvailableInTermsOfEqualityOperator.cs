namespace TddXt.XAssert.EqualsAssertions.EqualityOperator
{
  using System;

  using TddXt.XAssert.AssertionConstraints;

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
