using System;
using System.Linq;

namespace TddXt.XFluentAssert.ValueObjectConstraints;

internal static class ValueObjectWhiteList
{
  private static readonly Type[] WellKnownValueTypesList =
  {
    typeof(object),
    typeof(string),
    typeof(Guid)
  };

  public static bool Contains(Type type)
  {
    if (WellKnownValueTypesList.Contains(type))
    {
      return true;
    }

    if (type.IsEnum)
    {
      return true;
    }

    if (type.IsPrimitive)
    {
      return true;
    }

    return false;
  }
}