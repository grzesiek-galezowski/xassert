using System;
using System.Linq;

using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.ValueObjectConstraints;

internal class AllFieldsMustBeReadOnly(Type type) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    CheckImmutability(violations, type);
  }

  private void CheckImmutability(ConstraintsViolations violations, Type type)
  {
    var fields = SmartType.For(type).GetAllInstanceFields().ToList();
    var fieldWrappers = fields
      .Where(item => item.IsNotDeveloperDefinedReadOnlyField())
      .Where(item => item.IsNotSpecialCase());

    foreach (var item in fieldWrappers)
    {
      violations.Add(item.ShouldNotBeMutableButIs());
    }
  }
}