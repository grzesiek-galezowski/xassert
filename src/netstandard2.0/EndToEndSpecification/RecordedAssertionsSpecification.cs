using TddXt.XFluentAssert.Api;
using System;
using AutoFixture;
using FluentAssertions;
using Functional.Maybe;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.XFluentAssert.AssertionConstraints;
using Xunit;
using Xunit.Sdk;

namespace TddXt.XFluentAssert.EndToEndSpecification
{
  public class RecordedAssertionsSpecification
  {
    [Fact]
    public void ShouldAddErrorMessageWhenTruthAssertionFails()
    {
      //GIVEN
      var violations = Substitute.For<IConstraintsViolations>();
      var message = Root.Any.String();

      //WHEN
      RecordedAssertions.True(false, message, violations);

      //THEN
      violations.Received(1).Add(message);
    }

    [Fact]
    public void ShouldNotAddErrorMessageWhenTruthAssertionPasses()
    {
      //GIVEN
      var violations = Substitute.For<IConstraintsViolations>();
      var message = Root.Any.String();

      //WHEN
      RecordedAssertions.True(true, message, violations);

      //THEN
      violations.DidNotReceive().Add(message);
    }


    [Fact]
    public void ShouldFailStaticFieldsAssertionIfAssemblyContainsAtLeastOneStaticField()
    {
      var assembly = typeof(RecordedAssertionsSpecification).Assembly;

      new Action(() => assembly.Should().NotHaveStaticFields()).Should().ThrowExactly<XunitException>()
        .Which.Message.Should().ContainAll(nameof(_lolek), nameof(Lol2.Gieniek), nameof(StaticProperty));
    }

    [Fact]
    public void ShouldFailReferenceAssertionWhenAssemblyReferencesOtherAssembly()
    {
      var assembly1 = typeof(RecordedAssertionsSpecification).Assembly;
      new Action(() =>
        assembly1.Should().NotReferenceAssemblyWith(typeof(FactAttribute)))
        .Should().ThrowExactly<XunitException>();
    }


    [Fact]
    public void ShouldFailNonPublicEventsAssertionWhenAssemblyContainsAtLeastOneNonPublicEvent()
    {
      const string eventName = "ExplicitlyImplementedEvent";
      var assembly = typeof(RecordedAssertionsSpecification).Assembly;
      assembly.Should().DefineType("TddXt.XFluentAssert.EndToEndSpecification", nameof(ExplicitImplementation));
      assembly.Should().DefineType("TddXt.XFluentAssert.EndToEndSpecification", nameof(IExplicitlyImplemented));
      typeof(IExplicitlyImplemented).Should().HaveEventWithShortName(eventName);
      typeof(ExplicitImplementation).Should().HaveEventWithShortName(eventName);

      new Action(() => assembly.Should().NotHaveHiddenEvents()).Should().ThrowExactly<XunitException>()
        .Which.Message.Should().NotContain(eventName);
    }

    [Fact]
    public void ShouldFailConstructorLimitAssertionWhenAnyClassContainsAtLeastOneConstructor()
    {
      typeof(ObjectWithTwoConstructors).Should().HaveConstructor(new[] { typeof(int) });
      typeof(ObjectWithTwoConstructors).Should().HaveConstructor(new[] { typeof(string) });
      var assembly = typeof(RecordedAssertionsSpecification).Assembly;

      new Action(() => assembly.Should().HaveOnlyTypesWithSingleConstructor()).Should().ThrowExactly<XunitException>().Which
        .Message.Should().NotContain("MyException");
    }

    [Fact]
    public void ShouldSupportEmptyMaybe()
    {
      new ObjectWithMaybe().Invoking(o => o.Should().DependOn(Maybe<string>.Nothing))
        .Should().Throw<XunitException>();
    }

    public class Lol2
    {
#pragma warning disable 169
      public static int Gieniek = 123;
#pragma warning restore 169
    }

#pragma warning disable 67
    protected event AnyEventHandler Changed;
#pragma warning restore 67


#pragma warning disable 169
    private static int _lolek = 12;
#pragma warning restore 169

    public static int StaticProperty
    {
      get;
      set;
    }

  }

  public delegate void AnyEventHandler(object sender, IAnyEventHandlerArgs args);

  public interface IAnyEventHandlerArgs
  {
  }

  public class ObjectWithTwoConstructors
  {
    public ObjectWithTwoConstructors(int x)
    {

    }

    public ObjectWithTwoConstructors(string x)
    {

    }
  }

  public class MyException : Exception
  {

  }

  public class ObjectWithMaybe
  {
    public Maybe<string> M1;
    public Maybe<string> M2 = "".ToMaybe();
  }

  public interface IExplicitlyImplemented
  {
    event AnyEventHandler ExplicitlyImplementedEvent;
  }

  public class ExplicitImplementation : IExplicitlyImplemented
  {
    event AnyEventHandler IExplicitlyImplemented.ExplicitlyImplementedEvent
    {
      add { throw new NotImplementedException(); }
      remove { throw new NotImplementedException(); }
    }
  }
}

