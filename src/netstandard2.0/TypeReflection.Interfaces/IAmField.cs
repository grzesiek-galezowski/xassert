namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  using System.Text;

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
