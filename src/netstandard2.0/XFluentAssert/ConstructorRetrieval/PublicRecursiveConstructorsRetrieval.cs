using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  internal class PublicRecursiveConstructorsRetrieval : IConstructorRetrieval
  {
    private readonly IConstructorRetrieval _next;

    public PublicRecursiveConstructorsRetrieval(IConstructorRetrieval next)
    {
      _next = next;
    }

    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      var constructorList = constructors.TryToObtainPublicConstructorsWithRecursiveArguments();
      if (constructorList.Any())
      {
        return constructorList.ToList();
      }
      else
      {
        return _next.RetrieveFrom(constructors);
      }
    }
  }
}