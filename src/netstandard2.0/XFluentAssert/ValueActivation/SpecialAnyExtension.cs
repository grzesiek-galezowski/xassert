using System;

using TddXt.AnyExtensibility;

namespace TddXt.XFluentAssert.ValueActivation
{
  public static class SpecialAnyExtension
  {
    public static object InstanceAsObject(this BasicGenerator gen, Type type)
    {
      return gen.InstanceOf(new InstanceAsObjectGenerator(type));
    }
  }
}