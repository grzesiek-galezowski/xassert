namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  using System.Collections.Generic;
  using System.Linq;

  using TypeReflection.Interfaces;

  public class PublicStaticFactoryMethodRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public PublicStaticFactoryMethodRetrieval(ConstructorRetrieval next)
    {
      _next = next;
    }

    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      var methods = constructors.TryToObtainPublicStaticFactoryMethodWithoutRecursion();
      if (!methods.Any())
      {
        return _next.RetrieveFrom(constructors);
      }
      return methods;
    }
  }
}