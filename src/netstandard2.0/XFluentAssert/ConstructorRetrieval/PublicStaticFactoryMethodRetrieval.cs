using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class PublicStaticFactoryMethodRetrieval : IConstructorRetrieval
{
  private readonly IConstructorRetrieval _next;

  public PublicStaticFactoryMethodRetrieval(IConstructorRetrieval next)
  {
    _next = next;
  }

  public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
  {
    var methods = constructors.TryToObtainPublicStaticFactoryMethodWithoutRecursion();
    if (!methods.Any())
    {
      return _next.RetrieveFrom(constructors);
    }
    return methods;
  }
}