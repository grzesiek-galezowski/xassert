using System.Collections.Generic;
using CommonTypes;
using TypeReflection.Interfaces;

namespace ConstructorRetrieval
{
  public interface IConstructorQueries
  {
    Maybe<ICreateObjects> GetNonPublicParameterlessConstructorInfo();
    Maybe<ICreateObjects> GetPublicParameterlessConstructor();
    List<ICreateObjects> GetInternalConstructorsWithoutRecursiveParameters();
    IEnumerable<ICreateObjects> TryToObtainPublicConstructorsWithoutRecursiveArguments();
    IEnumerable<ICreateObjects> TryToObtainPublicConstructorsWithRecursiveArguments();
    IEnumerable<ICreateObjects> TryToObtainInternalConstructorsWithRecursiveArguments();
    IEnumerable<ICreateObjects> TryToObtainPrimitiveTypeConstructor();
    IEnumerable<ICreateObjects> TryToObtainPublicStaticFactoryMethodWithoutRecursion();
  }
}