﻿namespace TddXt.XFluentAssert.EqualityAssertions
{
  using AssertionConstraints;
  using ValueActivation;

  public class StateBasedEqualityMustBeImplementedInTermsOfEqualsMethod : IConstraint
  {
    private readonly ValueObjectActivator _activator;

    public StateBasedEqualityMustBeImplementedInTermsOfEqualsMethod(ValueObjectActivator activator)
    {
      _activator = activator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      RecordedAssertions.DoesNotThrow(() =>
      {
        var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
        var instance2 = _activator.CreateInstanceAsValueObjectWithPreviousParameters();

        //bug this does not test equatable!!!!
        RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.True(instance1.Equals(instance2),
            "a.Equals(b) should return true if both are created with the same arguments", violations),
            "a.Equals(b) should return true if both are created with the same arguments", violations);
        RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.True(instance2.Equals(instance1),
          "b.Equals(a) should return true if both are created with the same arguments", violations),
          "b.Equals(a) should return true if both are created with the same arguments", violations);
        //end of bug

        RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.True(instance1.Equals(instance2),
          "(object)a.Equals((object)b) should return true if both are created with the same arguments", violations),
          "(object)a.Equals((object)b) should return true if both are created with the same arguments", violations);
        RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.True(instance2.Equals(instance1),
          "(object)b.Equals((object)a) should return true if both are created with the same arguments", violations),
          "(object)b.Equals((object)a) should return true if both are created with the same arguments", violations);
      }, "Should be able to create an object of type " + _activator.TargetType, violations);
    }
  }
}