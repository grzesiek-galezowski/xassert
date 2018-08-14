using System.Collections.Generic;
using System.Linq;

namespace ConstructorRetrieval
{
  using TddXt.XAssert.TypeReflection.Interfaces;

  public class InternalConstructorWithoutRecursionRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public InternalConstructorWithoutRecursionRetrieval(ConstructorRetrieval next)
    {
      _next = next;
    }

    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      var internalConstructors = constructors.GetInternalConstructorsWithoutRecursiveParameters();

      if (internalConstructors.Any())
      {
        return internalConstructors;
      }
      else
      {
        return _next.RetrieveFrom(constructors);
      }
    }
  }
}