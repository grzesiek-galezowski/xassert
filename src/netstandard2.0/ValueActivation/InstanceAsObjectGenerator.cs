namespace TddXt.XFluentAssert.ValueActivation
{
  using System;

  using AnyExtensibility;

  internal class InstanceAsObjectGenerator : InlineGenerator<object>
  {
    private readonly Type _type;

    public InstanceAsObjectGenerator(Type type)
    {
      _type = type;
    }

    public object GenerateInstance(InstanceGenerator instanceGenerator, GenerationTrace trace)
    {
      return instanceGenerator.Instance(_type, trace);
    }
  }
}