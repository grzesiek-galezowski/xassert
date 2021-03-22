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
    public class SemaphoreSynchronizationSpecification
    {
        private static SemaphoreSlim Semaphore()
        {
            return new SemaphoreSlim(1, 1);
        }

        [Fact]
        public void ShouldNotThrowWhenVoidMethodIsSynchronizedCorrectly()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            service.Should().SynchronizeAccessTo(
                s => s.VoidCall(1),
                Blocking.On(service.Lock),
                wrappedObjectMock);
        }
        
        [Fact]
        public void ShouldNotThrowWhenAsyncMethodIsSynchronizedCorrectly()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            service.Should().SynchronizeAccessTo(
                s => s.AsyncCall(1),
                Blocking.On(service.Lock),
                wrappedObjectMock);
        }

        [Fact]
        public void ShouldNotThrowWhenNonVoidMethodIsSynchronizedCorrectly()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            service.Should().SynchronizeAccessTo(s => s.CallWithResult("alabama"), Blocking.On(service.Lock), wrappedObjectMock);
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotEnterSynchronization()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotEntered(1), Blocking.On(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<ReceivedCallsException>();
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotExitSynchronization()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotExited(1), 
                        Blocking.On(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenVoidMethodDoesNotExitSynchronizationOnException()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.VoidCallNotExitedOnException(1), Blocking.On(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<ReceivedCallsException>();
        }

        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotEnterSynchronizationAtAll()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotEntered("alabama"), 
                        Blocking.On(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }

        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotExitSynchronization()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExited("alabama"), 
                        Blocking.On(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }
        [Fact]
        public void ShouldThrowWhenNonVoidMethodDoesNotExitSynchronizationOnException()
        {
            //GIVEN
            var wrappedObjectMock = Substitute.For<IMyService>();
            var service = new SemaphoreSynchronizedMyService(wrappedObjectMock, Semaphore());

            //WHEN - THEN
            new Action(() =>
                    service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExitedOnException("alabama"), Blocking.On(service.Lock), wrappedObjectMock))
                .Should().ThrowExactly<XunitException>();
        }
    }

    class SemaphoreSynchronizedMyService : SynchronizedMyService<SemaphoreSlim>
    {
        public SemaphoreSynchronizedMyService(IMyService innerInstance, SemaphoreSlim aLock) : base(innerInstance, aLock)
        {
        }

        protected override void ExitLock()
        {
            Lock.Release();
        }

        protected override Task EnterLockAsync()
        {
            return Lock.WaitAsync();
        }

        protected override Task ExitLockAsync()
        {
            Lock.Release();
            return Task.CompletedTask;
        }

        protected override void EnterLock()
        {
            Lock.Wait();
        }
    }
}