using System.Threading;

using AwesomeAssertions;
using TddXt.XFluentAssert.Api.LockAssertions.Interfaces;

namespace TddXt.XFluentAssert.LockAssertions;

internal class WriteLockSlimAssertions(ReaderWriterLockSlim aLock) : ILockAssertions
{
  public void AssertUnlocked()
  {
    aLock.IsWriteLockHeld.Should().BeFalse("Expected write lock not being held at this moment, but it is!");
    AssertAlternativeLocksNotHeld();
  }

  private void AssertAlternativeLocksNotHeld()
  {
    aLock.IsReadLockHeld.Should().BeFalse("Expected read lock not being held at all, but it is!");
  }

  public void AssertLocked()
  {
    aLock.IsWriteLockHeld.Should().BeTrue("Expected write lock being held, but it is not!");
    AssertAlternativeLocksNotHeld();
  }
}