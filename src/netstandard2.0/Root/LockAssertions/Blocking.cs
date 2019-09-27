using System.Threading;
using TddXt.XFluentAssert.LockAssertions;

namespace TddXt.XFluentAssertRoot.LockAssertions
{
  public static class Blocking
  {
    public static XFluentAssert.LockAssertions.LockAssertions ReadOn(ReaderWriterLockSlim @lock)
    {
      return new ReadLockSlimAssertions(@lock);
    }

    public static XFluentAssert.LockAssertions.LockAssertions WriteOn(ReaderWriterLockSlim @lock)
    {
      return new WriteLockSlimAssertions(@lock);
    }

    public static XFluentAssert.LockAssertions.LockAssertions MonitorOn(object @lock)
    {
      return new MonitorAssertions(@lock);
    }
  }
}