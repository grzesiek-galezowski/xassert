using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions
{
  using System.Linq;

  using AssertionConstraints;
  using ValueActivation;

  public class StateBasedUnEqualityMustBeImplementedInTermsOfEqualsMethod : IConstraint
  {
    private readonly ValueObjectActivator _activator;
    private readonly int[] _indexesOfConstructorArgumentsToSkip;
    private readonly ISmartType _smartType;

    public StateBasedUnEqualityMustBeImplementedInTermsOfEqualsMethod(ValueObjectActivator activator,
      int[] indexesOfConstructorArgumentsToSkip, ISmartType smartType)
    {
      _activator = activator;
      _indexesOfConstructorArgumentsToSkip = indexesOfConstructorArgumentsToSkip;
      _smartType = smartType;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      for (var i = 0; i < _activator.GetConstructorParametersCount(); ++i)
      {
        var currentParamIndex = i;

        if (ArgumentIsPartOfValueIdentity(i))
        {
          var instance2 = _activator.CreateInstanceAsValueObjectWithModifiedParameter(i);

          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(instance1.Equals(instance2),
            "a.Equals(object b) should return false if both are created with different argument" + currentParamIndex, violations),
            "a.Equals(object b) should return false if both are created with different argument" + currentParamIndex, violations);
          RecordedAssertions.DoesNotThrow(() =>
            RecordedAssertions.False(instance2.Equals(instance1),
            "b.Equals(object a) should return false if both are created with different argument" + currentParamIndex, violations),
            "b.Equals(object a) should return false if both are created with different argument" + currentParamIndex, violations);

          var equatableEquals = _smartType.EquatableEquality();

          if (equatableEquals.HasValue)
          {
            RecordedAssertions.DoesNotThrow(() =>
                RecordedAssertions.False((bool)equatableEquals.Value().Evaluate(instance1, instance2),
                  "a.Equals(b) should return false if both are created with different argument" +
                  currentParamIndex, violations),
              "a.Equals((object)b) should return false if both are created with different argument" +
              currentParamIndex, violations);
            RecordedAssertions.DoesNotThrow(() =>
                RecordedAssertions.False((bool)equatableEquals.Value().Evaluate(instance2, instance1),
                  "b.Equals(a) should return false if both are created with different argument" +
                  currentParamIndex, violations),
              "b.Equals(a) should return false if both are created with different argument" +
              currentParamIndex, violations);
          }
        }
      }
    }

    private bool ArgumentIsPartOfValueIdentity(int i)
    {
      return !_indexesOfConstructorArgumentsToSkip.Contains(i);
    }
  }
}