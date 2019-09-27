using TddXt.XFluentAssertRoot;
using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Types;
using TddXt.XFluentAssert.TypeReflection;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

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

public static class TypeAssertionsExtensions
{
  public static AndConstraint<TypeAssertions> HaveCorrectEquality(this TypeAssertions assertions,
    params EqualityArg[] equalityArgs)
  {
    var smartType = SmartType.For(assertions.Subject);
    var constructor = smartType.PickConstructorWithLeastNonPointersParameters();

    var instance = constructor.Value().InvokeWithExample1ParamsOnly(equalityArgs);
    var equalInstance = constructor.Value().InvokeWithExample1ParamsOnly(equalityArgs);

    if (instance.Equals(null))
    {
      throw new Exception( /* TODO */);
    }
    if (instance.Equals(constructor))
    {
      throw new Exception( /* TODO */);
    }
    if (!instance.Equals(instance))
    {
      throw new Exception();
    }
    if (!instance.Equals(equalInstance))
    {
      throw new Exception();
    }
    if (!equalInstance.Equals(instance))
    {
      throw new Exception();
    }
    if (!(bool)smartType.EqualityOperator().Evaluate(instance, instance))
    {
      throw new Exception();
    }
    if ((bool)smartType.EqualityOperator().Evaluate(instance, null))
    {
      throw new Exception();
    }
    if (!(bool)smartType.EqualityOperator().Evaluate(equalInstance, instance))
    {
      throw new Exception();
    }
    if (!(bool)smartType.EqualityOperator().Evaluate(instance, equalInstance))
    {
      throw new Exception();
    }
    if ((bool)smartType.InequalityOperator().Evaluate(instance, instance))
    {
      throw new Exception();
    }
    if (!(bool)smartType.InequalityOperator().Evaluate(instance, null))
    {
      throw new Exception();
    }
    if ((bool)smartType.InequalityOperator().Evaluate(instance, equalInstance))
    {
      throw new Exception();
    }
    if ((bool)smartType.InequalityOperator().Evaluate(equalInstance, instance))
    {
      throw new Exception();
    }
    if (instance.GetHashCode() != equalInstance.GetHashCode())
    {
      throw new Exception();
    }
    if (instance.GetHashCode() != instance.GetHashCode())
    {
      throw new Exception();
    }

    //TODO compare in reverse (i.e. instance1.Equals(instance1))
    for (int i = 0; i < constructor.Value().GetParametersCount(); ++i)
    {
      var instance2 = constructor.Value().InvokeWithExample2ParamFor(i, equalityArgs);

      if (instance.Equals(instance2))
      {
        throw new Exception( /* TODO */);
      }
      if (instance2.Equals(instance))
      {
        throw new Exception( /* TODO */);
      }
      if ((bool)smartType.EqualityOperator().Evaluate(instance, instance2))
      {
        throw new Exception();
      }
      if ((bool)smartType.EqualityOperator().Evaluate(instance2, instance))
      {
        throw new Exception();
      }
      if (!(bool)smartType.InequalityOperator().Evaluate(instance, instance2))
      {
        throw new Exception();
      }
      if (!(bool)smartType.InequalityOperator().Evaluate(instance2, instance))
      {
        throw new Exception();
      }
      if (instance.GetHashCode().Equals(instance2.GetHashCode()))
      {
        throw new Exception();
      }

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

internal class MyIntWrapper : IEquatable<MyIntWrapper>
{
  private readonly int _a;
  private readonly int _b;

  public MyIntWrapper(int a, int b)
  {
    _a = a;
    _b = b;
  }

  public bool Equals(MyIntWrapper other)
  {
    if (ReferenceEquals(null, other))
    {
      return false;
    }

    if (ReferenceEquals(this, other))
    {
      return true;
    }
    return _a == other._a && _b == other._b;
  }

  public override bool Equals(object obj)
  {
    if (ReferenceEquals(null, obj))
    {
      return false;
    }

    if (ReferenceEquals(this, obj))
    {
      return true;
    }
    if (obj.GetType() != this.GetType()) 
    {
      return false;
    }
    return Equals((MyIntWrapper) obj);
  }

  public override int GetHashCode()
  {
    unchecked
    {
      return (_a * 397) ^ _b;
    }
  }

  public static bool operator ==(MyIntWrapper left, MyIntWrapper right)
  {
    return Equals(left, right);
  }

  public static bool operator !=(MyIntWrapper left, MyIntWrapper right)
  {
    return !Equals(left, right);
  }

  public override string ToString()
  {
    return $"{nameof(_a)}: {_a}, {nameof(_b)}: {_b}";
  }
}