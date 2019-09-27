using TddXt.XFluentAssertRoot;
using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Types;
using TddXt.XFluentAssert.TypeReflection;
using TddXt.XFluentAssert.TypeReflection.Interfaces;
using Xunit;

namespace TddXt.XFluentAssert.EndToEndSpecification
{
  using System;
  using FluentAssertions;

  using NSubstitute;

  using AnyRoot;
  using AnyRoot.Strings;
  using AssertionConstraints;
  using Root;

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


    [Fact]
    public void ShouldFailNonPublicEventsAssertionWhenAssemblyContainsAtLeastOneNonPublicEvent()
    {
      const string EventName = "explicitlyImplementedEvent";
      var assembly = typeof (RecordedAssertionsSpecification).Assembly;
      assembly.Should().DefineType("TddXt.XFluentAssert.EndToEndSpecification", nameof(ExplicitImplementation));
      assembly.Should().DefineType("TddXt.XFluentAssert.EndToEndSpecification", nameof(ExplicitlyImplemented));
      typeof(ExplicitlyImplemented).Should().HaveEventWithShortName(EventName);
      typeof(ExplicitImplementation).Should().HaveEventWithShortName(EventName);

      new Action(() => assembly.Should().NotHaveHiddenEvents()).Should().ThrowExactly<XunitException>()
        .Which.Message.Should().NotContain(EventName);
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

public class NewEqualityAssertionSpecification
{
    [Fact]
    public void ShouldXXXXX()
    {
        typeof(MyIntWrapper)
            .Should().HaveCorrectEquality(
                EqualityArg.For(() => 1, () => 2)
                );
    }

}

public static class TypeAssertionsExtensions
{
    public static AndConstraint<TypeAssertions> HaveCorrectEquality(this TypeAssertions assertions,
        params EqualityArg[] equalityArgs)
    {
        var smartType = SmartType.For(assertions.Subject);
        var constructor = smartType.PickConstructorWithLeastNonPointersParameters();

        constructor.Value().InvokeWithExample1ParamsOnly(equalityArgs)
          .Should().Be(
            constructor.Value().InvokeWithExample1ParamsOnly(equalityArgs));

        //TODO compare in reverse (i.e. instance1.Equals(instance1))
        for (int i = 0; i < constructor.Value().GetParametersCount(); ++i)
        {
            var instance1 = constructor.Value().InvokeWithExample1ParamsOnly(equalityArgs);
            var instance2 = constructor.Value().InvokeWithExample2ParamFor(i, equalityArgs);
            instance1.Should().NotBe(instance2);
            //bug test equality
        }
        for (int i = 0; i < constructor.Value().GetParametersCount(); ++i)
        {
            var instance1 = constructor.Value().InvokeWithExample2ParamFor(i, equalityArgs);
            var instance2 = constructor.Value().InvokeWithExample1ParamsOnly(equalityArgs);
            instance1.Should().NotBe(instance2);
            //bug test equality
        }

        //bug types without constructors or with invisible constructors?
            return new AndConstraint<TypeAssertions>(assertions);
    }
}

internal class MyIntWrapper
{
  private readonly int _a;
  private readonly int _b;

  public MyIntWrapper(int a, int b)
  {
    _a = a;
    _b = b;
  }

  public override string ToString()
  {
    return $"{nameof(_a)}: {_a}, {nameof(_b)}: {_b}";
  }
}