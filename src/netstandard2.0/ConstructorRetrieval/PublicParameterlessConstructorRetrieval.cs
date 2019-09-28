namespace TddXt.XFluentAssert.ConstructorRetrieval
{
  using System.Collections.Generic;

  using TypeReflection.Interfaces;

  public class PublicParameterlessConstructorRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public PublicParameterlessConstructorRetrieval(ConstructorRetrieval next)
    {
      _next = next;
    }

    public IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors)
    {
      var constructor = constructors.GetPublicParameterlessConstructor();
      if (constructor.HasValue)
      {
        return new List<ICreateObjects> {constructor.Value()};
      }
      else
      {
        return _next.RetrieveFrom(constructors);
      }
    }
  }
}