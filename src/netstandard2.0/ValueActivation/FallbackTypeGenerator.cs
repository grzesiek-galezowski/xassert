namespace TddXt.XFluentAssert.ValueActivation
{
  using System;
  using System.Collections.Generic;

  using FluentAssertions;

  using TddXt.XFluentAssert.TypeReflection.Interfaces;

  public class FallbackTypeGenerator
  {
    private readonly IType _smartType;

    public FallbackTypeGenerator(IType smartType)
    {
      _smartType = smartType;
    }

    public int GetConstructorParametersCount()
    {
      var constructor = _smartType.PickConstructorWithLeastNonPointersParameters();
      return constructor.Value().GetParametersCount(); //bug backward compatibility (for now)
    }

    public object GenerateInstance(IEnumerable<object> constructorParameters)
    {
      var instance = _smartType.PickConstructorWithLeastNonPointersParameters().Value()  //bug backward compatibility (for now)
        .InvokeWith(constructorParameters);
      instance.GetType().Should().Be(_smartType.ToClrType());
      return instance;
    }

    public List<object> GenerateConstructorParameters(Func<Type, object> parameterFactory)
    {
      var constructor = _smartType.PickConstructorWithLeastNonPointersParameters();
      var constructorParameters = constructor.Value()  //bug backward compatibility (for now)
        .GenerateAnyParameterValues(parameterFactory);
      return constructorParameters;
    }
  }


}