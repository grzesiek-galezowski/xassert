using System;

namespace TypeReflection.ImplementationDetails
{
  public class DuplicateConstantException : Exception
  {
    public DuplicateConstantException(string message) : base(message)
    {
      
    }
  }
}