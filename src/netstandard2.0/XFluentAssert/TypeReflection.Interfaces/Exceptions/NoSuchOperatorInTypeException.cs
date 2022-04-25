using System;

namespace TddXt.XFluentAssert.TypeReflection.Interfaces.Exceptions;

internal class NoSuchOperatorInTypeException : Exception
{
  public NoSuchOperatorInTypeException(string s)
    : base(s)
  {
  }
}