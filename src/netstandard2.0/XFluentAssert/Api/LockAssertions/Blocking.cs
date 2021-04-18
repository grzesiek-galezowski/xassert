using System.Threading;
using TddXt.XFluentAssert.LockAssertions;

namespace TddXt.XFluentAssert.Api.LockAssertions
{
  public static class Blocking
  {
    public static ILockAssertions ReadOn(ReaderWriterLockSlim @lock)
    {
      return new ReadLockSlimAssertions(@lock);
    }

    public static ILockAssertions WriteOn(ReaderWriterLockSlim @lock)
    {
      return new WriteLockSlimAssertions(@lock);
    }

    public static ILockAssertions MonitorOn(object @lock)
    {
      return new MonitorAssertions(@lock);
    }

    public static ILockAssertions On(SemaphoreSlim semaphore)
    {
      return new SemaphoreSlimAssertions(semaphore);
    }
  }
}