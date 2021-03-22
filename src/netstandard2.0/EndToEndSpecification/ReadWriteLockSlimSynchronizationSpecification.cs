using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Exceptions;
using TddXt.XFluentAssert.Api;
using TddXt.XFluentAssert.Api.LockAssertions;
using TddXt.XFluentAssert.EndToEndSpecification.Fixtures;
using Xunit;
using Xunit.Sdk;

namespace TddXt.XFluentAssert.EndToEndSpecification
{
    public class ReadWriteLockSlimSynchronizationSpecification
    {
        [Fact]
        public void ShouldNotThrowWhenVoidMethodIsReadSynchronizedCorrectly()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var aLock = new ReaderWriterLockSlim();
            var service = new ReadSynchronizedMyService(wrappedObjectMock, aLock);

            //WHEN - THEN
            service.Should().SynchronizeAccessTo(s => s.VoidCall(1), Blocking.ReadOn(aLock), wrappedObjectMock);
        }

        [Fact]
        public void ShouldNotThrowWhenVoidMethodIsWriteSynchronizedCorrectly()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            service.Should().SynchronizeAccessTo(s => s.VoidCall(1), Blocking.WriteOn(service.Lock), wrappedObjectMock);
        }

        [Fact]
        public void ShouldNotThrowWhenNonVoidMethodIsReadSynchronizedCorrectly()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var aLock = new ReaderWriterLockSlim();
            var service = new ReadSynchronizedMyService(wrappedObjectMock, aLock);

            //WHEN - THEN
            service.Should().SynchronizeAccessTo(s => s.CallWithResult("alabama"), Blocking.ReadOn(aLock), wrappedObjectMock);
        }

        [Fact]
        public void ShouldNotThrowWhenNonVoidMethodIsWriteSynchronizedCorrectly()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            service.Should().SynchronizeAccessTo(s => s.CallWithResult("alabama"), Blocking.WriteOn(service.Lock), wrappedObjectMock);
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotEnterReadLockAtAll()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotEntered(1), Blocking.ReadOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<ReceivedCallsException>();
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotExitReadLock()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotExited(1), Blocking.ReadOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotExitReadLockOnException()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotExitedOnException(1), Blocking.ReadOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<ReceivedCallsException>();
        }

        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotEnterReadLockAtAll()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotEntered("alabama"), Blocking.ReadOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotExitReadLock()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExited("alabama"), Blocking.ReadOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotExitReadLockOnException()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new ReadSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExitedOnException("alabama"), Blocking.ReadOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotEnterWriteLockAtAll()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotEntered(1), Blocking.WriteOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<ReceivedCallsException>();
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotExitWriteLock()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotExited(1), Blocking.WriteOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotExitWriteLockOnException()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotExitedOnException(1), Blocking.WriteOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<ReceivedCallsException>();
        }

        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotEnterWriteLockAtAll()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotEntered("alabama"), Blocking.WriteOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotExitWriteLock()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExited("alabama"), Blocking.WriteOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotExitWriteLockOnException()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new WriteSynchronizedMyService(wrappedObjectMock, new ReaderWriterLockSlim());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExitedOnException("alabama"), Blocking.WriteOn(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }
    }

    class ReadSynchronizedMyService : SynchronizedMyService<ReaderWriterLockSlim>
    {
        public ReadSynchronizedMyService(IMyService innerInstance, ReaderWriterLockSlim aLock) : base(innerInstance, aLock)
        {
        }

        protected override void ExitLock()
        {
            Lock.ExitReadLock();
        }

        protected override Task EnterLockAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task ExitLockAsync()
        {
            throw new NotImplementedException();
        }

        protected override void EnterLock()
        {
            Lock.EnterReadLock();
        }
    }

    class WriteSynchronizedMyService : SynchronizedMyService<ReaderWriterLockSlim>
    {
        public WriteSynchronizedMyService(IMyService innerInstance, ReaderWriterLockSlim aLock) : base(innerInstance, aLock)
        {
        }

        protected override void ExitLock()
        {
            Lock.ExitWriteLock();
        }

        protected override Task EnterLockAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task ExitLockAsync()
        {
            throw new NotImplementedException();
        }

        protected override void EnterLock()
        {
            Lock.EnterWriteLock();
        }
    }
} 