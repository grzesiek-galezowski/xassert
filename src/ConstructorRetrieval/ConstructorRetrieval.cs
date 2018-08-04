using System.Collections.Generic;
using TypeReflection.Interfaces;

namespace ConstructorRetrieval
{
  public interface ConstructorRetrieval
  {
    IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors);
  }
}