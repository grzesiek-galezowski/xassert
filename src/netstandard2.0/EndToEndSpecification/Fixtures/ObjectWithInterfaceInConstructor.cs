using System;

namespace TddXt.XFluentAssert.EndToEndSpecification.Fixtures
{
  [Serializable]
  public class ObjectWithInterfaceInConstructor
  {
    private readonly int _a;
    public readonly ISimple ConstructorArgument;
    private readonly string _b;
    public readonly ObjectWithInterfaceInConstructor ConstructorNestedArgument;

    public ObjectWithInterfaceInConstructor(
      int a,
      ISimple constructorArgument,
      string b,
      ObjectWithInterfaceInConstructor constructorNestedArgument)
    {
      _a = a;
      ConstructorArgument = constructorArgument;
      _b = b;
      ConstructorNestedArgument = constructorNestedArgument;
    }
  }
}