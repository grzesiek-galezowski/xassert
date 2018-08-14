namespace TddXt.XAssert.ValueActivation
{
  using System;

  using TddXt.AnyExtensibility;

  public static class SpecialAnyExtension
  {
    public static object InstanceAsObject(this BasicGenerator gen, Type type)
    {
      return gen.InstanceOf(new InstanceAsObjectGenerator(type));
    }
  }
}