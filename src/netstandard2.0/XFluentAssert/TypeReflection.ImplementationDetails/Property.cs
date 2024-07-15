using System.Reflection;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails;

internal class Property(PropertyInfo propertyInfo) : IAmProperty //bug there's another Property class in this sln
{
  public bool HasPublicSetter()
  {
    return propertyInfo.GetSetMethod() != null && propertyInfo.GetSetMethod().IsPublic;
  }

  public string ShouldNotBeMutableButIs()
  {
    return "Value objects are immutable by design, but Property "
           + propertyInfo.Name
           + " is mutable. Declare property setter as private to pass this check";
  }
}