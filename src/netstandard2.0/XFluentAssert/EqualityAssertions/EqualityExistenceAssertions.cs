using System;
using FluentAssertions;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions
{
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