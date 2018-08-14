using System.Collections.Generic;
using CommonTypes;

namespace ConstructorRetrieval
{
  using TddXt.XAssert.TypeReflection.Interfaces;

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