using System;
using AssertionConstraints;

namespace EqualsAssertions.EqualityOperator
{
  public class StateBasedEqualityShouldBeAvailableInTermsOfEqualityOperator
    : IConstraint
  {
    private readonly Type _type;
    private readonly Action<Type> _assertEqualityOperatorDefinedFor;

    public StateBasedEqualityShouldBeAvailableInTermsOfEqualityOperator(Type type, Action<Type> assertEqualityOperatorDefinedFor)
    {
      _type = type;
      _assertEqualityOperatorDefinedFor = assertEqualityOperatorDefinedFor;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
 	    _assertEqualityOperatorDefinedFor(_type);
    }
}
}
