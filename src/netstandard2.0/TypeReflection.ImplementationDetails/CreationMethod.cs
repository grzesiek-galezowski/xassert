namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  using Interfaces;

  public class CreationMethod : ICreateObjects
  {
    public static CreationMethod FromConstructorInfo(ConstructorInfo constructor)
    {
      return new CreationMethod(constructor, constructor.Invoke, 
        constructor.DeclaringType, 
        new CreationMethodParameters(constructor.GetParameters()));
    }

    public static CreationMethod FromStaticMethodInfo(MethodInfo m)
    {
      return new CreationMethod(m, args => m.Invoke(null, args), 
        m.ReturnType, 
        new CreationMethodParameters(m.GetParameters()));
    }

    private readonly MethodBase _constructor;
    private readonly Type _returnType;
    private readonly Func<object[], object> _invocation;
    private readonly CreationMethodParameters _creationMethodParameters;

    private CreationMethod(
      MethodBase constructor, 
      Func<object[], object> invocation, 
      Type returnType, CreationMethodParameters creationMethodParameters)
    {
      _constructor = constructor;
      _creationMethodParameters = creationMethodParameters;
      _returnType = returnType;
      _invocation = invocation;
    }

    public bool HasNonPointerArgumentsOnly()
    {
      if(!_creationMethodParameters.ContainAnyPointer())
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public bool HasLessParametersThan(int numberOfParams)
    {
      if (_creationMethodParameters.AreLessThan(numberOfParams))
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public int GetParametersCount()
    {
      return _creationMethodParameters.Count();
    }

    public List<object> GenerateAnyParameterValues(Func<Type, object> instanceGenerator)
    {
      var constructorValues = new List<object>();
      _creationMethodParameters.FillWithGeneratedValues(instanceGenerator, constructorValues);
      return constructorValues;
    }

    public string GetDescriptionForParameter(int i)
    {
      return _creationMethodParameters.GetDescriptionFor(i);
    }

    public object InvokeWith(IEnumerable<object> constructorParameters)
    {
      return _invocation(constructorParameters.ToArray());
    }

    public override string ToString()
    {
      var description = _constructor.DeclaringType.Name + "(";
      description += _creationMethodParameters + ")";
      return description;
    }

    public static bool IsInternal(MethodBase c)
    {
      return c.IsAssembly && !c.IsPublic && !c.IsStatic;
    }

    public bool IsFactoryMethod()
    {
      return _constructor.DeclaringType == _returnType;
    }

    public bool IsNotRecursive()
    {
      return !IsRecursive();
    }

    public bool IsRecursive()
    {
      return _creationMethodParameters.IsAnyOfType(_returnType);
    }

  }
}