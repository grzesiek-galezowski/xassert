using System.Threading;

using FluentAssertions;
using TddXt.XFluentAssert.Api.LockAssertions.Interfaces;

namespace TddXt.XFluentAssert.LockAssertions;

internal class ReadLockSlimAssertions(ReaderWriterLockSlim aLock) : ILockAssertions
{
  public void AssertUnlocked()
  {
    aLock.IsReadLockHeld.Should().BeFalse("Expected read lock not being held at this moment, but it is!");
    AssertAlternativeLocksNotHeld();
  }


  public void AssertLocked()
  {
    aLock.IsReadLockHeld.Should().BeTrue("Expected read lock being held, but it is not!");
    AssertAlternativeLocksNotHeld();
  }

  private void AssertAlternativeLocksNotHeld()
  {
    aLock.IsWriteLockHeld.Should().BeFalse("Expected write lock being held at all, but it is!");
  }
}