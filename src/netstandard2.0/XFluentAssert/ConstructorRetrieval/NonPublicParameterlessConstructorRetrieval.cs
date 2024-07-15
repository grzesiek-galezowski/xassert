using System.Collections.Generic;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class NonPublicParameterlessConstructorRetrieval(IConstructorRetrieval next) : IConstructorRetrieval
{
  public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
  {
    var constructor = constructors.GetNonPublicParameterlessConstructorInfo();
    if (constructor.HasValue)
    {
      return new List<ICreateObjects> { constructor.Value() };
    }
    else
    {
      return next.RetrieveFrom(constructors);
    }
  }
}