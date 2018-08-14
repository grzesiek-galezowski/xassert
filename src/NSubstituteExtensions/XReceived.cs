namespace TddXt.XAssert.NSubstituteExtensions
{
  using System;

  using NSubstitute.Core;

  using TddXt.XAssert.NSubstituteExtensions.ImplementationDetails;

  public class XReceived
  {
    public static void Only(Action action)
    {
      new SequenceExclusiveAssertion().Assert(SubstitutionContext.Current.RunQuery(action));
    }
  }
}
