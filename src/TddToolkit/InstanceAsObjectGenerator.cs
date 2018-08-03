using System;
using TddXt.AnyExtensibility;

namespace TddEbook.TddToolkit
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