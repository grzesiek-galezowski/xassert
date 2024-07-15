using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class PublicNonRecursiveConstructorRetrieval(IConstructorRetrieval next) : IConstructorRetrieval
{
  public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
  {
    var publicConstructors = constructors.TryToObtainPublicConstructorsWithoutRecursiveArguments();

    if (publicConstructors.Any())
    {
      return publicConstructors;
    }
    else
    {
      return next.RetrieveFrom(constructors);
    }
  }
}