using System.Threading;
using FluentAssertions;

namespace TddXt.XFluentAssert.LockAssertions
{
  public class SemaphoreSlimAssertions : ILockAssertions
  {
    private readonly SemaphoreSlim _semaphore;

    public SemaphoreSlimAssertions(SemaphoreSlim semaphore)
    {
      _semaphore = semaphore;
    }

    public void AssertUnlocked()
    {
      _semaphore.CurrentCount.Should().Be(1);
    }

    public void AssertLocked()
    {
      _semaphore.CurrentCount.Should().Be(0);
    }
  }
}