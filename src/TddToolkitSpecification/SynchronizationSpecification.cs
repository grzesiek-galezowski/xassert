using System;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TddEbook.TddToolkit;
using TddEbook.TddToolkitSpecification.Fixtures;

namespace TddEbook.TddToolkitSpecification
{
    public class SynchronizationSpecification
    {
      [Test]
      public void ShouldNotThrowWhenVoidMethodIsReadSynchronizedCorrectly()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var aLock = new ReaderWriterLockSlim();
        var service = new ReadSynchronizedMyService(wrappedObjectMock, aLock);

        //WHEN - THEN
        service.Should().SynchronizeAccessTo(s => s.VoidCall(1), Blocking.ReadOn(aLock), wrappedObjectMock);
      }

      [Test]
      public void ShouldNotThrowWhenVoidMethodIsWriteSynchronizedCorrectly()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        service.Should().SynchronizeAccessTo(s => s.VoidCall(1), Blocking.WriteOn(service.Lock), wrappedObjectMock);
      }

      [Test]
      public void ShouldNotThrowWhenVoidMethodIsMonitorSynchronizedCorrectly()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

        //WHEN - THEN
        service.Should().SynchronizeAccessTo(
          s => s.VoidCall(1), 
          Blocking.MonitorOn(service.Lock),
          wrappedObjectMock);
    }

      [Test]
      public void ShouldNotThrowWhenNonVoidMethodIsReadSynchronizedCorrectly()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var aLock = new ReaderWriterLockSlim();
        var service = new ReadSynchronizedMyService(wrappedObjectMock, aLock);

        //WHEN - THEN
        service.Should().SynchronizeAccessTo(s => s.CallWithResult("alabama"), Blocking.ReadOn(aLock), wrappedObjectMock);
      }

      [Test]
      public void ShouldNotThrowWhenNonVoidMethodIsWriteSynchronizedCorrectly()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        service.Should().SynchronizeAccessTo(s => s.CallWithResult("alabama"), Blocking.WriteOn(service.Lock), wrappedObjectMock);
      }

      [Test]
      public void ShouldNotThrowWhenNonVoidMethodIsMonitorSynchronizedCorrectly()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

        //WHEN - THEN
        service.Should().SynchronizeAccessTo(s => s.CallWithResult("alabama"), Blocking.MonitorOn(service.Lock), wrappedObjectMock);
      }

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotEnterMonitorAtAll()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

        //WHEN - THEN
        Assert.Catch<Exception>(() => 
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotEntered(1), Blocking.MonitorOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotExitMonitor()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

        //WHEN - THEN
        Assert.Catch<Exception>(() => 
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotExited(1), Blocking.MonitorOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotExitMonitorOnException()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

        //WHEN - THEN
        Assert.Catch<Exception>(() => 
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotExitedOnException(1), Blocking.MonitorOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotEnterMonitorAtAll()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

        //WHEN - THEN
        Assert.Catch<Exception>(() => 
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotEntered("alabama"), Blocking.MonitorOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotExitMonitor()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

        //WHEN - THEN
        Assert.Catch<Exception>(() => 
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExited("alabama"), Blocking.MonitorOn(service.Lock), wrappedObjectMock));
      }
      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotExitMonitorOnException()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

        //WHEN - THEN
        Assert.Catch<Exception>(() => 
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExitedOnException("alabama"), Blocking.MonitorOn(service.Lock), wrappedObjectMock));
      }

      //bug movie this elsewhere

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotEnterReadLockAtAll()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotEntered(1), Blocking.ReadOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotExitReadLock()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotExited(1), Blocking.ReadOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotExitReadLockOnException()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotExitedOnException(1), Blocking.ReadOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotEnterReadLockAtAll()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotEntered("alabama"), Blocking.ReadOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotExitReadLock()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExited("alabama"), Blocking.ReadOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotExitReadLockOnException()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExitedOnException("alabama"), Blocking.ReadOn(service.Lock), wrappedObjectMock));
      }

      //bug end
      //bug movie this elsewhere

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotEnterWriteLockAtAll()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotEntered(1), Blocking.WriteOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotExitWriteLock()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotExited(1), Blocking.WriteOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenVoidMethodDoesNotExitWriteLockOnException()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.VoidCallNotExitedOnException(1), Blocking.WriteOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotEnterWriteLockAtAll()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotEntered("alabama"), Blocking.WriteOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotExitWriteLock()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExited("alabama"), Blocking.WriteOn(service.Lock), wrappedObjectMock));
      }

      [Test]
      public void ShouldThrowWhenNonVoidMethodDoesNotExitWriteLockOnException()
      {
        //GIVEN
        var wrappedObjectMock = Substitute.For<MyService>();
        var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

        //WHEN - THEN
        Assert.Catch<Exception>(() =>
          service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExitedOnException("alabama"), Blocking.WriteOn(service.Lock), wrappedObjectMock));
      }

      //bug end

      private class ReadSynchronizedMyService : SynchronizedMyService<ReaderWriterLockSlim>
      {
        public ReadSynchronizedMyService(MyService innerInstance, ReaderWriterLockSlim aLock) : base(innerInstance, aLock)
        {
        }

        protected override void ExitLock()
        {
          Lock.ExitReadLock();
        }

        protected override void EnterLock()
        {
          Lock.EnterReadLock();
        }
      }

      private class WriteSynchronizedMyService : SynchronizedMyService<ReaderWriterLockSlim>
      {
        public WriteSynchronizedMyService(MyService innerInstance, ReaderWriterLockSlim aLock) : base(innerInstance, aLock)
        {
        }

        protected override void ExitLock()
        {
          Lock.ExitWriteLock();
        }

        protected override void EnterLock()
        {
          Lock.EnterWriteLock();
        }
      }
      
      private class MonitorSynchronizedMyService : SynchronizedMyService<object>
      {
        public MonitorSynchronizedMyService(MyService innerInstance, object aLock) : base(innerInstance, aLock)
        {
        }

        protected override void ExitLock()
        {
          Monitor.Exit(Lock);
        }

        protected override void EnterLock()
        {
          Monitor.Enter(Lock);
        }
      }

    }
}
