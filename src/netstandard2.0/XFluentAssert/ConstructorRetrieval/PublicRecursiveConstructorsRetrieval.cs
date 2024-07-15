using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class PublicRecursiveConstructorsRetrieval(IConstructorRetrieval next) : IConstructorRetrieval
{
  public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
  {
    var constructorList = constructors.TryToObtainPublicConstructorsWithRecursiveArguments();
    if (constructorList.Any())
    {
      return constructorList.ToList();
    }
    else
    {
      return next.RetrieveFrom(constructors);
    }
  }
}