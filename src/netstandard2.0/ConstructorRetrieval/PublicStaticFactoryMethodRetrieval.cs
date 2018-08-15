namespace TddXt.XAssert.ConstructorRetrieval
{
  using System.Collections.Generic;
  using System.Linq;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public class PublicStaticFactoryMethodRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public PublicStaticFactoryMethodRetrieval(ConstructorRetrieval next)
    {
      this._next = next;
    }

    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      var methods = constructors.TryToObtainPublicStaticFactoryMethodWithoutRecursion();
      if (methods.Count() == 0)
      {
        return this._next.RetrieveFrom(constructors);
      }
      return methods;
    }
  }
}