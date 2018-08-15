namespace TddXt.XAssert.TypeReflection.Interfaces
{
  using System;
  using System.Collections.Generic;

  public interface ICreateObjects
  {
    bool HasNonPointerArgumentsOnly();
    bool HasLessParametersThan(int numberOfParams);
    int GetParametersCount();
    bool HasAbstractOrInterfaceArguments();
    List<object> GenerateAnyParameterValues(Func<Type, object> instanceGenerator);
    string GetDescriptionForParameter(int i);
    object InvokeWithParametersCreatedBy(Func<Type, object> instanceGenerator);
    object InvokeWith(IEnumerable<object> constructorParameters);
    bool IsInternal();
    bool IsNotRecursive();
    bool IsRecursive();
  }
}
