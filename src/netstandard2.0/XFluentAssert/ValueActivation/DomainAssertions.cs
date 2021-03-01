using System;

namespace TddXt.XFluentAssert.ValueActivation
{
  static internal class DomainAssertions
  {
    public static void AssertDoesNotThrow(Action action, string message)
    {
      try
      {
        new Action(action).Invoke();
      }
      catch (Exception e)
      {
        throw new InvalidOperationException(message, e);
      }
    }

    public static void AssertAreNotEqual(Type instanceType, Type targetType)
    {
      if (instanceType != targetType)
      {
        throw new InvalidOperationException($"Expected {instanceType} != {targetType}");
      }
    }
  }
}