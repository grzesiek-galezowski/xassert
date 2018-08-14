﻿using System;
using System.Reflection;

using static TddXt.AnyRoot.Root;

namespace TddEbook.TddToolkit
{
  using TddXt.XAssert.AssertionConstraints;
  using TddXt.XAssert.TypeReflection;
  using TddXt.XAssert.TypeReflection.Interfaces;
  using TddXt.XAssert.ValueActivation;

  public class ConstructorsMustBeNullProtected : IConstraint
  {
    private readonly ISmartType _smartType;

    public ConstructorsMustBeNullProtected(ISmartType smartType)
    {
      _smartType = smartType;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      var constructors = _smartType.GetAllPublicConstructors();
      var fallbackTypeGenerator = new FallbackTypeGenerator(_smartType);

      foreach (var constructor in constructors)
      {
        AssertNullCheckForEveryPossibleArgumentOf(violations, constructor, fallbackTypeGenerator);
      }      
    }

    private static void AssertNullCheckForEveryPossibleArgumentOf(IConstraintsViolations violations,
      ICreateObjects constructor,
      FallbackTypeGenerator fallbackTypeGenerator)
    {
      for (int i = 0; i < constructor.GetParametersCount(); ++i)
      {
        var parameters = constructor.GenerateAnyParameterValues(Any.InstanceAsObject);
        if (SmartType.ForTypeOf(parameters[i]).CanBeAssignedNullValue())
        {
          parameters[i] = null;

          try
          {
            fallbackTypeGenerator.GenerateInstance(parameters);
            violations.Add("Not guarded against nulls: " + constructor + ", Not guarded parameter: " +
                           constructor.GetDescriptionForParameter(i));
          }
          catch (TargetInvocationException exception)
          {
            if (exception.InnerException.GetType() == typeof (ArgumentNullException))
            {
              //do nothing, this is the expected case
            }
            else
            {
              throw;
            }
          }
        }
      }
    }
  }
}