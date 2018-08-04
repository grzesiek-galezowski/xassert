using System;
using NSubstitute.Core;
using NSubstituteExtensions.ImplementationDetails;

namespace NSubstituteExtensions
{
  public class XReceived
  {
    public static void Only(Action action)
    {
      new SequenceExclusiveAssertion().Assert(SubstitutionContext.Current.RunQuery(action));
    }
  }
}
