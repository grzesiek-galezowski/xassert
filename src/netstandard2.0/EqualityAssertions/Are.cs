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
      return TypeOf<T>.InequalityOperator().Evaluate(instance1, instance2);
    }

    public static bool EqualInTermsOfEqualityOperator(Type type, object instance1, object instance2)
    {
      return (bool)SmartType.For(type).EqualityOperator().Evaluate(instance1, instance2);
    }

    public static bool NotEqualInTermsOfInEqualityOperator(Type type, object instance1, object instance2)
    {
      return (bool)SmartType.For(type).InequalityOperator().Evaluate(instance1, instance2);
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
      ExecutionOf(() => SmartType.For(type).EqualityOperator()).Should().NotThrow<Exception>();
    }

    public static void AssertInequalityOperatorIsDefinedFor(Type type)
    {
      ExecutionOf(() => SmartType.For(type).InequalityOperator()).Should().NotThrow<Exception>();
    }

  }
}
