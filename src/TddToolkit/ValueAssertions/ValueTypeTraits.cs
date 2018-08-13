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

    protected ValueTypeTraits()
    {
      this.IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify = new List<int>();
      RequireAllFieldsReadOnly = true;
      this.RequireSafeInequalityToNull = true;
      this.RequireEqualityAndInequalityOperatorImplementation = true;
    }

    public static ValueTypeTraits Custom
    {
      get
      {
        return new ValueTypeTraits();
      }
    }

    public List<int> IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify
    {
      get;
      set;
    }

    public bool RequireEqualityAndInequalityOperatorImplementation
    {
      get; 
      set;
    }

    public bool RequireSafeInequalityToNull
    {
      get;
      set;
    }

    public bool RequireAllFieldsReadOnly
    {
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
