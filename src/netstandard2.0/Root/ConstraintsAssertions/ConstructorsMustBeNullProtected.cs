﻿  using System;
  using System.Reflection;
  using TddXt.XFluentAssert.AssertionConstraints;
  using TddXt.XFluentAssert.TypeReflection;
  using TddXt.XFluentAssert.TypeReflection.Interfaces;
  using TddXt.XFluentAssert.ValueActivation;

  namespace TddXt.XFluentAssertRoot.ConstraintsAssertions
{

  using AnyRoot;
  
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
      var fallbackTypeGenerator = new ObjectGenerator(_smartType);

      foreach (var constructor in constructors)
      {
        AssertNullCheckForEveryPossibleArgumentOf(violations, constructor, fallbackTypeGenerator);
      }      
    }

    private static void AssertNullCheckForEveryPossibleArgumentOf(IConstraintsViolations violations,
      ICreateObjects constructor,
      ObjectGenerator objectGenerator)
    {
      for (int i = 0; i < constructor.GetParametersCount(); ++i)
      {
        var parameters = constructor.GenerateAnyParameterValues(Root.Any.InstanceAsObject);
        if (SmartType.ForTypeOf(parameters[i]).CanBeAssignedNullValue())
        {
          parameters[i] = null;

          try
          {
            objectGenerator.GenerateInstance(parameters);
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