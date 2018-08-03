using System;
using System.Collections.Generic;
using TddEbook.TypeReflection;
using TypeReflection.Interfaces;

namespace TddEbook.TddToolkit.ImplementationDetails
{
  public class FallbackTypeGenerator
  {
    private readonly IType _smartType;
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
  }


}