namespace TddXt.XFluentAssert.Root.ValueAssertions
{
  using System.Collections.Generic;

  public interface IKnowWhatValueTraitsToCheck
  {
    List<int> IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify
    {
      get;
      // ReSharper disable once MemberCanBePrivate.Global
      // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
      set;
    }

    bool RequireEqualityAndInequalityOperatorImplementation
    {
      get;
      // ReSharper disable once MemberCanBePrivate.Global
      set;
    }

    bool RequireSafeInequalityToNull
    {
      get;
      // ReSharper disable once MemberCanBePrivate.Global
      // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
      set;
    }

    bool RequireAllFieldsReadOnly
    {
      // ReSharper disable once MemberCanBePrivate.Global
      // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
      get;
      set;
    }

    IKnowWhatValueTraitsToCheck SkipConstructorArgument(int constructorArgumentIndex);
  }
}