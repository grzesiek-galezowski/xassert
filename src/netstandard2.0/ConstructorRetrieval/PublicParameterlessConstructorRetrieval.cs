namespace TddXt.XAssert.ConstructorRetrieval
{
  using System.Collections.Generic;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public class PublicParameterlessConstructorRetrieval : ConstructorRetrieval
  {
    private readonly ConstructorRetrieval _next;

    public PublicParameterlessConstructorRetrieval(ConstructorRetrieval next)
    {
      this._next = next;
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
        return this._next.RetrieveFrom(constructors);
      }
    }
  }
}