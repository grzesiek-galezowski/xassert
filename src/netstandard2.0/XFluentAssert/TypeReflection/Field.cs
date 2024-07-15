using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Core.Maybe;
using TddXt.XFluentAssert.TypeReflection.ImplementationDetails;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection;

internal class Field(FieldInfo fieldInfo) : IAmField
{
  public string Name => fieldInfo.Name;

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
    return !fieldInfo.IsInitOnly && !fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute), true);
  }

  public bool IsConstant()
  {
    return fieldInfo.IsLiteral && IsNotDeveloperDefinedReadOnlyField();
  }

  public string ShouldNotBeMutableButIs()
  {
    return "Value objects are immutable, but field "
           + fieldInfo.Name
           + " of type " + fieldInfo.DeclaringType + " is mutable. Make this field readonly to pass the check.";
  }

  public string GenerateExistenceMessage()
  {
    return "SmartType: " + fieldInfo.DeclaringType +
           " contains static field " + fieldInfo.Name +
           " of type " + fieldInfo.FieldType;

  }

  private bool HasTheSameNameAs(IAmField otherConstant)
  {
    return otherConstant.HasName(fieldInfo.Name);
  }

  public bool HasName(string name)
  {
    return fieldInfo.Name == name;
  }

  private bool HasTheSameValueAs(IAmField otherConstant)
  {
    return otherConstant.HasValue(fieldInfo.GetValue(null));
  }

  public bool HasValue(object name)
  {
    return fieldInfo.GetValue(null).Equals(name);
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
    builder.Append(fieldInfo.Name + " <" + fieldInfo.GetValue(null) + ">");
  }

  public bool IsNotSpecialCase()
  {
    return !fieldInfo.DeclaringType.Namespace.Equals("Value");
  }
}