using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Functional.Maybe;
using Functional.Maybe.Just;
using TddXt.XFluentAssert.TypeReflection.ImplementationDetails;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection
{
  public class Field : IAmField
  {
    private readonly FieldInfo _fieldInfo;

    public Field(FieldInfo fieldInfo)
    {
      _fieldInfo = fieldInfo;
    }

    public string Name => _fieldInfo.Name;

    public static Maybe<Field> FromUnaryExpression<T>(Expression<Func<T, object>> expression)
    {
      var unaryExpression = expression.Body as UnaryExpression;
      var propertyUsageExppression = unaryExpression.Operand as MemberExpression;
      if (propertyUsageExppression != null)
      {
        var fieldInfo = propertyUsageExppression.Member as FieldInfo;
        if (fieldInfo != null)
        {
          return new Field(fieldInfo).Just();
        }
      }
      return Maybe<Field>.Nothing;
    }

    public bool IsNotDeveloperDefinedReadOnlyField()
    {
      return !_fieldInfo.IsInitOnly && !_fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute), true);
    }

    public bool IsConstant()
    {
      return _fieldInfo.IsLiteral && IsNotDeveloperDefinedReadOnlyField();
    }

    public string ShouldNotBeMutableButIs()
    {
      return "Value objects are immutable, but field "
             + _fieldInfo.Name
             + " of type " + _fieldInfo.DeclaringType + " is mutable. Make this field readonly to pass the check.";
    }

    public string GenerateExistenceMessage()
    {
      return "SmartType: " + _fieldInfo.DeclaringType +
             " contains static field " + _fieldInfo.Name +
             " of type " + _fieldInfo.FieldType;

    }

    private bool HasTheSameNameAs(IAmField otherConstant)
    {
      return otherConstant.HasName(_fieldInfo.Name);
    }

    public bool HasName(string name)
    {
      return _fieldInfo.Name == name;
    }

    private bool HasTheSameValueAs(IAmField otherConstant)
    {
      return otherConstant.HasValue(_fieldInfo.GetValue(null));
    }

    public bool HasValue(object name)
    {
      return _fieldInfo.GetValue(null).Equals(name);
    }

    public void AssertNotDuplicateOf(IAmField otherConstant)
    {
      if (!HasTheSameNameAs(otherConstant))
      {
        if (HasTheSameValueAs(otherConstant))
        {
          var builder = new StringBuilder();
          AddNameTo(builder);
          builder.Append(" is a duplicate of ");
          otherConstant.AddNameTo(builder);
          throw new DuplicateConstantException(builder.ToString());
        }
      }
    }

    public void AddNameTo(StringBuilder builder)
    {
      builder.Append(_fieldInfo.Name + " <" + _fieldInfo.GetValue(null) + ">");
    }

    public bool IsNotSpecialCase()
    {
      return !_fieldInfo.DeclaringType.Namespace.Equals("Value");
    }
  }
}