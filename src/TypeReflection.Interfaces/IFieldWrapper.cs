using System;
using System.Text;

namespace TypeReflection.Interfaces
{
  public interface IFieldWrapper
  {
    bool IsNotDeveloperDefinedReadOnlyField();
    string ShouldNotBeMutableButIs();
    string GenerateExistenceMessage();
    bool HasName(string name);
    bool HasValue(object name);
    void AssertNotDuplicateOf(IFieldWrapper otherConstant);
    void AddNameTo(StringBuilder builder);
  }
}
