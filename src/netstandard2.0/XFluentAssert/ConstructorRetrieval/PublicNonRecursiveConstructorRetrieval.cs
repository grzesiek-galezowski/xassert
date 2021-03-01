using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  internal class PublicNonRecursiveConstructorRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public PublicNonRecursiveConstructorRetrieval(ConstructorRetrieval next)
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
}