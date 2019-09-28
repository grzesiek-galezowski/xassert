namespace TddXt.XFluentAssert.ValueActivation
{
  using System;

  using AnyExtensibility;

  public static class SpecialAnyExtension
  {
    public static object InstanceAsObject(this BasicGenerator gen, Type type)
    {
      return gen.InstanceOf(new InstanceAsObjectGenerator(type));
    }
  }
}