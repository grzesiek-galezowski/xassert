namespace TddXt.XFluentAssert.AssertionConstraints
{
  public interface IConstraint
  {
    void CheckAndRecord(ConstraintsViolations violations);
  }
}