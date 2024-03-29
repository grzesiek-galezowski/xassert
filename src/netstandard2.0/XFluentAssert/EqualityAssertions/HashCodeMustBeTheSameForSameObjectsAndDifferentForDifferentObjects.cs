﻿using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions;

internal class HashCodeMustBeTheSameForSameObjectsAndDifferentForDifferentObjects<T> : IConstraint
{
  private readonly Func<T>[] _equalInstances;
  private readonly Func<T>[] _otherInstances;

  public HashCodeMustBeTheSameForSameObjectsAndDifferentForDifferentObjects(
    Func<T>[] equalInstances, 
    Func<T>[] otherInstances)
  {
    _equalInstances = equalInstances;
    _otherInstances = otherInstances;
  }

  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory1 in _equalInstances)
    {
      foreach (var factory2 in _otherInstances)
      {
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.NotEqual(factory1().GetHashCode(), factory2().GetHashCode(),
              "b.GetHashCode() and b.GetHashCode() should return different values for not equal objects", violations),
          "b.GetHashCode() and b.GetHashCode() should return different values for not equal objects", violations);
      }
    }

    foreach (var factory in _equalInstances.Concat(_otherInstances))
    {
      var instance = factory();
      RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.Equal(instance.GetHashCode(), instance.GetHashCode(),
            "a.GetHashCode() should consistently return the same value", violations),
        "a.GetHashCode() should consistently return the same value", violations);
    }

    foreach (var factory1 in _equalInstances)
    {
      foreach (var factory2 in _equalInstances)
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