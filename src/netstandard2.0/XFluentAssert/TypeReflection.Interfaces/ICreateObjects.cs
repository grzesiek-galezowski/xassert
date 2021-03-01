using System;
using System.Collections.Generic;

namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  public interface ICreateObjects
  {
    bool HasNonPointerArgumentsOnly();
    bool HasLessParametersThan(int numberOfParams);
    int GetParametersCount();
    List<object> GenerateAnyParameterValues(Func<Type, object> instanceGenerator);
    string GetDescriptionForParameter(int i);
    object InvokeWith(IEnumerable<object> constructorParameters);
    bool IsNotRecursive();
    bool IsRecursive();
  }
}
