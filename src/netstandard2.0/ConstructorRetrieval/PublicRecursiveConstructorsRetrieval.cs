namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  using System.Collections.Generic;
  using System.Linq;

  using TypeReflection.Interfaces;

  public class PublicRecursiveConstructorsRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public PublicRecursiveConstructorsRetrieval(ConstructorRetrieval next)
    {
      this._next = next;
    }

    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      var constructorList = constructors.TryToObtainPublicConstructorsWithRecursiveArguments();
      if (constructorList.Any())
      {
        return constructorList.ToList();
      }
      else
      {
        return this._next.RetrieveFrom(constructors);
      }
    }
  }
}