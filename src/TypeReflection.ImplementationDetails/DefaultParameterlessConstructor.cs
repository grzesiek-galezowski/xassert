namespace TddXt.XAssert.TypeReflection.ImplementationDetails
{
  using System;
  using System.Collections.Generic;
  using System.Reflection;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public class DefaultParameterlessConstructor : ICreateObjects
  {
    private readonly Func<object> _creation;

    public DefaultParameterlessConstructor(Func<object> creation)
    {
      this._creation = creation;
    }

    public bool HasNonPointerArgumentsOnly()
    {
      return true;
    }

    public bool HasLessParametersThan(int numberOfParams)
    {
      return true;
    }

    public int GetParametersCount()
    {
      return 0;
    }

    public bool HasAbstractOrInterfaceArguments()
    {
      return false;
    }

    public List<object> GenerateAnyParameterValues(Func<Type, object> instanceGenerator)
    {
      return new List<object>();
    }

    public string GetDescriptionForParameter(int i)
    {
      return string.Empty;
    }

    public object InvokeWithParametersCreatedBy(Func<Type, object> instanceGenerator)
    {
      return this._creation.Invoke();
    }

    public object InvokeWith(IEnumerable<object> constructorParameters)
    {
      return this._creation.Invoke();
    }

    public bool IsInternal()
    {
      return false; //?? actually, this is not right...
    }

    public static ICreateObjects ForOrdinaryType(ConstructorInfo constructorInfo)
    {
      return new DefaultParameterlessConstructor(() => constructorInfo.Invoke(new object[]{}));
    }

    public static IEnumerable<ICreateObjects> ForValue(Type type)
    {
      return new [] { new DefaultParameterlessConstructor(() => Activator.CreateInstance(type))};
    }

    public bool IsNotRecursive()
    {
      return true;
    }

    public bool IsRecursive()
    {
      return false;
    }
  }
}