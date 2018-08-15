namespace TddXt.XAssert.TddEbook.TddToolkit.SimpleAssertions
{
  using FluentAssertions.Numeric;

  public class CharAssertions : ComparableTypeAssertions<char>
  {
    public CharAssertions(char c) : base(c)
    {
      this.CharSubject = c;
    }

    public char CharSubject { get; }
  }
}