using System;
using System.Reflection;
using AssertionConstraints;
using TypeReflection;
using TypeReflection.Interfaces;
using ValueActivation;
using static TddXt.AnyRoot.Root;

namespace TddEbook.TddToolkit
{
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
      IConstructorWrapper constructor,
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