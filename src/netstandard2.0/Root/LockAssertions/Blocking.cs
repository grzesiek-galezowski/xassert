namespace TddXt.XFluentAssert.Root.LockAssertions
{
  using System.Threading;

  using TddXt.XFluentAssert.LockAssertions;

  public static class Blocking
  {
    public static LockAssertions ReadOn(ReaderWriterLockSlim @lock)
    {
      return new ReadLockSlimAssertions(@lock);
    }

    public static LockAssertions WriteOn(ReaderWriterLockSlim @lock)
    {
      return new WriteLockSlimAssertions(@lock);
    }

    public static LockAssertions MonitorOn(object @lock)
    {
      return new MonitorAssertions(@lock);
    }
  }
}