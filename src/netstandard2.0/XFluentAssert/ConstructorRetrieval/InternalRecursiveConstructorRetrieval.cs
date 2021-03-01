using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  internal class InternalRecursiveConstructorRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public InternalRecursiveConstructorRetrieval(ConstructorRetrieval next)
    {
      _next = next;
    }

    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      var foundConstructors = constructors.TryToObtainInternalConstructorsWithRecursiveArguments();
      if (foundConstructors.Any())
      {
        return foundConstructors.ToArray();
      }
      else
      {
        return _next.RetrieveFrom(constructors);
      }
    }
  }
}