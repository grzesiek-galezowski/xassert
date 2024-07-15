using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class PublicStaticFactoryMethodRetrieval(IConstructorRetrieval next) : IConstructorRetrieval
{
  public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
  {
    var methods = constructors.TryToObtainPublicStaticFactoryMethodWithoutRecursion();
    if (!methods.Any())
    {
      return next.RetrieveFrom(constructors);
    }
    return methods;
  }
}