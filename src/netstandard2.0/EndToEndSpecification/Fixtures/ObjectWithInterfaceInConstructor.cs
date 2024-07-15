using System;

namespace TddXt.XFluentAssert.EndToEndSpecification.Fixtures;

[Serializable]
public class ObjectWithInterfaceInConstructor(
  int a,
  ISimple constructorArgument,
  string b,
  ObjectWithInterfaceInConstructor constructorNestedArgument)
{
  private readonly int _a = a;
  public readonly ISimple ConstructorArgument = constructorArgument;
  private readonly string _b = b;
  public readonly ObjectWithInterfaceInConstructor ConstructorNestedArgument = constructorNestedArgument;
}