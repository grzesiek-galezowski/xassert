using AtmaFileSystem;
using TddXt.XFluentAssert.Api;
using TddXt.XFluentAssert.Api.ValueAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using NullableReferenceTypesExtensions;
using TddXt.AnyRoot.Strings;
using TddXt.TypeReflection;
using TddXt.XFluentAssert.CommonTypes;
using TddXt.XFluentAssert.TypeReflection.ImplementationDetails;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using static TddXt.AnyRoot.Root;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications
{
  public class XAssertSpecification
  {
    private readonly ITestOutputHelper _output;

    public XAssertSpecification(ITestOutputHelper output)
    {
      _output = output;
    }

    [Fact]
    public void ShouldThrowAssertionExceptionWhenTypeIsNotGuardedAgainstNullConstructorParameters()
    {
      new Action(() => typeof(NotGuardedObject).Should().HaveNullProtectedConstructors())
        .Should().ThrowExactly<XunitException>()
        .Which.Message.Should().ContainAll("Not guarded parameter: String b", "Not guarded parameter: String dede")
        .And.NotContain("Not guarded parameter: Int32 a");
    }

    [Fact]
    public void ShouldNotThrowAssertionExceptionWhenTypeIsGuardedAgainstNullConstructorParameters()
    {
      new Action(() => typeof(GuardedObject).Should().HaveNullProtectedConstructors())
        .Should().NotThrow();
    }

    [Fact]
    public void ShouldPassValueTypeAssertionForProperValueType()
    {
      typeof(ProperValueType).Should().HaveValueSemantics();
    }

    [Fact]
    public void ShouldPassValueTypeAssertionForProperValueTypeDerivedFromValueLibrary()
    {
      typeof(ProperValueTypeDerivedFromLibrary).Should().HaveValueSemantics();
    }

    [Fact]
    public void ShouldPassValueTypeAssertionForProperValueTypeWithInternalConstructor()
    {
      typeof(FileExtension).Should().HaveValueSemantics();
    }
    [Fact]
    public void ShouldPassValueTypeAssertionForProperValueTypeWithNonGenericSuperclass()
    {
      typeof(RelativeFilePath).Should().HaveValueSemantics();
    }

    [Fact]
    public void ShouldPreferInternalNonRecursiveConstructorsToPublicRecursiveOnes()
    {
      new Action(() => Any.Instance<DirectoryPath>()).Should().NotThrow();
      new Action(() => typeof(DirectoryPath).Should().HaveValueSemantics()).Should().NotThrow();
    }

    [Fact]
    public void ShouldBeAbleToChooseInternalConstructorWhenThereisNoPublicOne()
    {
      new Action(() => Any.Instance<FileNameWithoutExtension>()).Should().NotThrow();
      new Action(() => typeof(FileNameWithoutExtension).Should().HaveValueSemantics()).Should().NotThrow();
    }

    [Fact]
    public void ShouldAllowSpecifyingConstructorArgumentsNotTakenIntoAccountDuringValueBehaviorCheck()
    {
      typeof(ProperValueTypeWithOneArgumentIdentity)
        .Should().HaveValueSemantics(ValueTypeTraits.Custom.SkipConstructorArgument(0));

      new Action(() => typeof(ProperValueTypeWithOneArgumentIdentity).Should().HaveValueSemantics()).Should().ThrowExactly<XunitException>();
    }

    [Fact]
    public void ShouldAcceptProperFullValueTypesAndRejectBadOnes()
    {
      typeof(ProperValueType).Should().HaveValueSemantics();
      new Action(() => typeof(ProperValueTypeWithoutEqualityOperator).Should().HaveValueSemantics()).Should().ThrowExactly<XunitException>()
        .Which.Message.Should().Contain("equality operator");
    }

    [Fact]
    public void ShouldWorkForStructuresWithDefaultEquality()
    {
      typeof(Maybe<string>).Should().HaveValueSemantics();
    }

    [Fact]
    public void ShouldWorkForPrimitves()
    {
      typeof(int).Should().HaveValueSemantics();
    }

    [Fact]
    public void ShouldFailUpperCaseAssertionOnLowerCaseStringAndPassOnUpperCaseString()
    {
      var s = Any.String();
      new Action(() => s.ToLower().Should().BeUppercase()).Should().ThrowExactly<XunitException>();
      new Action(() => s.ToUpper().Should().BeUppercase()).Should().NotThrow();
    }

    [Fact]
    public void ShouldFailLowerCaseAssertionOnUpperCaseStringAndPassOnLowerCaseString()
    {
      var s = Any.String();
      new Action(() => s.ToUpper().Should().BeLowercase()).Should().ThrowExactly<XunitException>();
      new Action(() => s.ToLower().Should().BeLowercase()).Should().NotThrow();
    }

    [Fact]
    public void ShouldFailUpperCaseAssertionOnLowerCaseCharAndPassOnUpperCaseChar()
    {
      var c = Any.AlphaChar();
      new Action(() => FluentAssertionsCharExtensions.Should(char.ToLower(c)).BeUppercase()).Should().ThrowExactly<XunitException>();
      new Action(() => FluentAssertionsCharExtensions.Should(char.ToUpper(c)).BeUppercase()).Should().NotThrow();
    }

    [Fact]
    public void ShouldFailLowerCaseAssertionOnUpperCaseCharAndPassOnLowerCaseChar()
    {
      var c = Any.AlphaChar();
      new Action(() => FluentAssertionsCharExtensions.Should(char.ToUpper(c)).BeLowercase()).Should().ThrowExactly<XunitException>();
      new Action(() => FluentAssertionsCharExtensions.Should(char.ToLower(c)).BeLowercase()).Should().NotThrow();
    }

    [Fact]
    public void ShouldAllowToSkipSomePropertiesWhenComparingLikeness()
    {
      var tp1 = new TwoProp()
      {
        X1 = 123,
        X2 = 345,
        X3 = 999,
        X4 = 123
      };
      var tp2 = new TwoProp();
      tp2.X1 = 123;
      tp2.X2 = 346;
      tp2.X3 = 346;
      tp2.X4 = 346;

      tp1.Should().BeLike(tp2, tp => tp.X2, tp => tp.X3, tp => tp.X4);

      tp1.X1 = 0;

      tp1.Should().NotBeLike(tp2, tp => tp.X2, tp => tp.X3, tp => tp.X4);
    }

    [Fact]
    public void ShouldAllowComparingLikenessOfGenericTypesEgCollections()
    {
      new List<int> { 1, 2, 3 }.Should().BeLike(new List<int> { 1, 2, 3 });
      new Action(() => new List<int> { 1, 2, 3 }.Should().NotBeLike(new List<int> { 1, 2, 3 }))
        .Should().ThrowExactly<XunitException>();

      new List<int> { 1, 2, 3 }.Should().NotBeLike(new List<int> { 1, 2, 4 });
      new Action(() => new List<int> { 1, 2, 3 }.Should().BeLike(new List<int> { 1, 2, 4 }))
        .Should().ThrowExactly<XunitException>();
    }

    [Fact]
    public void AllowAssertingWhetherConstClassHasUniqueValues()
    {
      new Action(() => typeof(ConstsWithUniqueValues).Should().HaveUniqueConstants())
        .Should().NotThrow();

      new Action(() => typeof(ConstsWithRepeatingValues).Should().HaveUniqueConstants())
        .Should().ThrowExactly<DuplicateConstantException>()
        .WithMessage("Val1 <0> is a duplicate of Val3 <0>");
    }

    [Fact]
    public void ShouldFailNoStaticFieldsAssertionWhenTypeHasStaticFields()
    {
      new Action(() =>
          typeof(TypeWithStaticField).Should().NotHaveStaticFields())
        .Should().Throw<Exception>();
    }

    [Fact]
    public void ShouldPassNoStaticFieldsAssertionWhenTypeHasStaticFields()
    {
      new Action(() =>
          typeof(TypeWithNoStaticField).Should().NotHaveStaticFields())
        .Should().NotThrow<Exception>();
    }

    [Fact]
    public void ShouldPassUppercaseAssertionOnUppercaseString()
    {
      Any.UpperCaseString().Should().BeUppercase();
    }

    [Fact]
    public void ShouldFailUppercaseAssertionOnLowercaseString()
    {

      new Action(() => (Any.UpperCaseString() + "a").Should().BeUppercase())
        .Should().ThrowExactly<XunitException>();
    }
    [Fact]
    public void ShouldPassLowercaseAssertionOnLowercaseString()
    {
      Any.LowerCaseString().Should().BeLowercase();
    }

    [Fact]
    public void ShouldFailLowercaseAssertionOnUppercaseString()
    {

      new Action(() => (Any.LowerCaseString() + "A").Should().BeLowercase())
        .Should().ThrowExactly<XunitException>();
    }


    // todo circular dependencies

  }

  public class PlaygroundForNewValueAssertions //bug
  {
    [Fact]
    public void Lol() //bug
    {
      AssertIsProperValueObject<ProperValueType>(
        new Func<ProperValueType>[]
        {
          () => new ProperValueType(1, new[] {1,2,3})
        },
        new Func<ProperValueType>[]
        {
          () => new ProperValueType(2, new[] {1,2,3}),
          () => new ProperValueType(1, new[] {1,2}),
          () => new ProperValueType(1, null)
        });
    }

    private void AssertIsProperValueObject<T>(Func<T>[] equalInstances, Func<T>[] otherInstances)
    {
      //equal to itself
      var instances = equalInstances.Select(i => i());
      foreach (var instance in instances)
      {
        instance.Should().Be(instance);
      }

      //bug instances equal to themselves
      //bug what about nulls in constructors?

      //equal to other equals
      foreach (var instanceFactory1 in equalInstances)
      {
        instanceFactory1().Equals(null).Should().BeFalse();
        foreach (var instanceFactory2 in equalInstances)
        {
          instanceFactory1().Equals(instanceFactory2()).Should().BeTrue(); //bug message
          instanceFactory2().Equals(instanceFactory1()).Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory1(), instanceFactory2())
            .Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory2(), instanceFactory1())
            .Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory1(), instanceFactory2())
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory2(), instanceFactory1())
            .Should().BeFalse(); //bug message
          instanceFactory1().GetHashCode().Should().Be(instanceFactory2().GetHashCode()); //bug message
        }
      }

      //bug check for empty arrays
      //bug check for nulls

      foreach (var instanceFactory1 in equalInstances)
      {
        foreach (var instanceFactory2 in otherInstances)
        {
          instanceFactory1().Equals(instanceFactory2()).Should().BeFalse(); //bug message
          instanceFactory2().Equals(instanceFactory1()).Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory1(), instanceFactory2())
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory2(), instanceFactory1())
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory1(), instanceFactory2())
            .Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory2(), instanceFactory1())
            .Should().BeTrue(); //bug message
          instanceFactory1().GetHashCode().Should().NotBe(instanceFactory2().GetHashCode()); //bug message
        }
      }
    }
  }


  class A1
  {
    A2 _a2;
    B2 _b2;

    public A1()
    {
      _a2 = new A2();
      _b2 = new B2();
    }

    public A1(A2 a2, B2 b2)
    {
      _a2 = a2;
      _b2 = b2;
    }
  }

  class B2
  {
    B3 _b3;

    public B2()
    {
      _b3 = new B3();
    }

    public B2(B3 b3)
    {
      _b3 = b3;
    }
  }

  class A2
  {
    private A3 _a3;
    private B3 _b3;
    private string _str;
    private int _num;

    public A2()
    {
      _a3 = new A3();
      _b3 = new B3();
      _str = ";p;";
      _num = 123;
    }

    public A2(A3 a3, B3 b3, string s, int i)
    {
      _a3 = a3;
      _b3 = b3;
      _str = s;
      _num = i;
    }
  }

  class B3
  {

  }

  class A3
  {

  }


  class TypeWithStaticField
  {
    private static readonly int A = 1;
  }
  class TypeWithNoStaticField
  {
    private readonly int _a = 1;
  }

  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public enum EnumWithUniqueValues
  {
    Val1,
    Val2,
    Val3,
  }

  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public enum EnumWithRepeatingValues
  {
    Val1,
    Val2,
    Val3 = Val1,
  }

  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public class ConstsWithUniqueValues
  {
    public const int Val1 = 0;
    public const int Val2 = Val1 + 1;
    public const int Val3 = Val2 + 1;
  }

  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public class ConstsWithRepeatingValues
  {
    public const int Val1 = 0;
    public const int Val2 = Val1 + 1;
    public const int Val3 = Val1;
  }


}





