using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions;

internal class StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualsMethod<T>(
  Func<T>[] equalInstances,
  Func<T>[] otherInstances) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory in equalInstances.Concat(otherInstances))
    {
      RecordedAssertions.DoesNotThrow(() =>
      {
        var instance1 = factory();

        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(instance1.Equals(instance1),
              "a.Equals(object a) should return true", violations),
          "a.Equals(object a) should return true", violations);
      }, "Should be able to create an object of type " + typeof(T), violations);

      var equatableEquals = TypeOf<T>.EquatableEquality();
      if (equatableEquals.HasValue)
      {
        var instance1 = factory();
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True((bool)equatableEquals.Value().Evaluate(instance1, instance1),
              "a.Equals(a) should return true", violations),
          "a.Equals(a) should return true", violations);
      }
    }
  }
}