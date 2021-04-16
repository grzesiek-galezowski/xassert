﻿using System;
using System.Linq;
using TddXt.AnyGenerators.Root.ImplementationDetails;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions2.InequalityOperator
{
  public class InequalityWithNullMustBeImplementedInTermsOfInequalityOperator<T> : IConstraint
  {
    private readonly Func<T>[] _equalInstances;
    private readonly Func<T>[] _otherInstances;

    public InequalityWithNullMustBeImplementedInTermsOfInequalityOperator(
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
        var instance1 = factory();
        RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.True(
            Are.NotEqualInTermsOfInEqualityOperator(typeof(T), instance1, null),
            "a != null should return true", violations),
            "a != null should return true", violations);
        RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.True(
            Are.NotEqualInTermsOfInEqualityOperator(typeof(T), null, instance1),
            "null != a should return true", violations),
          "null != a should return true", violations);
      }
    }
  }
}