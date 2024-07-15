using System;
using System.Threading;
using TddXt.XFluentAssert.Api.LockAssertions.Interfaces;

namespace TddXt.XFluentAssert.LockAssertions;

internal class MonitorAssertions(object aLock) : ILockAssertions
{
  public void AssertUnlocked()
  {
    try
    {
      Monitor.Exit(aLock);
      throw new Exception("Expected lock not being held, but it is!");
    }
    catch (SynchronizationLockException)
    {
    }
  }

  public void AssertLocked()
  {
    Monitor.Exit(aLock);
    Monitor.Enter(aLock);
  }
}