using System.Threading;
using LockAssertions;

namespace TddEbook.TddToolkit
{
  public static class Blocking
  {
    public static LockAssertions.LockAssertions ReadOn(ReaderWriterLockSlim @lock)
    {
      return new ReadLockSlimAssertions(@lock);
    }

    public static LockAssertions.LockAssertions WriteOn(ReaderWriterLockSlim @lock)
    {
      return new WriteLockSlimAssertions(@lock);
    }

    public static LockAssertions.LockAssertions MonitorOn(object @lock)
    {
      return new MonitorAssertions(@lock);
    }
  }
}