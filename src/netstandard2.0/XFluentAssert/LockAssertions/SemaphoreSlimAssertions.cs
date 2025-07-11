using System.Threading;
using AwesomeAssertions;
using TddXt.XFluentAssert.Api.LockAssertions.Interfaces;

namespace TddXt.XFluentAssert.LockAssertions;

internal class SemaphoreSlimAssertions(SemaphoreSlim semaphore) : ILockAssertions
{
  public void AssertUnlocked()
  {
    semaphore.CurrentCount.Should().Be(1);
  }

  public void AssertLocked()
  {
    semaphore.CurrentCount.Should().Be(0);
  }
}