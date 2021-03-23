﻿using System.Threading.Tasks;

namespace TddXt.XFluentAssert.EndToEndSpecification.Fixtures
{
  public abstract class SynchronizedMyService<T> : IMyService
  {
    private readonly IMyService _innerInstance;

    protected SynchronizedMyService(IMyService innerInstance, T aLock)
    {
      _innerInstance = innerInstance;
      Lock = aLock;
    }

    public T Lock
    {
      get;
    }

    public void VoidCall(int i)
    {
      try
      {
        EnterLock();
        _innerInstance.VoidCall(i);
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
        await _innerInstance.AsyncCall(i);
      }
      finally
      {
        await ExitLockAsync();
      }
    }

    public async Task AsyncCallNotEntered(int i)
    {
      await _innerInstance.AsyncCall(i);
    }

    public async Task AsyncCallNotExited(int i)
    {
      await EnterLockAsync();
      await _innerInstance.AsyncCall(i);
    }

    public async Task AsyncCallNotExitedOnException(int i)
    {
        await EnterLockAsync();
        await _innerInstance.AsyncCall(i);
        await ExitLockAsync();
    }

    public void VoidCallNotExitedOnException(int i)
    {
      EnterLock();
      _innerInstance.VoidCall(i);
      ExitLock();
    }

    public void VoidCallNotEntered(int i)
    {
      _innerInstance.VoidCall(i);
    }

    public void VoidCallNotExited(int i)
    {
      EnterLock();
      _innerInstance.VoidCall(i);
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
        return _innerInstance.CallWithResult(alabama);
      }
      finally
      {
        ExitLock();
      }
    }

    public int CallWithResultNotEntered(string alabama)
    {
      return _innerInstance.CallWithResult(alabama);
    }

    public int CallWithResultNotExited(string alabama)
    {
      EnterLock();
      return _innerInstance.CallWithResult(alabama);
    }

    public int CallWithResultNotExitedOnException(string alabama)
    {
      EnterLock();
      var result = _innerInstance.CallWithResult(alabama);
      ExitLock();
      return result;
    }
  }
}