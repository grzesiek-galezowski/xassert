using System.Threading;

using FluentAssertions;
using TddXt.XFluentAssert.Api.LockAssertions.Interfaces;

namespace TddXt.XFluentAssert.LockAssertions;

internal class ReadLockSlimAssertions : ILockAssertions
{
  private readonly ReaderWriterLockSlim _aLock;

  public ReadLockSlimAssertions(ReaderWriterLockSlim aLock)
  {
    _aLock = aLock;
  }

  public void AssertUnlocked()
  {
    _aLock.IsReadLockHeld.Should().BeFalse("Expected read lock not being held at this moment, but it is!");
    AssertAlternativeLocksNotHeld();
  }


  public void AssertLocked()
  {
    _aLock.IsReadLockHeld.Should().BeTrue("Expected read lock being held, but it is not!");
    AssertAlternativeLocksNotHeld();
  }

  private void AssertAlternativeLocksNotHeld()
  {
    _aLock.IsWriteLockHeld.Should().BeFalse("Expected write lock being held at all, but it is!");
  }
}