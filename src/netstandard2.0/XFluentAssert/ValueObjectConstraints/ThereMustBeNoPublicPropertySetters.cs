using System;
using System.Linq;

using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.ValueObjectConstraints
{
  internal class ThereMustBeNoPublicPropertySetters : IConstraint
  {
    private readonly Type _type;

    public ThereMustBeNoPublicPropertySetters(Type type)
    {
      _type = type;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var properties = SmartType.For(_type).GetAllPublicInstanceProperties();

      foreach (var item in properties.Where(item => item.HasPublicSetter()))
      {
        violations.Add(item.ShouldNotBeMutableButIs());
      }
    }
  }
}