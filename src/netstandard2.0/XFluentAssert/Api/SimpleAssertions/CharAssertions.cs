using FluentAssertions.Numeric;

namespace TddXt.XFluentAssert.Api.SimpleAssertions
{
  public class CharAssertions : ComparableTypeAssertions<char>
  {
    public CharAssertions(char c) : base(c)
    {
      CharSubject = c;
    }

    public char CharSubject { get; }
  }
}