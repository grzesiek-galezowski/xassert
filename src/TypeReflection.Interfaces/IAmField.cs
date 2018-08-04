using System;
using System.Text;

namespace TypeReflection.Interfaces
{
  public interface IAmField
  {
    bool IsNotDeveloperDefinedReadOnlyField();
    string ShouldNotBeMutableButIs();
    string GenerateExistenceMessage();
    bool HasName(string name);
    bool HasValue(object name);
    void AssertNotDuplicateOf(IAmField otherConstant);
    void AddNameTo(StringBuilder builder);
  }
}
