using FluentAssertions.Numeric;

namespace TddXt.XFluentAssertRoot.SimpleAssertions
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