using System.Text;

namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  internal interface IAmField
  {
    bool IsNotDeveloperDefinedReadOnlyField();
    string ShouldNotBeMutableButIs();
    string GenerateExistenceMessage();
    bool HasName(string name);
    bool HasValue(object name);
    void AssertNotDuplicateOf(IAmField otherConstant);
    void AddNameTo(StringBuilder builder);
    bool IsNotSpecialCase();
  }
}
