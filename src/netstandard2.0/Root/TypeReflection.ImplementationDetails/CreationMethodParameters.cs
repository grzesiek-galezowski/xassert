namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  public class CreationMethodParameters
  {
    private readonly ParameterInfo[] _parameters;
    private readonly bool _hasAbstractOrInterfaceArguments;
    private readonly IEnumerable<TypeInfo> _parameterTypes;

    public CreationMethodParameters(ParameterInfo[] parameters)
    {
      _parameters = parameters;
      _parameterTypes =
        parameters.Select(p => IntrospectionExtensions.GetTypeInfo(p.ParameterType));
      _hasAbstractOrInterfaceArguments =
        parameters.Select(p => p.ParameterType.GetTypeInfo())
          .Any(type => type.IsAbstract || type.IsInterface);
    }

    public bool ContainAnyPointer()
    {
      return _parameterTypes.Any(type => type.IsPointer);
    }

    public bool AreLessThan(int numberOfParams)
    {
      return _parameters.Length < numberOfParams;
    }

    public int Count()
    {
      return _parameters.Length;
    }

    public void FillWithGeneratedValues(Func<Type, object> instanceGenerator, List<object> constructorValues)
    {
      foreach (var constructorParam in _parameterTypes)
      {
        constructorValues.Add(instanceGenerator(constructorParam));
      }
    }

    public string GetDescriptionFor(int i)
    {
      ParameterInfo parameter = _parameters[i];
      return parameter.ParameterType.Name + " " + parameter.Name;
    }

    public override string ToString()
    {
      int parametersCount = Count();
      string argsDescription = "";
      for (int i = 0; i < parametersCount; ++i)
      {
        argsDescription += GetDescriptionFor(i) + Separator(i, parametersCount);
      }

      return argsDescription;
    }

    private static string Separator(int i, int parametersCount)
    {
      return i == parametersCount - 1 ? "" : ", ";
    }

    public bool IsAnyOfType(Type type)
    {
      return _parameters.Any(p => p.ParameterType == type);
    }

  }
}