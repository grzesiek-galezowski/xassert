using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions;

internal class MustBeSealed(Type type) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(type.IsSealed,
          $"{type.Name} must be sealed, or derivatives will be able to override GetHashCode() with mutable code",
          violations),
      $"{type.Name} must be sealed, or derivatives will be able to override GetHashCode() with mutable code",
      violations);
  }
}