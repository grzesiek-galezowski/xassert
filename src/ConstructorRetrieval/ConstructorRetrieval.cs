using System.Collections.Generic;
using TypeReflection.Interfaces;

namespace TypeReflection.ImplementationDetails.ConstructorRetrievals
{
  public interface ConstructorRetrieval
  {
    IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors);
  }
}