using System.Collections.Generic;
using System.Linq;

namespace ConstructorRetrieval
{
  using TddXt.XAssert.TypeReflection.Interfaces;

  public class PublicNonRecursiveConstructorRetrieval : ConstructorRetrieval
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