using System.Collections.Generic;
using TddEbook.TddToolkit.CommonTypes;

namespace TypeReflection.Interfaces
{
  public interface IConstructorQueries
  {
    Maybe<IConstructorWrapper2> GetNonPublicParameterlessConstructorInfo();
    Maybe<IConstructorWrapper2> GetPublicParameterlessConstructor();
    List<IConstructorWrapper2> TryToObtainInternalConstructorsWithoutRecursiveArguments();
    IEnumerable<IConstructorWrapper2> TryToObtainPublicConstructorsWithoutRecursiveArguments();
    IEnumerable<IConstructorWrapper2> TryToObtainPublicConstructorsWithRecursiveArguments();
    IEnumerable<IConstructorWrapper2> TryToObtainInternalConstructorsWithRecursiveArguments();
    IEnumerable<IConstructorWrapper2> TryToObtainPrimitiveTypeConstructor();
    IEnumerable<IConstructorWrapper2> TryToObtainPublicStaticFactoryMethodWithoutRecursion();
  }
}