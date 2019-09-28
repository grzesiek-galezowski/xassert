namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails
{
  using System.Reflection;

  using CommonTypes;
  using Interfaces;
  using Interfaces.Exceptions;

  public class BinaryOperator<T, TResult> : IAmBinaryOperator<T,TResult>
  {
    private readonly IAmBinaryOperator _method;

    private BinaryOperator(IAmBinaryOperator binaryOperator)
    {
      this._method = binaryOperator;
    }

    public TResult Evaluate(T instance1, T instance2)
    {
      return (TResult)this._method.Evaluate(instance1, instance2);
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
      this._method = method;
    }

    public object Evaluate(object instance1, object instance2)
    {
      return this._method.Invoke(null, new[] { instance1, instance2 });
    }

    public static IAmBinaryOperator Wrap(
      Maybe<MethodInfo> maybeOperator, 
      Maybe<MethodInfo> maybeFallbackOperator, 
      string op)
    {
      if (maybeOperator.Otherwise(maybeFallbackOperator).HasValue)
      {
        return new BinaryOperator(maybeOperator.Otherwise(maybeFallbackOperator).Value());
      }
      else
      {
        throw new NoSuchOperatorInTypeException("No method " + op);
      }
    }
  }
}