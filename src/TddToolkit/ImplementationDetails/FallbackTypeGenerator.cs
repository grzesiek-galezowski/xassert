using System;
using System.Collections.Generic;
using TddEbook.TypeReflection;
using TypeReflection.Interfaces;

namespace TddEbook.TddToolkit.ImplementationDetails
{
  public class FallbackTypeGenerator
  {
    private readonly IType2 _smartType;
    private readonly Type _type;

    public FallbackTypeGenerator(Type type)
    {
      _smartType = SmartType.For(type);
      _type = type;
    }

    public int GetConstructorParametersCount()
    {
      var constructor = _smartType.PickConstructorWithLeastNonPointersParameters();
      return constructor.Value.GetParametersCount(); //bug backward compatibility (for now)
    }

    public object GenerateInstance(IInstanceGenerator instanceGenerator)
    {
      var instance = _smartType.PickConstructorWithLeastNonPointersParameters()
        .Value.InvokeWithParametersCreatedBy(instanceGenerator.Instance);
      XAssert.Equal(_type, instance.GetType());
      return instance;
    }

    public object GenerateInstance(IEnumerable<object> constructorParameters)
    {
      var instance = _smartType.PickConstructorWithLeastNonPointersParameters().Value  //bug backward compatibility (for now)
        .InvokeWith(constructorParameters);
      XAssert.Equal(_type, instance.GetType());
      return instance;
    }

    public List<object> GenerateConstructorParameters(Func<Type, object> parameterFactory)
    {
      var constructor = _smartType.PickConstructorWithLeastNonPointersParameters();
      var constructorParameters = constructor.Value  //bug backward compatibility (for now)
        .GenerateAnyParameterValues(parameterFactory);
      return constructorParameters;
    }

    public bool ConstructorIsInternalOrHasAtLeastOneNonConcreteArgumentType()
    {
      var constructor = _smartType.PickConstructorWithLeastNonPointersParameters();
      return constructor.Value //bug backward compatibility (for now)
        .HasAbstractOrInterfaceArguments()
      || constructor.Value.IsInternal();
    }


    public void FillFieldsAndPropertiesOf(object result, IInstanceGenerator instanceGenerator)
    {
      FillPropertyValues(result, instanceGenerator);
      FillFieldValues(result, instanceGenerator);
    }

    private void FillFieldValues(object result, IInstanceGenerator instanceGenerator)
    {
      var fields = _smartType.GetAllPublicInstanceFields();
      foreach (var field in fields)
      {
        try
        {
          field.SetValue(result, instanceGenerator.Instance(field.FieldType));
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
      }
    }

    private void FillPropertyValues(object result, IInstanceGenerator instanceGenerator)
    {
      var properties = _smartType.GetPublicInstanceWritableProperties();

      foreach (var property in properties)
      {
        try
        {
          var propertyType = property.PropertyType;

          if (!property.HasAbstractGetter())
          {
            var value = instanceGenerator.Instance(propertyType);
            property.SetValue(result, value);
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e.Message);
        }
      }
    }
  }


}