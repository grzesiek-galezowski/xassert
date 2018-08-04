using System.Collections.Generic;
using TypeReflection.Interfaces;

namespace TypeReflection.ImplementationDetails.ConstructorRetrievals
{
  public class PrimitiveConstructorRetrieval : ConstructorRetrieval
  {
    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      return constructors.TryToObtainPrimitiveTypeConstructor();
    }
  }
}