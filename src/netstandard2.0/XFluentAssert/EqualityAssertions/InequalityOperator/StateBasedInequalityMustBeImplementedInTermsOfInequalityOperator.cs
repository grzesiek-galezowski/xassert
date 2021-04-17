using System;
using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.EqualityAssertions.InequalityOperator
{
  public class StateBasedInequalityMustBeImplementedInTermsOfInequalityOperator<T>
    : IConstraint
  {
    private readonly int[] _indexesOfConstructorArgumentsToSkip;
    private readonly Func<T>[] _equalInstances;
    private readonly Func<T>[] _otherInstances;

    public StateBasedInequalityMustBeImplementedInTermsOfInequalityOperator(
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
            RecordedAssertions.True(Are.NotEqualInTermsOfInEqualityOperator(typeof(T), factory1(), factory2()),
              "a != b should return true if they are not equal", violations),
              "a != b should return true if they are not equal", violations);
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True(Are.NotEqualInTermsOfInEqualityOperator(typeof(T), factory1(), factory2()),
              "b != a should return true if if they are not equal", violations),
              "b != a should return true if if they are not equal", violations);
        }
      }
    }
  }
}