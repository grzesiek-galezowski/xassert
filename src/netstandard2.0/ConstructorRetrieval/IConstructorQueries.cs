namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  using System.Collections.Generic;

  using TddXt.XFluentAssert.CommonTypes;
  using TddXt.XFluentAssert.TypeReflection.Interfaces;

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