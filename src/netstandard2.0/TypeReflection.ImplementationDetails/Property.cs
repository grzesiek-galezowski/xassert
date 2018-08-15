namespace TddXt.XAssert.TypeReflection.ImplementationDetails
{
  using System.Reflection;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public class Property : IAmProperty //bug there's another Property class in this sln
  {
    private readonly PropertyInfo _propertyInfo;

    public Property(PropertyInfo propertyInfo)
    {
      this._propertyInfo = propertyInfo;
    }

    public bool HasPublicSetter()
    {
      return this._propertyInfo.GetSetMethod() != null && this._propertyInfo.GetSetMethod().IsPublic;
    }

    public string ShouldNotBeMutableButIs()
    {
      return "Value objects are immutable by design, but Property "
             + this._propertyInfo.Name
             + " is mutable. Declare property setter as private to pass this check";
    }
  }
}