using System.Collections.Generic;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  internal class NonPublicParameterlessConstructorRetrieval : IConstructorRetrieval
  {
    private readonly IConstructorRetrieval _next;

    public NonPublicParameterlessConstructorRetrieval(IConstructorRetrieval next)
    {
      _next = next;
    }

    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      var constructor = constructors.GetNonPublicParameterlessConstructorInfo();
      if (constructor.HasValue)
      {
        return new List<ICreateObjects> { constructor.Value() };
      }
      else
      {
        return _next.RetrieveFrom(constructors);
      }
    }
  }
}