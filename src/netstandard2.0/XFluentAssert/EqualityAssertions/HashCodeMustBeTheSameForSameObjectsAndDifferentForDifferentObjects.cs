using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions;

internal class HashCodeMustBeTheSameForSameObjectsAndDifferentForDifferentObjects<T>(
  Func<T>[] equalInstances,
  Func<T>[] otherInstances) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory1 in equalInstances)
    {
      foreach (var factory2 in otherInstances)
      {
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.NotEqual(factory1().GetHashCode(), factory2().GetHashCode(),
              "b.GetHashCode() and b.GetHashCode() should return different values for not equal objects", violations),
          "b.GetHashCode() and b.GetHashCode() should return different values for not equal objects", violations);
      }
    }

    foreach (var factory in equalInstances.Concat(otherInstances))
    {
      var instance = factory();
      RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.Equal(instance.GetHashCode(), instance.GetHashCode(),
            "a.GetHashCode() should consistently return the same value", violations),
        "a.GetHashCode() should consistently return the same value", violations);
    }

    foreach (var factory1 in equalInstances)
    {
      foreach (var factory2 in equalInstances)
      {
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.Equal(factory1().GetHashCode(), factory2().GetHashCode(),
              "a.GetHashCode() and b.GetHashCode() should be equal for equal objects",
              violations),
          "a.GetHashCode() and b.GetHashCode() should be equal for equal objects",
          violations);
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.Equal(factory1().GetHashCode(), factory2().GetHashCode(),
              "a.GetHashCode() and b.GetHashCode() should be equal for equal objects",
              violations),
          "a.GetHashCode() and b.GetHashCode() should be equal for equal objects",
          violations);
      }
    }
  }
}