using System;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails;

internal class DuplicateConstantException : Exception
{
  public DuplicateConstantException(string message) : base(message)
  {

  }
}