﻿namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails
{
  using System.Reflection;

  using Interfaces;

  public class Property : IAmProperty //bug there's another Property class in this sln
  {
    private readonly PropertyInfo _propertyInfo;

    public Property(PropertyInfo propertyInfo)
    {
      _propertyInfo = propertyInfo;
    }

    public bool HasPublicSetter()
    {
      return _propertyInfo.GetSetMethod() != null && _propertyInfo.GetSetMethod().IsPublic;
    }

    public string ShouldNotBeMutableButIs()
    {
      return "Value objects are immutable by design, but Property "
             + _propertyInfo.Name
             + " is mutable. Declare property setter as private to pass this check";
    }
  }
}