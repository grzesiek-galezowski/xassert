namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails
{
  using System;

  public class DuplicateConstantException : Exception
  {
    public DuplicateConstantException(string message) : base(message)
    {

    }
  }
}