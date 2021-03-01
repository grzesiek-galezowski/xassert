using System.Collections.Generic;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  public interface IConstructorRetrieval
  {
    IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors);
  }
}