using System;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions2
{
  public static class Are
  {
    public static bool EqualInTermsOfEqualityOperator(Type type, object? instance1, object? instance2)
    {
      return (bool)SmartType.For(type).EqualityOperator().Evaluate(instance1, instance2);
    }

    public static bool NotEqualInTermsOfInEqualityOperator(Type type, object? instance1, object? instance2)
    {
      return (bool)SmartType.For(type).InequalityOperator().Evaluate(instance1, instance2);
    }
  }
}
