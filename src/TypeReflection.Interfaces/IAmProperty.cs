namespace TypeReflection.Interfaces
{
  public interface IAmProperty
  {
    bool HasPublicSetter();
    string ShouldNotBeMutableButIs();
  }
}
