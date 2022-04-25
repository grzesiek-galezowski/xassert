namespace TddXt.XFluentAssert.TypeReflection.Interfaces;

internal interface ICreateObjects
{
  bool HasNonPointerArgumentsOnly();
  bool HasLessParametersThan(int numberOfParams);
  int GetParametersCount();
  bool IsNotRecursive();
  bool IsRecursive();
}