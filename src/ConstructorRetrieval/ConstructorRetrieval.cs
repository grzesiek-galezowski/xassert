namespace TddXt.XAssert.ConstructorRetrieval
{
  using System.Collections.Generic;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public interface ConstructorRetrieval
  {
    IEnumerable<ICreateObjects> RetrieveFrom(IConstructorQueries constructors);
  }
}