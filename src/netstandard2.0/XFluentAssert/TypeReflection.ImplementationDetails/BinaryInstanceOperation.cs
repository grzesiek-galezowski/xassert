using System.Reflection;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails;

internal class BinaryInstanceOperation(MethodInfo equalsMethod) : IAmBinaryOperator
{
  public object Evaluate(object? instance1, object? instance2)
  {
    return equalsMethod.Invoke(instance1, new[] { instance2 });
  }
}