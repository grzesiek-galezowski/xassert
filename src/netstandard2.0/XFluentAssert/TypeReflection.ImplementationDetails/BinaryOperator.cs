﻿using System.Reflection;
using Core.Maybe;
using TddXt.XFluentAssert.TypeReflection.Interfaces;
using TddXt.XFluentAssert.TypeReflection.Interfaces.Exceptions;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails;

internal class BinaryOperator<T, TResult> : IAmBinaryOperator<T, TResult>
{
  private readonly IAmBinaryOperator _method;

  private BinaryOperator(IAmBinaryOperator binaryOperator)
  {
    _method = binaryOperator;
  }

  public TResult Evaluate(T? instance1, T? instance2)
  {
    return (TResult)_method.Evaluate(instance1, instance2);
  }

  public static IAmBinaryOperator<T, bool> Wrap(IAmBinaryOperator binaryOperator)
  {
    return new BinaryOperator<T, bool>(binaryOperator);
  }
}

internal class BinaryOperator(MethodInfo method) : IAmBinaryOperator
{
  public object Evaluate(object? instance1, object? instance2)
  {
    return method.Invoke(null, new[] { instance1, instance2 });
  }

  public static IAmBinaryOperator Wrap(
    Maybe<MethodInfo> maybeOperator,
    Maybe<MethodInfo> maybeFallbackOperator,
    string op)
  {
    if (maybeOperator.Or(() => maybeFallbackOperator).HasValue)
    {
      return new BinaryOperator(maybeOperator.Or(maybeFallbackOperator).Value());
    }
    else
    {
      throw new NoSuchOperatorInTypeException("No method " + op);
    }
  }
}