namespace TddXt.XAssert.ConstructorRetrieval
{
  using System.Collections.Generic;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public class PrimitiveConstructorRetrieval : ConstructorRetrieval
  {
    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      return constructors.TryToObtainPrimitiveTypeConstructor();
    }
  }
}