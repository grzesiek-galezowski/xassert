namespace TddXt.XFluentAssert.EqualityAssertions.EqualityOperator
{
  using System;

  using AssertionConstraints;

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
