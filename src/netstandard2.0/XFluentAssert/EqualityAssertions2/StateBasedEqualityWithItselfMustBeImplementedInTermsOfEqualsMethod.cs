using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions2
{
  public class StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualsMethod : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly ISmartType _smartType;

    public StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualsMethod(
      ValueObjectActivator activator,
      ISmartType smartType)
    {
      _activator = activator;
      _smartType = smartType;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      RecordedAssertions.DoesNotThrow(() =>
      {
        var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
        RecordedAssertions.DoesNotThrow(() =>
          RecordedAssertions.True(instance1.Equals(instance1),
            "a.Equals(object a) should return true", violations),
            "a.Equals(object a) should return true", violations);
      }, "Should be able to create an object of type " + _activator.TargetType, violations);

      var equatableEquals = _smartType.EquatableEquality();
      if (equatableEquals.HasValue)
      {
        var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.True((bool)equatableEquals.Value.Evaluate(instance1, instance1),
              "a.Equals(a) should return true", violations),
          "a.Equals(a) should return true", violations);

      }

    }
  }
}