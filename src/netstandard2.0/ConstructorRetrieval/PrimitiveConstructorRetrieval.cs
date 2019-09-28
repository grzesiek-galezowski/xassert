namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  using System.Collections.Generic;

  using TypeReflection.Interfaces;

  public class PrimitiveConstructorRetrieval : ConstructorRetrieval
  {
    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      return constructors.TryToObtainPrimitiveTypeConstructor();
    }
  }
}