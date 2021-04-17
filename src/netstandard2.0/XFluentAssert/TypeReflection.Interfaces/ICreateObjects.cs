namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  public interface ICreateObjects
  {
    bool HasNonPointerArgumentsOnly();
    bool HasLessParametersThan(int numberOfParams);
    int GetParametersCount();
    bool IsNotRecursive();
    bool IsRecursive();
  }
}
