using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class InternalConstructorWithoutRecursionRetrieval(IConstructorRetrieval next) : IConstructorRetrieval
{
  public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
  {
    var internalConstructors = constructors.GetInternalConstructorsWithoutRecursiveParameters();

    if (internalConstructors.Any())
    {
      return internalConstructors;
    }
    else
    {
      return next.RetrieveFrom(constructors);
    }
  }
}