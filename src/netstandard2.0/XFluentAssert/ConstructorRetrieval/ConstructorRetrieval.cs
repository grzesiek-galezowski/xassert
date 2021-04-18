using System.Collections.Generic;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  internal interface IConstructorRetrieval
  {
    IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors);
  }
}