using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class InternalRecursiveConstructorRetrieval(IConstructorRetrieval next) : IConstructorRetrieval
{
  public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
  {
    var foundConstructors = constructors.TryToObtainInternalConstructorsWithRecursiveArguments();
    if (foundConstructors.Any())
    {
      return foundConstructors.ToArray();
    }
    else
    {
      return next.RetrieveFrom(constructors);
    }
  }
}