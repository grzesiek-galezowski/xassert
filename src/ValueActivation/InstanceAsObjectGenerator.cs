using System;
using TddXt.AnyExtensibility;

namespace ValueActivation
{
  internal class InstanceAsObjectGenerator : InlineGenerator<object>
  {
    private readonly Type _type;

    public InstanceAsObjectGenerator(Type type)
    {
      _type = type;
    }

    public object GenerateInstance(InstanceGenerator instanceGenerator)
    {
      return instanceGenerator.Instance(_type);
    }
  }
}