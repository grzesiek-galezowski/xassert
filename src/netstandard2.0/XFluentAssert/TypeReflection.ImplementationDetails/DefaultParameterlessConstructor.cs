using System;
using System.Collections.Generic;
using System.Reflection;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails;

internal class DefaultParameterlessConstructor(Func<object> creation) : ICreateObjects
{
  private readonly Func<object> _creation = creation;

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

  public static ICreateObjects ForOrdinaryType(ConstructorInfo constructorInfo)
  {
    return new DefaultParameterlessConstructor(() => constructorInfo.Invoke(new object[] { }));
  }

  public static IEnumerable<ICreateObjects> ForValue(Type type)
  {
    return new[] { new DefaultParameterlessConstructor(() => Activator.CreateInstance(type)) };
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