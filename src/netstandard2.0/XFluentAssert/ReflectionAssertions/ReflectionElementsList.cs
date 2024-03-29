﻿using System;
using System.Collections.Generic;
using System.Linq;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.ReflectionAssertions;

internal class ReflectionElementsList
{
  public static string Format(IEnumerable<IAmField> staticFields)
  {
    var result = new HashSet<string>(staticFields.Select(f => f.GenerateExistenceMessage()));
    return string.Join(Environment.NewLine, result);
  }

  public static string NonPublicEventsFoundMessage(IEnumerable<IAmEvent> nonPublicEvents)
  {
    var result = new HashSet<string>(nonPublicEvents.Select(eventWrapper => eventWrapper.GenerateNonPublicExistenceMessage()));
    return string.Join(Environment.NewLine, result);
  }

  public static string Format(IEnumerable<Tuple<Type, int>> constructorLimitsExceeded)
  {
    var limits = new HashSet<Tuple<Type, int>>(constructorLimitsExceeded);
    var result = limits.Select(l => l.Item1 + " contains " + l.Item2 + " constructors");
    return string.Join(Environment.NewLine, result);
  }
}