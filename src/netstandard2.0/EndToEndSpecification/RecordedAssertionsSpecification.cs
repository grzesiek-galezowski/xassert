namespace TddXt.XFluentAssert.EndToEndSpecification
{
  using System;

  using FluentAssertions;

  using NSubstitute;

  using TddXt.AnyRoot;
  using TddXt.AnyRoot.Strings;
  using TddXt.XFluentAssert.AssertionConstraints;
  using TddXt.XFluentAssert.Root;

  using Xunit;
  using Xunit.Sdk;

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
        .Which.Message.Should().ContainAll(nameof(_lolek), nameof(Lol2._gieniek), nameof(StaticProperty));
    }

    [Fact]
    public void ShouldFailReferenceAssertionWhenAssemblyReferencesOtherAssembly()
    {
      var assembly1 = typeof(RecordedAssertionsSpecification).Assembly;
      new Action(() => 
        assembly1.Should().NotReferenceAssemblyWith(typeof(FactAttribute)))
        .Should().ThrowExactly<XunitException>();
    }


    [Fact] //TODO 1. only single constructor! 2. class inheritance levels
    public void ShouldFailNonPublicEventsAssertionWhenAssemblyContainsAtLeastOneNonPublicEvent()
    {
      var assembly = typeof (RecordedAssertionsSpecification).Assembly;
      new Action(() => assembly.Should().NotHaveHiddenEvents()).Should().ThrowExactly<XunitException>()
        .Which.Message.Should().NotContain("explicitlyImplementedEvent");
    }

    [Fact] //TODO 1. only single constructor! 2. class inheritance levels, 3. private/protected events
    public void ShouldFailConstructorLimitAssertionWhenAnyClassContainsAtLeastOneConstructor()
    {
      var assembly = typeof (RecordedAssertionsSpecification).Assembly;
      new Action(() => assembly.Should().HaveOnlyTypesWithSingleConstructor()).Should().ThrowExactly<XunitException>().Which
        .Message.Should().NotContain("MyException");
    }



    public class Lol2
    {
#pragma warning disable 169
      public static int _gieniek = 123;
#pragma warning restore 169
    }

#pragma warning disable 67
    protected event AnyEventHandler _changed;
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

  public delegate void AnyEventHandler(object sender, AnyEventHandlerArgs args);

  public interface AnyEventHandlerArgs
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

  public interface ExplicitlyImplemented
  {
    event AnyEventHandler explicitlyImplementedEvent;    
  }

  public class ExplicitImplementation : ExplicitlyImplemented
  {
    event AnyEventHandler ExplicitlyImplemented.explicitlyImplementedEvent
    {
      add { throw new NotImplementedException(); }
      remove { throw new NotImplementedException(); }
    }
  }
}
