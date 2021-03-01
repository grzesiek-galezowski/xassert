using System.Threading;
using TddXt.XFluentAssert.LockAssertions;

namespace TddXt.XFluentAssert.Api.LockAssertions
{
  public static class Blocking
  {
    public static XFluentAssert.LockAssertions.ILockAssertions ReadOn(ReaderWriterLockSlim @lock)
    {
      return new ReadLockSlimAssertions(@lock);
    }

    public static XFluentAssert.LockAssertions.ILockAssertions WriteOn(ReaderWriterLockSlim @lock)
    {
      return new WriteLockSlimAssertions(@lock);
    }

    public static XFluentAssert.LockAssertions.ILockAssertions MonitorOn(object @lock)
    {
      return new MonitorAssertions(@lock);
    }
  }
}