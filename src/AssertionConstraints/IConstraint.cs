namespace AssertionConstraints
{
  public interface IConstraint
  {
    void CheckAndRecord(ConstraintsViolations violations);
  }
}