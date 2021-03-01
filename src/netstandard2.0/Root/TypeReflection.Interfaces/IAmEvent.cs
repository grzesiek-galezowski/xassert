namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  public interface IAmEvent
  {
    string GenerateNonPublicExistenceMessage();

    bool HasName(string eventName);

    string Name();
  }
}