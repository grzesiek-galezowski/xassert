using System.Reflection;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails;

internal class BinaryInstanceOperation : IAmBinaryOperator
{
  private readonly MethodInfo _equalsMethod;

  public BinaryInstanceOperation(MethodInfo equalsMethod)
  {
    _equalsMethod = equalsMethod;
  }

  public object Evaluate(object? instance1, object? instance2)
  {
    return _equalsMethod.Invoke(instance1, new[] { instance2 });
  }
}