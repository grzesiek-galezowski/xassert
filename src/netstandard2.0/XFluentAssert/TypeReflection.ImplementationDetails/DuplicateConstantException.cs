using System;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails
{
  public class DuplicateConstantException : Exception
  {
    public DuplicateConstantException(string message) : base(message)
    {

    }
  }
}