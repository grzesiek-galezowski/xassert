using System.Collections.Generic;
using System.Linq;

namespace ConstructorRetrieval
{
  using TddXt.XAssert.TypeReflection.Interfaces;

  public class InternalRecursiveConstructorRetrieval : ConstructorRetrieval
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