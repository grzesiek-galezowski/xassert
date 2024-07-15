using System;
using System.Collections.Generic;
using System.Linq;

namespace TddXt.XFluentAssert.EndToEndSpecification;

public class SequenceEqualsMatcher(List<int> expected)
{
  private readonly List<string> _failures = new List<string>();

  public bool Matches(IEnumerable<int> actual)
  {
    var sequenceEqual = actual.SequenceEqual(expected);
    if (!sequenceEqual)
    {
      _failures.Add(EnumerableToString(actual) + " does not match " + EnumerableToString(expected));
    }
    return sequenceEqual;
  }

  private string EnumerableToString(IEnumerable<int> enumerable)
  {
    if (!enumerable.Any())
    {
      return "EMPTY";
    }
    return "{" + string.Join(", ", enumerable) + "}";
  }

  public override string ToString()
  {
    return string.Join(Environment.NewLine, _failures);
  }
}