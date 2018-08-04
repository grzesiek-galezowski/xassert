using System.Reflection;
using System.Text;
using TypeReflection.Interfaces;

namespace TypeReflection.ImplementationDetails
{
  public class Event : IAmEvent
  {
    private readonly EventInfo _eventInfo;

    public Event(EventInfo eventInfo)
    {
      _eventInfo = eventInfo;
    }

    public string GenerateNonPublicExistenceMessage()
    {
      return new StringBuilder("SmartType: ")
        .Append(_eventInfo.DeclaringType)
        .Append(" contains non public event ")
        .Append(_eventInfo.Name)
        .Append(" of type ")
        .Append(_eventInfo.EventHandlerType).ToString();

    }
  }
}