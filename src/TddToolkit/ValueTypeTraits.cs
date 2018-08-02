﻿using System.Collections.Generic;

namespace TddEbook.TddToolkit
{
  public class ValueTypeTraits
  {
    protected ValueTypeTraits()
    {
      IndexesOfConstructorArgumentsIndexesThatDoNotContituteAValueIdentify = new List<int>();
      RequireAllFieldsReadOnly = true;
      RequireSafeUnequalityToNull = true;
      RequireEqualityAndUnequalityOperatorImplementation = true;
    }

    public static ValueTypeTraits Custom
    {
      get
      {
        return new ValueTypeTraits();
      }
    }

    public List<int> IndexesOfConstructorArgumentsIndexesThatDoNotContituteAValueIdentify
    {
      get;
      set;
    }

    public bool RequireEqualityAndUnequalityOperatorImplementation
    {
      get; 
      set;
    }

    public bool RequireSafeUnequalityToNull
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
      IndexesOfConstructorArgumentsIndexesThatDoNotContituteAValueIdentify.Add(constructorArgumentIndex);
      return this;
    }

    public static ValueTypeTraits Full()
    {
      var result = new ValueTypeTraits
      {
        RequireEqualityAndUnequalityOperatorImplementation = true
      };
      return result;
    }
  }
}
