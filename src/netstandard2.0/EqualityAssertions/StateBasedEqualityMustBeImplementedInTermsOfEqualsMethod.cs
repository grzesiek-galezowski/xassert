using System;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions
{
  using AssertionConstraints;
  using ValueActivation;

  public class StateBasedEqualityMustBeImplementedInTermsOfEqualsMethod : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly ISmartType _type;

    public StateBasedEqualityMustBeImplementedInTermsOfEqualsMethod(
      ValueObjectActivator activator, 
      ISmartType type)
    {
      _activator = activator;
      _type = type;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      RecordedAssertions.DoesNotThrow(() =>
      {
        var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
        var instance2 = _activator.CreateInstanceAsValueObjectWithPreviousParameters();

        var equatableEquality = _type.EquatableEquality();
        if (equatableEquality.HasValue)
        {
          RecordedAssertions.DoesNotThrow(() =>
              RecordedAssertions.True((bool)equatableEquality.Value().Evaluate(instance1, instance2),
            "a.Equals(b) should return true if both are created with the same arguments", violations),
          "a.Equals(b) should return true if both are created with the same arguments", violations);
          RecordedAssertions.DoesNotThrow(() =>
              RecordedAssertions.True((bool)equatableEquality.Value().Evaluate(instance2, instance1),
                "b.Equals(a) should return true if both are created with the same arguments", violations),
            "b.Equals(a) should return true if both are created with the same arguments", violations);
          //end of bug
        }
        
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