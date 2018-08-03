using System;
using AssertionConstraints;

namespace EqualsAssertions.InequalityOperator
{
  public class StateBasedEqualityShouldBeAvailableInTermsOfInequalityOperator : IConstraint
  {
    private readonly Type _type;
    private Action<Type> _assertIsInequalityOperatorDefinedFor;

    public StateBasedEqualityShouldBeAvailableInTermsOfInequalityOperator(Type type, Action<Type> assertIsInequalityOperatorDefinedFor)
    {
      _type = type;
      _assertIsInequalityOperatorDefinedFor = assertIsInequalityOperatorDefinedFor;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
 	    _assertIsInequalityOperatorDefinedFor(_type);
    }
}
}
