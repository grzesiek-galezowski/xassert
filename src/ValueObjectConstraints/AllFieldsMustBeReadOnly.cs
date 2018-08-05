﻿using System;
using System.Linq;
using AssertionConstraints;
using TypeReflection;

namespace ValueObjectConstraints
{
  public class AllFieldsMustBeReadOnly : IConstraint
  {
    private readonly Type _type;

    public AllFieldsMustBeReadOnly(Type type)
    {
      _type = type;
    }


    public void CheckAndRecord(ConstraintsViolations violations)
    {
      CheckImmutability(violations, _type);
    }

    private void CheckImmutability(ConstraintsViolations violations, Type type)
    {
      var fields = SmartType.For(type).GetAllInstanceFields().ToList();
      var fieldWrappers = fields
        .Where(item => item.IsNotDeveloperDefinedReadOnlyField());

      foreach (var item in fieldWrappers)
      {
        violations.Add(item.ShouldNotBeMutableButIs());
      }
    }
  }
}