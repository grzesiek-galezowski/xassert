namespace TddXt.XAssert.AssertionConstraints
{
  public interface IConstraint
  {
    void CheckAndRecord(ConstraintsViolations violations);
  }
}