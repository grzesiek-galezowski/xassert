using System.Collections.Generic;
using Functional.Maybe;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval
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