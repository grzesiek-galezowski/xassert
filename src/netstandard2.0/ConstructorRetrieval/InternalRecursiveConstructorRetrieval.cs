namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  using System.Collections.Generic;
  using System.Linq;

  using TypeReflection.Interfaces;

  public class InternalRecursiveConstructorRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public InternalRecursiveConstructorRetrieval(ConstructorRetrieval next)
    {
      this._next = next;
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
        return this._next.RetrieveFrom(constructors);
      }
    }
  }
}