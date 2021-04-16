using System;
using System.Linq;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions2.EqualityOperator
{
  public class InequalityWithNullMustBeImplementedInTermsOfEqualityOperator<T> : IConstraint
  {
    private readonly Func<T>[] _equalInstances;
    private readonly Func<T>[] _otherInstances;

    public InequalityWithNullMustBeImplementedInTermsOfEqualityOperator(
      Func<T>[] equalInstances, Func<T>[] otherInstances)
    {
      _equalInstances = equalInstances;
      _otherInstances = otherInstances;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      foreach (var factory in _equalInstances.Concat(_otherInstances))
      {
        var instance1 = factory();
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(typeof(T), instance1, null),
              "a == null should return false", violations),
          "a == null should return false", violations);

        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(typeof(T), null, instance1),
              "null == a should return false", violations),
          "null == a should return false", violations);
      }
    }
  }
}