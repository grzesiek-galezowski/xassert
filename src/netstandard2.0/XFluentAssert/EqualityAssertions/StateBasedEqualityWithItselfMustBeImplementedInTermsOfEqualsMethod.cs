using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions;

internal class StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualsMethod<T> : IConstraint
{
  private readonly Func<T>[] _equalInstances;
  private readonly Func<T>[] _otherInstances;

  public StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualsMethod(
    Func<T>[] equalInstances, 
    Func<T>[] otherInstances)
  {
    _equalInstances = equalInstances;
    _otherInstances = otherInstances;
  }

  public void CheckAndRecord(ConstraintsViolations violations)
  {
    foreach (var factory in _equalInstances.Concat(_otherInstances))
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