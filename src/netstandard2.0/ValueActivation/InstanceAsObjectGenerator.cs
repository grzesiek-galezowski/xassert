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

    public object GenerateInstance(InstanceGenerator instanceGenerator, GenerationRequest request)
    {
      return instanceGenerator.Instance(_type, request);
    }
  }
}