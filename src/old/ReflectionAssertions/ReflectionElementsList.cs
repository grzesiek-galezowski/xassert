namespace TddXt.XAssert.ReflectionAssertions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public class ReflectionElementsList
  {
    public static string Format(IEnumerable<IAmField> staticFields)
    {
      var result = new HashSet<string>(staticFields.Select(f => f.GenerateExistenceMessage()));
      return String.Join(Environment.NewLine, result);
    }

    public static string Format(IEnumerable<IAmEvent> nonPublicEvents)
    {
      var result = new HashSet<string>(nonPublicEvents.Select(eventWrapper => eventWrapper.GenerateNonPublicExistenceMessage()));
      return String.Join(Environment.NewLine, result);
    }

    public static string Format(IEnumerable<Tuple<Type, int>> constructorLimitsExceeded)
    {
      var limits = new HashSet<Tuple<Type, int>>(constructorLimitsExceeded);
      var result = limits.Select(l => l.Item1 + " contains " + l.Item2 + " constructors");
      return String.Join(Environment.NewLine, result);
    }
  }
}