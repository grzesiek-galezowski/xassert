using System;
using System.Text;

namespace TypeReflection.Interfaces
{
  public interface IFieldWrapper2
  {
    bool IsNotDeveloperDefinedReadOnlyField();
    string ShouldNotBeMutableButIs();
    string GenerateExistenceMessage();
    void SetValue(object result, object instance);
    Type FieldType { get; }
    bool HasTheSameNameAs(IFieldWrapper2 otherConstant);
    bool HasName(string name);
    bool HasTheSameValueAs(IFieldWrapper2 otherConstant);
    bool HasValue(object name);
    void AssertNotDuplicateOf(IFieldWrapper2 otherConstant);
    void AddNameTo(StringBuilder builder);
  }
}
