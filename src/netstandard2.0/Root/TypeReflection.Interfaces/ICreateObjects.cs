namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  using System;
  using System.Collections.Generic;

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
