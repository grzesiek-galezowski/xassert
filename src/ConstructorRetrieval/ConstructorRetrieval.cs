using System.Collections.Generic;

namespace ConstructorRetrieval
{
  using TddXt.XAssert.TypeReflection.Interfaces;

  public interface ConstructorRetrieval
  {
    IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors);
  }
}