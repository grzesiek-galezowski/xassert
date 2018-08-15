
namespace TddXt.XAssert.GraphAssertions.Experimental
{
  using System;
  using System.Reflection;

  using Albedo;

  public partial class XAssert //bug remove this class or move to experimental
  {

    public static void Contains(Object o, Type t) // todo this is unfinished!!!!
    {
      var propertiesAndFields = o.GetType().GetPropertiesAndFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.NonPublic);
      new SearchVisitor(o, t);
    }

  }

  //bug this is unfinished and does not work yet!!
  public class SearchVisitor : ReflectionVisitor<Boolean>
  {
    private readonly object target;
    private readonly Type _searchedType;
    private bool value = false;

    public SearchVisitor(object target, Type searchedType)
    {
      this.target = target;
      _searchedType = searchedType;
    }

    public override bool Value
    {
      get { return this.value; }
    }

    public override IReflectionVisitor<bool> Visit(
      FieldInfoElement fieldInfoElement)
    {
      if (_searchedType == fieldInfoElement.FieldInfo.FieldType)
      {
        value = true;
      }
      return this;
    }

    public override IReflectionVisitor<bool> Visit(
      PropertyInfoElement propertyInfoElement)
    {
      if (_searchedType == propertyInfoElement.PropertyInfo.PropertyType)
      {
        value = true;
      }
      return this;
    }
  }
}
