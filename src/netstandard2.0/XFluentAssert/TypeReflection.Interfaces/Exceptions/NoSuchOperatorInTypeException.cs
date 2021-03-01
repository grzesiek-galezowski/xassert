using System;

namespace TddXt.XFluentAssert.TypeReflection.Interfaces.Exceptions
{
  public class NoSuchOperatorInTypeException : Exception
  {
    public NoSuchOperatorInTypeException(string s)
      : base(s)
    {
    }
  }
}