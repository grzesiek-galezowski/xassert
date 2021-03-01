using System.Collections.Generic;
using TddXt.XFluentAssert.ValueObjectConstraints;

namespace TddXt.XFluentAssert.Api.ValueAssertions
{
  public class ValueTypeTraits : IKnowWhatValueTraitsToCheck
  {
    public static IKnowWhatValueTraitsToCheck Full()
    {
      var result = new ValueTypeTraits
      {
        RequireEqualityAndInequalityOperatorImplementation = true
      };
      return result;
    }

    private ValueTypeTraits()
    {
      IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify = new List<int>();
      RequireAllFieldsReadOnly = true;
      RequireSafeInequalityToNull = true;
      RequireEqualityAndInequalityOperatorImplementation = true;
    }

    public static IKnowWhatValueTraitsToCheck Custom => new ValueTypeTraits();

    public List<int> IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify
    {
      get;
      // ReSharper disable once MemberCanBePrivate.Global
      // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
      set;
    }

    public bool RequireEqualityAndInequalityOperatorImplementation
    {
      get;
      // ReSharper disable once MemberCanBePrivate.Global
      set;
    }

    public bool RequireSafeInequalityToNull
    {
      get;
      // ReSharper disable once MemberCanBePrivate.Global
      // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
      set;
    }

    public bool RequireAllFieldsReadOnly
    {
      // ReSharper disable once MemberCanBePrivate.Global
      // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
      get; set;
    }

    public static IKnowWhatValueTraitsToCheck Default()
    {
      return Full();
    }

    public IKnowWhatValueTraitsToCheck SkipConstructorArgument(int constructorArgumentIndex)
    {
      IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify.Add(constructorArgumentIndex);
      return this;
    }

  }
}
