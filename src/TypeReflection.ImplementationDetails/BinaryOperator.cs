using System.Reflection;
using CommonTypes;
using TypeReflection.Interfaces;
using TypeReflection.Interfaces.Exceptions;

namespace TypeReflection.ImplementationDetails
{

  public class BinaryOperator<T, TResult> : IAmBinaryOperator<T,TResult>
  {
    private readonly IAmBinaryOperator _method;

    private BinaryOperator(IAmBinaryOperator binaryOperator)
    {
      _method = binaryOperator;
    }

    public TResult Evaluate(T instance1, T instance2)
    {
      return (TResult)_method.Evaluate(instance1, instance2);
    }

    public static IAmBinaryOperator<T, bool> Wrap(IAmBinaryOperator binaryOperator)
    {
      return new BinaryOperator<T, bool>(binaryOperator);
    }
  }

  public class BinaryOperator : IAmBinaryOperator
  {
    private readonly MethodInfo _method;

    public BinaryOperator(MethodInfo method)
    {
      _method = method;
    }

    public object Evaluate(object instance1, object instance2)
    {
      return _method.Invoke(null, new[] { instance1, instance2 });
    }

    public static IAmBinaryOperator Wrap(
      Maybe<MethodInfo> maybeOperator, 
      Maybe<MethodInfo> maybeFallbackOperator, 
      string op)
    {
      if (maybeOperator.Otherwise(maybeFallbackOperator).HasValue)
      {
        return new BinaryOperator(maybeOperator.Otherwise(maybeFallbackOperator).Value);
      }
      else
      {
        throw new NoSuchOperatorInTypeException("No method " + op);
      }
    }
  }
}