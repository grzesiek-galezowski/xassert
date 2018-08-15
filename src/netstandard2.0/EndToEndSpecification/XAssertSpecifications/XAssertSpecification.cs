namespace TddEbook.TddToolkitSpecification.XAssertSpecifications
{
  using System;
  using System.Diagnostics.CodeAnalysis;

  using FluentAssertions;

  using TddXt.AnyRoot.Strings;
  using TddXt.XAssert.CommonTypes;
  using TddXt.XAssert.TddEbook.TddToolkit;
  using TddXt.XAssert.TddEbook.TddToolkit.ValueAssertions;
  using TddXt.XAssert.TypeReflection.ImplementationDetails;

  using Xunit;
  using Xunit.Sdk;

  using static TddXt.AnyRoot.Root;

  public class XAssertSpecification
  {

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
    public void ShouldPassValueTypeAssertionForProperValueTypeWithInternalConstructor()
    {
      typeof(FileExtension).Should().HaveValueSemantics();
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
      new Action(() => typeof(ProperValueTypeWithoutEqualityOperator).Should().HaveValueSemantics()).Should().ThrowExactly<XunitException>();
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
      new Action(() => s.ToLower().Should().BeUppercase() ).Should().ThrowExactly<XunitException>();
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
      new Action(() => char.ToLower(c).Should().BeUppercase()).Should().ThrowExactly<XunitException>();
      new Action(() => char.ToUpper(c).Should().BeUppercase()).Should().NotThrow();
    }

    [Fact]
    public void ShouldFailLowerCaseAssertionOnUpperCaseCharAndPassOnLowerCaseChar()
    {
      var c = Any.AlphaChar();
      new Action(() => char.ToUpper(c).Should().BeLowercase()).Should().ThrowExactly<XunitException>();
      new Action(() => char.ToLower(c).Should().BeLowercase()).Should().NotThrow();
    }

    [Fact]
    public void ShouldAllowToSkipSomePropertiesWhenComparingLikeness()
    {
      var tp1 = new TwoProp()
      {
        X1 = 123,
        X2 = 345,
        X3 = 999,
        x4 = 123
      };
      var tp2 = new TwoProp();
      tp2.X1 = 123;
      tp2.X2 = 346;
      tp2.X3 = 346;
      tp2.x4 = 346;

      tp1.Should().BeLike(tp2, tp => tp.X2, tp => tp.X3, tp => tp.x4);

      tp1.X1 = 0;

      tp1.Should().NotBeLike(tp2, tp => tp.X2, tp => tp.X3, tp => tp.x4);
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


  }

  class TypeWithStaticField
  {
    private static readonly int a = 1;
  }
  class TypeWithNoStaticField
  {
    private readonly int a = 1;
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
    public const int Val3 = Val2+1;
  }

  [SuppressMessage("ReSharper", "UnusedMember.Global")]
  public class ConstsWithRepeatingValues
  {
    public const int Val1 = 0;
    public const int Val2 = Val1 + 1;
    public const int Val3 = Val1;
  }


}





