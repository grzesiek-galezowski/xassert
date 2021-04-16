using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;
using TddXt.XFluentAssert.ValueActivation;

namespace TddXt.XFluentAssert.EqualityAssertions2
{
  public class UnEqualityWithNullMustBeImplementedInTermsOfEqualsMethod : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly ISmartType _smartType;

    public UnEqualityWithNullMustBeImplementedInTermsOfEqualsMethod(ValueObjectActivator activator,
      ISmartType smartType)
    {
      _activator = activator;
      _smartType = smartType;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(instance1.Equals(null),
        "a.Equals(null) should return false", violations),
        "a.Equals(null) should return false", violations);

      var equatableEquals = _smartType.EquatableEquality();
      if (equatableEquals.HasValue)
      {
        RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False((bool)equatableEquals.Value.Evaluate(instance1, null),
              "a.Equals(null) should return false", violations),
          "a.Equals(null) should return false", violations);
      }
    }
  }
}