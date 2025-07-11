using AwesomeAssertions.Execution;
using AwesomeAssertions.Numeric;

namespace TddXt.XFluentAssert.Api.SimpleAssertions;

public class CharAssertions(char c, AssertionChain assertionChain) : ComparableTypeAssertions<char>(c, assertionChain)
{
  public char CharSubject { get; } = c;
}