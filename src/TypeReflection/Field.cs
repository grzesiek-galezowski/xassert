namespace TddXt.XAssert.TypeReflection
{
  using System;
  using System.Linq.Expressions;
  using System.Reflection;
  using System.Runtime.CompilerServices;
  using System.Text;

  using TddXt.XAssert.CommonTypes;
  using TddXt.XAssert.TypeReflection.ImplementationDetails;
  using TddXt.XAssert.TypeReflection.Interfaces;

  public class Field : IAmField
  {
    private readonly FieldInfo _fieldInfo;

    public Field(FieldInfo fieldInfo)
    {
      this._fieldInfo = fieldInfo;
    }

    public string Name
    {
      get { return this._fieldInfo.Name; }
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

    public bool IsNotDeveloperDefinedReadOnlyField()
    {
      return !this._fieldInfo.IsInitOnly && !this._fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute), true);
    }

    public bool IsConstant()
    {
      return this._fieldInfo.IsLiteral && this.IsNotDeveloperDefinedReadOnlyField();
    }

    public string ShouldNotBeMutableButIs()
    {
      return "Value objects are immutable, but field "
             + this._fieldInfo.Name
             + " of type " + this._fieldInfo.DeclaringType + " is mutable. Make this field readonly to pass the check.";
    }

    public string GenerateExistenceMessage()
    {
      return "SmartType: " + this._fieldInfo.DeclaringType +
             " contains static field " + this._fieldInfo.Name +
             " of type " + this._fieldInfo.FieldType;

    }

    private bool HasTheSameNameAs(IAmField otherConstant)
    {
      return otherConstant.HasName(this._fieldInfo.Name);
    }

    public bool HasName(string name)
    {
      return this._fieldInfo.Name == name;
    }

    private bool HasTheSameValueAs(IAmField otherConstant)
    {
      return otherConstant.HasValue(this._fieldInfo.GetValue(null));
    }

    public bool HasValue(object name)
    {
      return this._fieldInfo.GetValue(null).Equals(name);
    }

    public void AssertNotDuplicateOf(IAmField otherConstant)
    {
      if (!this.HasTheSameNameAs(otherConstant))
      {
        if (this.HasTheSameValueAs(otherConstant))
        {
          var builder = new StringBuilder();
          this.AddNameTo(builder);
          builder.Append(" is a duplicate of ");
          otherConstant.AddNameTo(builder);
          throw new DuplicateConstantException(builder.ToString());
        }
      }
    }

    public void AddNameTo(StringBuilder builder)
    {
      builder.Append(this._fieldInfo.Name + " <" + this._fieldInfo.GetValue(null) + ">");
    }

  }
}