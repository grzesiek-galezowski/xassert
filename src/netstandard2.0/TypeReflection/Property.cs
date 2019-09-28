namespace TddXt.XFluentAssert.TypeReflection
{
  using System;
  using System.Linq.Expressions;
  using System.Reflection;

  using CommonTypes;

  public class Property
  {
    public static Property ObjectOf<T>(Expression<Func<T, object>> expression)
    {
      var propertyUsageExppression = expression.Body as MemberExpression;
      if (propertyUsageExppression != null)
      {
        var propertyInfo = propertyUsageExppression.Member as PropertyInfo;
        if (propertyInfo != null)
        {
          return new Property(propertyInfo);
        }
      }
      throw new Exception("The expression is not a property body");
    }

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


    //todo change to method that returns property info
    public static Property ValueOf<T, U>(Expression<Func<T, U>> expression) where U : struct
    {
      var propertyUsageExppression = expression.Body as MemberExpression;
      if (propertyUsageExppression != null)
      {
        var propertyInfo = propertyUsageExppression.Member as PropertyInfo;
        if (propertyInfo != null)
        {
          return new Property(propertyInfo);
        }
      }
      throw new Exception("The expression is not a property body");
    }

    public string Name
    {
      get { return _propertyInfo.Name; }
    }

    private Property(PropertyInfo property)
    {
      _propertyInfo = property;
    }

    private readonly PropertyInfo _propertyInfo;
  }
}