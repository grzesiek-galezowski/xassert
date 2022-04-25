using System;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.EqualityAssertions;

class EqualityExistenceAssertions
{
  public static void AssertEqualityOperatorIsDefinedFor(Type type)
  {
    try
    {
      SmartType.For(type).EqualityOperator();
    }
    catch (Exception e)
    {
      throw new InvalidOperationException("Expected no exception when retrieving equality operator, but got " + e);
    }
  }

  public static void AssertInequalityOperatorIsDefinedFor(Type type)
  {
    try
    {
      SmartType.For(type).InequalityOperator();
    }
    catch (Exception e)
    {
      throw new InvalidOperationException("Expected no exception when retrieving equality operator, but got " + e);
    }
  }
}