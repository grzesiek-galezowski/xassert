namespace TddXt.XAssert.ConstructorRetrieval
{
  using System.Collections.Generic;
  using System.Linq;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public class InternalConstructorWithoutRecursionRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public InternalConstructorWithoutRecursionRetrieval(ConstructorRetrieval next)
    {
      this._next = next;
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
        return this._next.RetrieveFrom(constructors);
      }
    }
  }
}