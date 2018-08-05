using System.Collections.Generic;

namespace AssertionConstraints
{
  public static class AssertionConstraintsEngine
  {
    public static void TypeAdheresTo(IEnumerable<IConstraint> constraints)
    {
      var violations = ConstraintsViolations.Empty();
      foreach (var constraint in constraints)
      {
        RecordedAssertions.DoesNotThrow(() => constraint.CheckAndRecord(violations),
          "Did not expect exception", violations);
      }

      violations.AssertNone();
    }
  }
}