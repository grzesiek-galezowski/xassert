using System.Linq;
using System.Reflection;
using System.Text;

using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails;

internal class Event(EventInfo eventInfo) : IAmEvent
{
  private readonly string _shortName = eventInfo.Name.Split('.').Last();

  public string GenerateNonPublicExistenceMessage()
  {
    return new StringBuilder("SmartType: ")
      .Append(eventInfo.DeclaringType)
      .Append(" contains non public event ")
      .Append(eventInfo.Name)
      .Append(" of type ")
      .Append(eventInfo.EventHandlerType).ToString();
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