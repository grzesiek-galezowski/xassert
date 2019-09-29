namespace TddXt.XFluentAssert.TypeReflection
{
  using System;
  using System.Linq.Expressions;
  using System.Reflection;

  using CommonTypes;

  public class Property
  {
    public static Maybe<Property> FromUnaryExpression<T>(Expression<Func<T, object>> expression)
    {
      var unaryExpression = expression.Body as UnaryExpression;
      var propertyUsageExppression = unaryExpression.Operand as MemberExpression;
      if (propertyUsageExppression != null)
      {
        var propertyInfo = propertyUsageExppression.Member as PropertyInfo;
        if (propertyInfo != null)
        {
          return new Property(propertyInfo);
        }
      }
      return null;
    }


    public string Name => _propertyInfo.Name;

    private Property(PropertyInfo property)
    {
      _propertyInfo = property;
    }

    private readonly PropertyInfo _propertyInfo;
  }
}