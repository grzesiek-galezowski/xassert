namespace TddXt.XAssert.TypeReflection.ImplementationDetails
{
  using System.Reflection;
  using System.Text;

  using TddXt.XAssert.TypeReflection.Interfaces;

  public class Event : IAmEvent
  {
    private readonly EventInfo _eventInfo;

    public Event(EventInfo eventInfo)
    {
      this._eventInfo = eventInfo;
    }

    public string GenerateNonPublicExistenceMessage()
    {
      return new StringBuilder("SmartType: ")
        .Append(this._eventInfo.DeclaringType)
        .Append(" contains non public event ")
        .Append(this._eventInfo.Name)
        .Append(" of type ")
        .Append(this._eventInfo.EventHandlerType).ToString();

    }
  }
}