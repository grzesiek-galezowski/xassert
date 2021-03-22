﻿using TddXt.XFluentAssert.Api;
using TddXt.XFluentAssert.Api.LockAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Exceptions;
using TddXt.XFluentAssert.EndToEndSpecification.Fixtures;
using Xunit;
using Xunit.Sdk;

namespace TddXt.XFluentAssert.EndToEndSpecification
{
  public class MonitorSynchronizationSpecification
  {
    [Fact]
    public void ShouldNotThrowWhenVoidMethodIsMonitorSynchronizedCorrectly()
    {
      //GIVEN
      var wrappedObjectMock = Substitute.For<IMyService>();
      var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

      //WHEN - THEN
      service.Should().SynchronizeAccessTo(
        s => s.VoidCall(1),
        Blocking.MonitorOn(service.Lock),
        wrappedObjectMock);
    }

    [Fact]
    public void ShouldNotThrowWhenNonVoidMethodIsMonitorSynchronizedCorrectly()
    {
      //GIVEN
      var wrappedObjectMock = Substitute.For<IMyService>();
      var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

      //WHEN - THEN
      service.Should().SynchronizeAccessTo(s => s.CallWithResult("alabama"), Blocking.MonitorOn(service.Lock), wrappedObjectMock);
    }

    [Fact]
    public void ShouldThrowWhenVoidMethodDoesNotEnterMonitorAtAll()
    {
      //GIVEN
      var wrappedObjectMock = Substitute.For<IMyService>();
      var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

      //WHEN - THEN
      new Action(() =>
        service.Should().SynchronizeAccessTo(s => s.VoidCallNotEntered(1), Blocking.MonitorOn(service.Lock), wrappedObjectMock))
      .Should().ThrowExactly<ReceivedCallsException>();
    }

    [Fact]
    public void ShouldThrowWhenVoidMethodDoesNotExitMonitor()
    {
      //GIVEN
      var wrappedObjectMock = Substitute.For<IMyService>();
      var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

      //WHEN - THEN
      new Action(() =>
        service.Should().SynchronizeAccessTo(s => s.VoidCallNotExited(1), Blocking.MonitorOn(service.Lock), wrappedObjectMock))
        .Should().ThrowExactly<Exception>();
    }

    [Fact]
    public void ShouldThrowWhenVoidMethodDoesNotExitMonitorOnException()
    {
      //GIVEN
      var wrappedObjectMock = Substitute.For<IMyService>();
      var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

      //WHEN - THEN
      new Action(() =>
        service.Should().SynchronizeAccessTo(s => s.VoidCallNotExitedOnException(1), Blocking.MonitorOn(service.Lock), wrappedObjectMock))
      .Should().ThrowExactly<ReceivedCallsException>();
    }

    [Fact]
    public void ShouldThrowWhenNonVoidMethodDoesNotEnterMonitorAtAll()
    {
      //GIVEN
      var wrappedObjectMock = Substitute.For<IMyService>();
      var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

      //WHEN - THEN
      new Action(() =>
        service.Should().SynchronizeAccessTo(s => s.CallWithResultNotEntered("alabama"), Blocking.MonitorOn(service.Lock), wrappedObjectMock))
        .Should().ThrowExactly<XunitException>();
    }

    [Fact]
    public void ShouldThrowWhenNonVoidMethodDoesNotExitMonitor()
    {
      //GIVEN
      var wrappedObjectMock = Substitute.For<IMyService>();
      var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

      //WHEN - THEN
      new Action(() =>
        service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExited("alabama"), Blocking.MonitorOn(service.Lock), wrappedObjectMock))
        .Should().ThrowExactly<Exception>();
    }
    [Fact]
    public void ShouldThrowWhenNonVoidMethodDoesNotExitMonitorOnException()
    {
      //GIVEN
      var wrappedObjectMock = Substitute.For<IMyService>();
      var service = new MonitorSynchronizedMyService(wrappedObjectMock, new object());

      //WHEN - THEN
      new Action(() =>
        service.Should().SynchronizeAccessTo(s => s.CallWithResultNotExitedOnException("alabama"), Blocking.MonitorOn(service.Lock), wrappedObjectMock))
        .Should().ThrowExactly<XunitException>();
    }
  }

  class MonitorSynchronizedMyService : SynchronizedMyService<object>
  {
    public MonitorSynchronizedMyService(IMyService innerInstance, object aLock) : base(innerInstance, aLock)
    {
    }

    protected override void ExitLock()
    {
      Monitor.Exit(Lock);
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
      Monitor.Enter(Lock);
    }
  }
}