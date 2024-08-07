﻿using System;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions;

internal class StateBasedInequalityMustBeImplementedInTermsOfEqualsMethod<T>(
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
            RecordedAssertions.False(factory1().Equals(factory2()),
              "a.Equals(object b) should return false if they are not equal", violations),
          "a.Equals(object b) should return false if they are not equal", violations);
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(factory2().Equals(factory1()),
              "b.Equals(object a) should return false if they are not equal", violations),
          "b.Equals(object a) should return false if they are not equal", violations);

        var equatableEquals = TypeOf<T>.EquatableEquality();

        if (equatableEquals.HasValue)
        {
          RecordedAssertions.DoesNotThrow(() =>
              RecordedAssertions.False((bool)equatableEquals.Value().Evaluate(factory1(), factory2()),
                "a.Equals(T b) should return false if they are not equal", violations),
            "a.Equals(T b) should return false if they are not equal", violations);
          RecordedAssertions.DoesNotThrow(() =>
              RecordedAssertions.False((bool)equatableEquals.Value().Evaluate(factory2(), factory1()),
                "b.Equals(T a) should return false if they are not equal", violations),
            "b.Equals(T a) should return false if they are not equal", violations);
        }
      }
    }
  }
}