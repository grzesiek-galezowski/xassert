namespace TddXt.XAssert.TypeReflection.Interfaces.Exceptions
{
  using System;

  public class NoSuchOperatorInTypeException : Exception
  {
    public NoSuchOperatorInTypeException(string s)
      : base(s)
    {
    }
  }
}