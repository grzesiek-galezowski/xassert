namespace TddXt.XFluentAssert.Root.SimpleAssertions
{
  using FluentAssertions.Numeric;

  public class CharAssertions : ComparableTypeAssertions<char>
  {
    public CharAssertions(char c) : base(c)
    {
      CharSubject = c;
    }

    public char CharSubject { get; }
  }
}