using System.Threading.Tasks;

namespace TddXt.XFluentAssert.EndToEndSpecification.Fixtures
{
  public interface IMyService
  {
    void VoidCall(int i);
    Task AsyncCall(int i);
    Task AsyncCallNotEntered(int i);
    Task AsyncCallNotExited(int i);
    Task AsyncCallNotExitedOnException(int i);
    void VoidCallNotExited(int i);
    void VoidCallNotExitedOnException(int i);
    void VoidCallNotEntered(int i);
    int CallWithResult(string alabama);
    int CallWithResultNotEntered(string alabama);
    int CallWithResultNotExited(string alabama);
    int CallWithResultNotExitedOnException(string alabama);
  }
}