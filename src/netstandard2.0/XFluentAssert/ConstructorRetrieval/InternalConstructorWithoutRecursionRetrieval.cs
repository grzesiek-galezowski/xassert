using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class InternalConstructorWithoutRecursionRetrieval : IConstructorRetrieval
{
  private readonly IConstructorRetrieval _next;

  public InternalConstructorWithoutRecursionRetrieval(IConstructorRetrieval next)
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