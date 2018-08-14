namespace TddXt.XAssert.TypeReflection.Interfaces
{
  public interface IAmProperty
  {
    bool HasPublicSetter();
    string ShouldNotBeMutableButIs();
  }
}
