using System;

using TddXt.XFluentAssert.AssertionConstraints;

namespace TddXt.XFluentAssert.ValueObjectConstraints;

internal class HasToBeAConcreteClass : IConstraint
{
  private readonly Type _type;

  public HasToBeAConcreteClass(Type type)
  {
    _type = type;
  }

  public void CheckAndRecord(ConstraintsViolations violations)
  {
    if (_type.IsAbstract)
    {
      violations.Add("SmartType " + _type + " is abstract but abstract classes cannot be value objects");
    }

    if (_type.IsInterface)
    {
      violations.Add("SmartType " + _type + " is an interface but interfaces cannot be value objects");
    }
  }
}