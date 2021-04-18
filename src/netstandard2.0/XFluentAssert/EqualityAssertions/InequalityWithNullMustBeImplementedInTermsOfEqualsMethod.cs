using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions
{
  internal class InequalityWithNullMustBeImplementedInTermsOfEqualsMethod<T> : IConstraint
  {
    private readonly Func<T>[] _equalInstances;
    private readonly Func<T>[] _otherInstances;

    public InequalityWithNullMustBeImplementedInTermsOfEqualsMethod(
      Func<T>[] equalInstances, 
      Func<T>[] otherInstances)
    {
      _equalInstances = equalInstances;
      _otherInstances = otherInstances;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      if (!typeof(T).IsValueType)
      {
        foreach (var factory in _equalInstances.Concat(_otherInstances))
        {
          var instance1 = factory();
          RecordedAssertions.DoesNotThrow(() =>
              RecordedAssertions.False(instance1.Equals(null),
                "a.Equals(null) should return false", violations),
            "a.Equals(null) should return false", violations);

          var equatableEquals = SmartType.ForTypeOf(instance1).EquatableEquality();
          if (equatableEquals.HasValue)
          {
            RecordedAssertions.DoesNotThrow(() =>
                RecordedAssertions.False((bool) equatableEquals.Value.Evaluate(instance1, null),
                  "a.Equals(null) should return false", violations),
              "a.Equals(null) should return false", violations);
          }
        }
      }
    }
  }
}