﻿using System;
using System.Reflection;

using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

using TddXt.AnyRoot.Strings;
using static TddXt.AnyRoot.Root;

namespace TddEbook.TddToolkitSpecification
{
  using TddXt.XAssert.AssertionConstraints;
  using TddXt.XAssert.TddEbook.TddToolkit;

  class RecordedAssertionsSpecification
  {
    [Test]
    public void ShouldAddErrorMessageWhenTruthAssertionFails()
    {
      //GIVEN
      var violations = Substitute.For<IConstraintsViolations>();
      var message = Any.String();

      //WHEN
      RecordedAssertions.True(false, message, violations);

      //THEN
      violations.Received(1).Add(message);
    }

    [Test]
    public void ShouldNotAddErrorMessageWhenTruthAssertionPasses()
    {
      //GIVEN
      var violations = Substitute.For<IConstraintsViolations>();
      var message = Any.String();

      //WHEN
      RecordedAssertions.True(true, message, violations);

      //THEN
      violations.DidNotReceive().Add(message);
    }


    [Test]
    public void ShouldFailStaticFieldsAssertionIfAssemblyContainsAtLeastOneStaticField()
    {
      var assembly = typeof (RecordedAssertionsSpecification).Assembly;
      
      var e = Assert.Throws<AssertionException>(() => assembly.Should().NotHaveStaticFields());
      StringAssert.Contains(nameof(_lolek), e.Message);
      StringAssert.Contains(nameof(Lol2._gieniek), e.Message);
      StringAssert.Contains(nameof(StaticProperty), e.Message);
    }

    [Test]
    public void ShouldFailReferenceAssertionWhenAssemblyReferencesOtherAssembly()
    {
      var assembly1 = typeof(RecordedAssertionsSpecification).Assembly;
      Assert.Throws<AssertionException>(() => 
        assembly1.Should().NotReferenceAssemblyWith(typeof(TestAttribute)));
    }


    [Test] //TODO 1. only single constructor! 2. class inheritance levels
    public void ShouldFailNonPublicEventsAssertionWhenAssemblyContainsAtLeastOneNonPublicEvent()
    {
      var assembly = typeof (RecordedAssertionsSpecification).Assembly;
      var ex = Assert.Throws<AssertionException>(() => assembly.Should().NotHaveHiddenEvents());
      StringAssert.DoesNotContain("explicitlyImplementedEvent", ex.Message);
    }

    [Test] //TODO 1. only single constructor! 2. class inheritance levels, 3. private/protected events
    public void ShouldFailConstructorLimitAssertionWhenAnyClassContainsAtLeastOneConstructor()
    {
      var assembly = typeof (RecordedAssertionsSpecification).Assembly;
      var exception = Assert.Throws<AssertionException>(() => assembly.Should().HaveOnlyTypesWithSingleConstructor());
      StringAssert.DoesNotContain("MyException", exception.Message);
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