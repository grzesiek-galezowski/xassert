using FluentAssertions.Numeric;

namespace TddXt.XFluentAssertRoot.SimpleAssertions
{
  public class CharAssertions : ComparableTypeAssertions<char>
  {
    public CharAssertions(char c) : base(c)
    {
      this.CharSubject = c;
    }

    public char CharSubject { get; }
  }
}