namespace TddXt.XAssert.EqualsAssertions.InequalityOperator
{
  using System;

  using TddXt.XAssert.AssertionConstraints;

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
