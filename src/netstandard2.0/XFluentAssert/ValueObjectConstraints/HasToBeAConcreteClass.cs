using System;

using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.ValueObjectConstraints;

internal class HasToBeAConcreteClass(Type type) : IConstraint
{
  public void CheckAndRecord(ConstraintsViolations violations)
  {
    if (type.IsAbstract)
    {
      violations.Add("SmartType " + type + " is abstract but abstract classes cannot be value objects");
    }

    if (type.IsInterface)
    {
      violations.Add("SmartType " + type + " is an interface but interfaces cannot be value objects");
    }
  }
}