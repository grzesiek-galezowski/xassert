namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails
{
  using System;
  using System.Linq;
  using System.Reflection;
  using System.Text;

  using Interfaces;

  public class Event : IAmEvent
  {
    private readonly EventInfo _eventInfo;

    private readonly string _shortName;

    public Event(EventInfo eventInfo)
    {
      _eventInfo = eventInfo;
      _shortName = eventInfo.Name.Split('.').Last();
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

    public bool HasName(string eventName)
    {
      return _shortName.Equals(eventName);
    }

    public string Name() => _shortName;

    public override string ToString()
    {
      return _shortName;
    }
  }
}