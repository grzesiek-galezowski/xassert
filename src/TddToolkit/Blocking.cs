using System.Threading;
using LockAssertions;

namespace TddEbook.TddToolkit
{
  public static class Blocking
  {
    public static LockAssertions.LockAssertions ReadOn(ReaderWriterLockSlim aLock)
    {
      return new ReadLockSlimAssertions(aLock);
    }

    public static LockAssertions.LockAssertions WriteOn(ReaderWriterLockSlim aLock)
    {
      return new WriteLockSlimAssertions(aLock);
    }

    public static LockAssertions.LockAssertions MonitorOn(object aLock)
    {
      return new MonitorAssertions(aLock);
    }
  }
}