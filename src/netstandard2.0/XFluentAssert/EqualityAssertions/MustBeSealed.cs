using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions;

internal class MustBeSealed : IConstraint
{
  private readonly Type _type;

  public MustBeSealed(Type type)
  {
    _type = type;
  }

  public void CheckAndRecord(ConstraintsViolations violations)
  {
    RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.True(_type.IsSealed,
          $"{_type.Name} must be sealed, or derivatives will be able to override GetHashCode() with mutable code",
          violations),
      $"{_type.Name} must be sealed, or derivatives will be able to override GetHashCode() with mutable code",
      violations);
  }
}