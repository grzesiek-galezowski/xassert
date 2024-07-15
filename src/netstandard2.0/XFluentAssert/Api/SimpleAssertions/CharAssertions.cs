using FluentAssertions.Numeric;

namespace TddXt.XFluentAssert.Api.SimpleAssertions;

public class CharAssertions(char c) : ComparableTypeAssertions<char>(c)
{
  public char CharSubject { get; } = c;
}