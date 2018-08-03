using System;

namespace TypeReflection
{
  public static class DefaultValue
  {
    public static object Of(Type t)
    {
      if (t.IsValueType)
      {
        return Activator.CreateInstance(t);
      }

      return null;
    }
  }
}