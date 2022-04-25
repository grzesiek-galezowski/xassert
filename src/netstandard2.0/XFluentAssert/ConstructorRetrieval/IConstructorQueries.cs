using System.Collections.Generic;
using Core.Maybe;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal interface IConstructorQueries
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