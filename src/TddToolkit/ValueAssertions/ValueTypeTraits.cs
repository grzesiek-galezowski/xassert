using System.Collections.Generic;

namespace TddEbook.TddToolkit
{
  public class ValueTypeTraits
  {
    public static ValueTypeTraits Full()
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

    public static ValueTypeTraits Custom => new ValueTypeTraits();

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

    public static ValueTypeTraits Default()
    {
      return Full();
    }

    public ValueTypeTraits SkipConstructorArgument(int constructorArgumentIndex)
    {
      this.IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify.Add(constructorArgumentIndex);
      return this;
    }

  }
}
