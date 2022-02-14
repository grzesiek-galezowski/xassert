using System;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions
{
  internal class StateBasedEqualityMustBeImplementedInTermsOfEqualsMethod<T> : IConstraint
  {
    private readonly Func<T>[] _equalInstances;

    public StateBasedEqualityMustBeImplementedInTermsOfEqualsMethod(Func<T>[] equalInstances)
    {
      _equalInstances = equalInstances;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      RecordedAssertions.DoesNotThrow(() =>
      {
        var equatableEquality = TypeOf<T>.EquatableEquality();
        if (equatableEquality.HasValue)
        {
          foreach (var factory1 in _equalInstances)
          {
            foreach (var factory2 in _equalInstances)
            {
              RecordedAssertions.DoesNotThrow(() =>
                  RecordedAssertions.True((bool) equatableEquality.Value().Evaluate(factory1(), factory2()),
                    "a.Equals(T b) should return true for equal objects", violations),
                "a.Equals(T b) should return true for equal objects", violations);
              RecordedAssertions.DoesNotThrow(() =>
                  RecordedAssertions.True((bool) equatableEquality.Value().Evaluate(factory2(), factory1()),
                    "b.Equals<T>(a) should return true for equal objects", violations),
                "b.Equals(T a) should return true for equal objects", violations);

              RecordedAssertions.DoesNotThrow(() =>
                  RecordedAssertions.True(factory1().Equals(factory2()),
                    "a.Equals(object b) should return true for equal objects", violations),
                "a.Equals(object b) should return true for equal objects", violations);
              RecordedAssertions.DoesNotThrow(() =>
                  RecordedAssertions.True(factory2().Equals(factory1()),
                    "b.Equals(object a) should return true for equal objects", violations),
                "b.Equals(object a) should return true for equal objects", violations);
            }
          }
        }
      }, "Should be able to create an object of type " + typeof(T), violations);
    }
  }
}