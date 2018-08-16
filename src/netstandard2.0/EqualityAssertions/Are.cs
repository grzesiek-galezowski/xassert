namespace TddXt.XFluentAssert.EqualityAssertions
{
  using System;

  using FluentAssertions;

  using TddXt.XFluentAssert.TypeReflection;

  public static class Are
  {
    public static bool EqualInTermsOfEqualityOperator<T>(T instance1, T instance2) where T : class
    {
      return TypeOf<T>.Equality().Evaluate(instance1, instance2);
    }

    public static bool NotEqualInTermsOfInEqualityOperator<T>(T instance1, T instance2) where T : class
    {
      return TypeOf<T>.Inequality().Evaluate(instance1, instance2);
    }

    public static bool EqualInTermsOfEqualityOperator(Type type, object instance1, object instance2)
    {
      return (bool)SmartType.For(type).Equality().Evaluate(instance1, instance2);
    }

    public static bool NotEqualInTermsOfInEqualityOperator(Type type, object instance1, object instance2)
    {
      return (bool)SmartType.For(type).Inequality().Evaluate(instance1, instance2);
    }
  }

  class EqualityExistenceAssertions
  {
    private static Action ExecutionOf(Action func)
    {
      return func;
    }

    public static void AssertEqualityOperatorIsDefinedFor(Type type)
    {
      ExecutionOf(() => SmartType.For(type).Equality()).Should().NotThrow<Exception>();
    }

    public static void AssertInequalityOperatorIsDefinedFor(Type type)
    {
      ExecutionOf(() => SmartType.For(type).Inequality()).Should().NotThrow<Exception>();
    }

  }
}
