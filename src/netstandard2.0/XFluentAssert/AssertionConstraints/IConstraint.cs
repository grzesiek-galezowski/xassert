namespace TddXt.XFluentAssert.AssertionConstraints;

internal interface IConstraint
{
  void CheckAndRecord(ConstraintsViolations violations);
}