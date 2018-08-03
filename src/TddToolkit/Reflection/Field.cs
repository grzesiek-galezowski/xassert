using System;
using System.Linq.Expressions;
using System.Reflection;
using CommonTypes;

namespace TddEbook.TddToolkit.Reflection
{
  public class Field
  {
    private readonly FieldInfo _fieldInfo;

    public Field(FieldInfo fieldInfo)
    {
      _fieldInfo = fieldInfo;
    }

    public string Name
    {
      get { return _fieldInfo.Name; }
    }

    public static Maybe<Field> FromUnaryExpression<T>(Expression<Func<T, object>> expression)
    {
      var unaryExpression = expression.Body as UnaryExpression;
      var propertyUsageExppression = unaryExpression.Operand as MemberExpression;
      if (propertyUsageExppression != null)
      {
        var fieldInfo = propertyUsageExppression.Member as FieldInfo;
        if (fieldInfo != null)
        {
          return new Field(fieldInfo);
        }
      }
      return null;
    }

  }
}