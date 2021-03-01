using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using FluentAssertions;
using Functional.Maybe;
using NSubstitute;
using TddXt.XFluentAssert.Api;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using static TddXt.AnyRoot.Root;
using ObjectDependencyAssertions = TddXt.XFluentAssert.Api.ObjectDependencyAssertions;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications
{
  public class GraphAssertionsSpecification : IDisposable
  {
    private readonly ITestOutputHelper _output;
    private StringWriter _stringWriter;

    public GraphAssertionsSpecification(ITestOutputHelper output)
    {
      this._output = output;
      _stringWriter = new StringWriter();
      Console.SetOut(_stringWriter);
    }

    [Fact]
    public void ShouldAllowAssertingOnTypeDependency()
    {
      //TODO split into several facts
      ObjectDependencyAssertions.DependOn<int>(new A1().Should());
      ObjectDependencyAssertions.DependOn<A2>(new A1().Should());
      ObjectDependencyAssertions.DependOn<A3>(new A1().Should());
      ObjectDependencyAssertions.DependOn<B2>(new A1().Should());
      ObjectDependencyAssertions.DependOn<B3>(new A1().Should());

      new Action(() => ObjectDependencyAssertions.DependOn<SecurityAttribute>(new A1().Should()))
        .Should().ThrowExactly<XunitException>()
        .WithMessage("Could not find " +
                     "System.Security.Permissions.SecurityAttribute " +
                     "anywhere in dependency graph");

      new Action(() => ObjectDependencyAssertions.DependOn<A1>(new A1().Should()))
        .Should().ThrowExactly<XunitException>()
        .WithMessage("Could not find " +
                     "TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.A1 " +
                     "anywhere in dependency graph");


    }

    [Fact]
    public void ShouldAllowAssertingOnInstanceDependency()
    {
      //TODO split into several facts
      var a3 = new A3();
      var b3 = new B3();
      var abc = "abc";
      var num = 123;
      var a2 = new A2(a3, b3, abc, num);
      var b2 = new B2(b3);
      var a1 = new A1(a2, b2);

      ObjectDependencyAssertions.DependOn(a1.Should(), a2);
      ObjectDependencyAssertions.DependOn(a1.Should(), b2);
      ObjectDependencyAssertions.DependOn(a1.Should(), b3);
      ObjectDependencyAssertions.DependOn(a1.Should(), abc);
      ObjectDependencyAssertions.DependOn(a1.Should(), num);

      new Action(() => ObjectDependencyAssertions.DependOn(a1.Should(), new A2()))
        .Should().ThrowExactly<XunitException>().WithMessage(
          @"Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.A2 anywhere in dependency graph however, another instance of this type was found within the following paths:
[Root(A1)]->[_a2(A2)]->[_a3(A3)]
[Root(A1)]->[_a2(A2)]->[_b3(B3)]
[Root(A1)]->[_a2(A2)]->[_str(String)]
[Root(A1)]->[_a2(A2)]->[_num(Int32)]");
      new Action(() => ObjectDependencyAssertions.DependOn(a1.Should(), new B2()))
        .Should().ThrowExactly<XunitException>().WithMessage(
          @"Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.B2 anywhere in dependency graph however, another instance of this type was found within the following paths:
[Root(A1)]->[_b2(B2)]->[_b3(B3)]");
      new Action(() => ObjectDependencyAssertions.DependOn(a1.Should(), new B3()))
        .Should().ThrowExactly<XunitException>().WithMessage(
          @"Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.B3 anywhere in dependency graph however, another instance of this type was found within the following paths:
[Root(A1)]->[_a2(A2)]->[_b3(B3)]
[Root(A1)]->[_b2(B2)]->[_b3(B3)]");
      new Action(() => ObjectDependencyAssertions.DependOn(a1.Should(), abc + "a"))
        .Should().ThrowExactly<XunitException>().WithMessage(
          @"Could not find the particular instance: abca anywhere in dependency graph however, another instance of this type was found within the following paths:
[Root(A1)]->[_a2(A2)]->[_str(String)]");
      new Action(() => ObjectDependencyAssertions.DependOn(a1.Should(), num + 1))
        .Should().ThrowExactly<XunitException>().WithMessage(
          @"Could not find the particular instance: 124 anywhere in dependency graph however, another instance of this type was found within the following paths:
[Root(A1)]->[_a2(A2)]->[_num(Int32)]");

      new Action(() => ObjectDependencyAssertions.DependOn(a1.Should(), a1))
        .Should().ThrowExactly<XunitException>().Which.Message.Contains(
          @"Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.A1 anywhere in dependency graph");
      new Action(() => ObjectDependencyAssertions.DependOn(a2.Should(), a1))
        .Should().ThrowExactly<XunitException>().Which.Message.Contains(
          "Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.A1 anywhere in dependency graph");

      ObjectDependencyAssertions.DependOn(abc.Should(), 3);
    }

    [Fact]
    public void ShouldAllowUsingReadOnlyDictionaryInsideAnObject()
    {
      //GIVEN
      //WHEN
      var readOnlyDictionary = Any.Instance<IReadOnlyDictionary<string, string>>();
      IObjectWithReadOnlyDictionary obj = new ObjectWithReadOnlyDictionary(readOnlyDictionary);
      //THEN
      ObjectDependencyAssertions.DependOn(obj.Should(), readOnlyDictionary);
    }

    [Fact]
    public void ShouldBeAbleToAssertOnDependencyObjectChainsSuccessfully()
    {
      //GIVEN
      var decorator4 = new Decorator4();
      var decorator3 = new Decorator3(decorator4);
      var decorator2 = new Decorator2(decorator3);

      //WHEN
      var decorator1 = new Decorator1(decorator2);

      //THEN
      ObjectDependencyAssertions.DependOnChain(ObjectDependencyAssertions.DependOnChain(decorator1.Should(), decorator2, decorator3).And, decorator2, decorator3, decorator4);
    }

    [Fact]
    public void ShouldThrowExceptionWhenChainCannotBeFound()
    {
      //GIVEN
      var decorator4 = new Decorator4();
      var decorator3 = new Decorator3(decorator4);
      var decorator2 = new Decorator2(decorator3);

      //WHEN
      var decorator1 = new Decorator1(decorator2);

      //THEN
      new Action(() => { ObjectDependencyAssertions.DependOnChain(decorator1.Should(), decorator2, decorator3, decorator1); })
        .Should().ThrowExactly<XunitException>()
        .WithMessage(
          @"Could not find the particular sequence of objects: [TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.Decorator2, TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.Decorator3, TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.Decorator1] anywhere in dependency graph. Paths searched:
 [Root(Decorator1)]->[_decorator(Decorator2)]->[_decorator3(Decorator3)]->[_decorator4(Decorator4)]");
    }

    [Fact]
    public void ShouldBeAbleToAssertOnDependencyTypeChainsSuccessfully()
    {
      //GIVEN
      var decorator4 = new Decorator4();
      var decorator3 = new Decorator3(decorator4);
      var decorator2 = new Decorator2(decorator3);

      //WHEN
      var decorator1 = new Decorator1(decorator2);

      //THEN
      ObjectDependencyAssertions.DependOnTypeChain(decorator1.Should(), decorator2.GetType(), decorator3.GetType());
      ObjectDependencyAssertions.DependOnTypeChain(decorator1.Should(), decorator2.GetType(), decorator3.GetType(), decorator4.GetType());
    }

    [Fact]
    public void ShouldThrowExceptionWhenTypeChainCannotBeFound()
    {
      //GIVEN
      var decorator4 = new Decorator4();
      var decorator3 = new Decorator3(decorator4);
      var decorator2 = new Decorator2(decorator3);

      //WHEN
      var decorator1 = new Decorator1(decorator2);

      //THEN
      new Action(() =>
        {
          ObjectDependencyAssertions.DependOnTypeChain(decorator1.Should(), decorator2.GetType(), decorator3.GetType(), decorator1.GetType());
        })
        .Should().ThrowExactly<XunitException>()
        .WithMessage(
          @"Could not find the particular sequence of objects: [TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.Decorator2, TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.Decorator3, TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.Decorator1] anywhere in dependency graph. Paths searched:
 [Root(Decorator1)]->[_decorator(Decorator2)]->[_decorator3(Decorator3)]->[_decorator4(Decorator4)]");
    }

    [Fact]
    public void ShouldNotFailWhenInvokedOnObjectWithProxies()
    {
      var enumerable = Substitute.For<IEnumerable<int>>();
      new Action(() =>
          ObjectDependencyAssertions.DependOn(new MyObjectImpl(Substitute.For<IEnumerable<int>>()).Should(), "lol"))
        .Should().ThrowExactly<XunitException>();

      new Action(() => { ObjectDependencyAssertions.DependOn(new MyObjectImpl(enumerable).Should(), enumerable); })
        .Should().NotThrow();

      new Action(() => ObjectDependencyAssertions.DependOn(new MyObjectImpl(Substitute.For<IEnumerable<int>>())
            .Should(), Substitute.For<IEnumerable<int>>()))
        .Should().ThrowExactly<XunitException>();
    }

    [Fact]
    public void ShouldBeAbleToFindItemsWithinCollections()
    {
      ObjectDependencyAssertions.DependOn(new List<string> { "trolololo" }.Should(), "trolololo");

      new Action(() => ObjectDependencyAssertions.DependOn(new List<string> { "trolololo" }.Should(), "trolololo2"))
        .Should().ThrowExactly<XunitException>()
        .WithMessage(
          @"Could not find the particular instance: trolololo2 anywhere in dependency graph however, another instance of this type was found within the following paths:
[Root(List`1)]->[_items(String[])]->[array element[0](String)]");
    }

    [Fact]
    public void ShouldSupportCancellationTokens()
    {
      //GIVEN
      var cancellationToken = new CancellationToken(true);

      //WHEN - THEN
      ObjectDependencyAssertions.DependOn<CancellationTokenSource>(cancellationToken.Should());
      ObjectDependencyAssertions.DependOn<ManualResetEvent>(cancellationToken.Should());
    }

    [Fact]
    public void ShouldSupportDateTimes()
    {
      ObjectDependencyAssertions.DependOn<DateTime>(Any.Instance<SomethingWithTime>().Should());
    }

    [Fact]
    public void ShouldAllowSpecifyingAdditionalTypesToSkip()
    {
      new
      {
        x = Maybe<int>.Nothing,
        y = 12
      }.Invoking(o => ObjectDependencyAssertions.DependOn(o.Should(), 12)).Should().Throw<Exception>();

      new
      {
        x = Maybe<int>.Nothing,
        y = 12
      }.Invoking(o => o.Should().DependOn(12, options => options.SkipType<Maybe<int>>()))
        .Should().NotThrow();

      new
      {
        x = Maybe<int>.Nothing,
        y = 12
      }.Invoking(o => o.Should().DependOn<int>(options => options.SkipType<Maybe<int>>())).Should().NotThrow();
    }

    [Fact]
    public void ShouldAllowSpecifyingAdditionalObjectsToSkip()
    {
      new
      {
        x = Maybe<int>.Nothing,
        y = 12
      }.Invoking(o => ObjectDependencyAssertions.DependOn(o.Should(), 12)).Should().Throw<Exception>();

      new
      {
        x = Maybe<int>.Nothing,
        y = 12
      }.Invoking(o => ObjectDependencyAssertions.DependOn(o.Should(), 12, options => options.Skip(Maybe<int>.Nothing)))
        .Should().NotThrow();
    }

    [Fact]
    public void ShouldAllowSpecifyingAdditionalObjectsToSkipAvoidingNulls()
    {
      new
      {
        x = Maybe<int>.Nothing,
        y = 12
      }.Invoking(o => ObjectDependencyAssertions.DependOn(o.Should(), 12)).Should().Throw<Exception>();

      new
      {
        z = null as string,
        x = Maybe<int>.Nothing,
        y = 12,
      }.Invoking(o => ObjectDependencyAssertions.DependOn(o.Should(), 12, options => options.Skip(Maybe<int>.Nothing)))
          .Should().NotThrow();
    }

    //todo add Should().NotDependOn();
    //todo add Should().DependOn(Func matchCriteria)

    private class MyObjectImpl
    {
      private readonly IEnumerable<int> _s;

      public MyObjectImpl(IEnumerable<int> s)
      {
        _s = s;
      }
    }

    public void Dispose()
    {
      _output.WriteLine(_stringWriter.ToString());
    }
  }



  public interface IObjectWithReadOnlyDictionary
  {
  }

  public class ObjectWithReadOnlyDictionary : IObjectWithReadOnlyDictionary
  {
    private readonly IReadOnlyDictionary<string, string> _readOnlyDictionary;

    public ObjectWithReadOnlyDictionary(IReadOnlyDictionary<string, string> readOnlyDictionary)
    {
      _readOnlyDictionary = readOnlyDictionary;
    }
  }

  public class Decorator4 : IDecorator
  {
  }

  public class Decorator3 : IDecorator
  {
    private readonly IDecorator _decorator4;

    public Decorator3(IDecorator decorator4)
    {
      _decorator4 = decorator4;
    }
  }

  public class Decorator2 : IDecorator
  {
    private readonly IDecorator _decorator3;

    public Decorator2(IDecorator decorator3)
    {
      _decorator3 = decorator3;
    }
  }

  public class Decorator1 : IDecorator
  {
    private readonly IDecorator _decorator;

    public Decorator1(IDecorator decorator)
    {
      _decorator = decorator;
    }
  }

  public interface IDecorator
  {
  }

  public class SomethingWithTime
  {
    public DateTime LastAccess { get; set; }
  }
}