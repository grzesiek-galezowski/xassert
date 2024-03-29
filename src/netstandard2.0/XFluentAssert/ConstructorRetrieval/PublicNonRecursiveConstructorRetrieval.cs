using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class PublicNonRecursiveConstructorRetrieval : IConstructorRetrieval
{
  private readonly IConstructorRetrieval _next;

  public PublicNonRecursiveConstructorRetrieval(IConstructorRetrieval next)
  {
    _next = next;
  }

  public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
  {
    var publicConstructors = constructors.TryToObtainPublicConstructorsWithoutRecursiveArguments();

    if (publicConstructors.Any())
    {
      return publicConstructors;
    }
    else
    {
      return _next.RetrieveFrom(constructors);
    }
  }
}