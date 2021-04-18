namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  internal interface IAmEvent
  {
    string GenerateNonPublicExistenceMessage();

    bool HasName(string eventName);

    string Name();
  }
}