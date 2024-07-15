using System;
using System.Linq;

using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.ValueObjectConstraints;

internal class ThereMustBeNoPublicPropertySetters(Type type) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    var properties = SmartType.For(type).GetAllPublicInstanceProperties();

    foreach (var item in properties.Where(item => item.HasPublicSetter()))
    {
      violations.Add(item.ShouldNotBeMutableButIs());
    }
  }
}