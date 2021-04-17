using AtmaFileSystem;
using TddXt.XFluentAssert.Api;
using TddXt.XFluentAssert.Api.ValueAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Functional.Maybe;
using Functional.Maybe.Just;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Numbers;
using TddXt.AnyRoot.Strings;
using TddXt.TypeReflection;
using TddXt.XFluentAssert.EqualityAssertions;
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
    public void ShouldPassValueTypeAssertionForProperValueType()
    {
      var integer = Any.Integer();
      var anArray = Any.Array<int>();
      typeof(ProperValueType).Should().HaveValueSemantics(
        new Func<ProperValueType>[]
        {
          () => new(integer, anArray)
        },
        new Func<ProperValueType>[]
        {
          () => new(Any.OtherThan(integer), anArray),
          () => new(integer, Any.OtherThan<int[]>(anArray))
        });
    }

    [Fact]
    public void ShouldPassValueTypeAssertionForProperValueTypeDerivedFromValueLibrary()
    {
      var integer = Any.Integer();
      var str = Any.String();
      typeof(ProperValueTypeDerivedFromLibrary).Should().HaveValueSemantics(
        new Func<ProperValueTypeDerivedFromLibrary>[]
        {
          () => new(integer, str)
        },
        new Func<ProperValueTypeDerivedFromLibrary>[]
        {
          () => new(Any.OtherThan(integer), str),
          () => new(integer, Any.OtherThan(str))
        });
    }

    [Fact]
    public void ShouldPassValueTypeAssertionForProperValueTypeWithInternalConstructor()
    {
      var str = Any.String();
      typeof(FileExtension).Should().HaveValueSemantics(
        new Func<FileExtension>[]
        {
          () => new(str)
        },
        new Func<FileExtension>[]
        {
          () => new(Any.OtherThan(str))
        });
    }
    [Fact]
    public void ShouldPassValueTypeAssertionForProperValueTypeWithNonGenericSuperclass()
    {
      var str = Any.String();
      typeof(RelativeFilePath).Should().HaveValueSemantics(
        new Func<RelativeFilePath>[]
        {
          () => RelativeFilePath.Value(str)
        },
        new Func<RelativeFilePath>[]
        {
          () => RelativeFilePath.Value(Any.OtherThan(str))
        });
    }

    [Fact]
    public void ShouldAllowSpecifyingConstructorArgumentsNotTakenIntoAccountDuringValueBehaviorCheck()
    {
      var enumerable = Any.Enumerable<int>();
      var enumerable2 = Any.Enumerable<int>();
      var integer = Any.Integer();
      typeof(ProperValueTypeWithOneArgumentIdentity)
        .Should().HaveValueSemantics(
          new Func<ProperValueTypeWithOneArgumentIdentity>[]
          {
            () => new(enumerable, integer),
            () => new(enumerable2, integer),
          },
          new Func<ProperValueTypeWithOneArgumentIdentity>[]
          {
            () => new(enumerable, Any.OtherThan(integer)),
            () => new(enumerable2, Any.OtherThan(integer)),
          });
    }

    [Fact]
    public void ShouldAcceptProperFullValueTypesAndRejectBadOnes()
    {
      int integer = Any.Integer();
      new Action(() => typeof(ProperValueTypeWithoutEqualityOperator).Should().HaveValueSemantics(
        new Func<ProperValueTypeWithoutEqualityOperator>[]
        {
          () => new(integer)
        },
        new Func<ProperValueTypeWithoutEqualityOperator>[]
        {
          () => new(Any.OtherThan(integer))
        })).Should().ThrowExactly<XunitException>()
        .Which.Message.Should().Contain("equality operator");
    }

    [Fact]
    public void ShouldWorkForStructuresWithDefaultEquality()
    {
      var str = Any.String();
      typeof(Maybe<string>).Should().HaveValueSemantics(
        new Func<Maybe<string>>[]
        {
          () => str.Just()
        },
        new Func<Maybe<string>>[]
        {
          () => Maybe<string>.Nothing, 
          () => Any.OtherThan(str).Just(), 
        });
    }

    [Fact]
    public void ShouldWorkForPrimitves()
    {
      typeof(int).Should().HaveValueSemantics(
        new Func<int>[]
        {
          () => 1
        },
        new Func<int>[]
        {
          () => 2, 
          () => 3, 
        });
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





