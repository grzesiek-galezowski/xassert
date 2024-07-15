using System.Threading.Tasks;

namespace TddXt.XFluentAssert.EndToEndSpecification.Fixtures;

public abstract class SynchronizedMyService<T>(IMyService innerInstance, T aLock) : IMyService
{
  public T Lock
  {
    get;
  } = aLock;

  public void VoidCall(int i)
  {
    try
    {
      EnterLock();
      innerInstance.VoidCall(i);
    }
    finally
    {
      ExitLock();
    }
  }

  public async Task AsyncCall(int i)
  {
    try
    {
      await EnterLockAsync();
      await innerInstance.AsyncCall(i);
    }
    finally
    {
      await ExitLockAsync();
    }
  }

  public async Task AsyncCallNotEntered(int i)
  {
    await innerInstance.AsyncCallNotEntered(i);
  }

  public async Task AsyncCallNotExited(int i)
  {
    await EnterLockAsync();
    await innerInstance.AsyncCallNotExited(i);
  }

  public async Task AsyncCallNotExitedOnException(int i)
  {
    await EnterLockAsync();
    await innerInstance.AsyncCallNotExitedOnException(i);
    await ExitLockAsync();
  }

  public void VoidCallNotExitedOnException(int i)
  {
    EnterLock();
    innerInstance.VoidCall(i);
    ExitLock();
  }

  public void VoidCallNotEntered(int i)
  {
    innerInstance.VoidCall(i);
  }

  public void VoidCallNotExited(int i)
  {
    EnterLock();
    innerInstance.VoidCall(i);
  }

  protected abstract void EnterLock();
  protected abstract void ExitLock();
  protected abstract Task EnterLockAsync();
  protected abstract Task ExitLockAsync();

  public int CallWithResult(string alabama)
  {
    try
    {
      EnterLock();
      return innerInstance.CallWithResult(alabama);
    }
    finally
    {
      ExitLock();
    }
  }

  public int CallWithResultNotEntered(string alabama)
  {
    return innerInstance.CallWithResult(alabama);
  }

  public int CallWithResultNotExited(string alabama)
  {
    EnterLock();
    return innerInstance.CallWithResult(alabama);
  }

  public int CallWithResultNotExitedOnException(string alabama)
  {
    EnterLock();
    var result = innerInstance.CallWithResult(alabama);
    ExitLock();
    return result;
  }
}