namespace TddEbook.TddToolkitSpecification.XAssertSpecifications
{
  using System;
  using System.Diagnostics.CodeAnalysis;

  using FluentAssertions;

  using NUnit.Framework;

  using TddEbook.TddToolkit;

  using TddXt.AnyRoot.Strings;
  using TddXt.XAssert.CommonTypes;
  using TddXt.XAssert.TypeReflection.ImplementationDetails;

  using static TddXt.AnyRoot.Root;

  public class XAssertSpecification
  {

    [Test]
    public void ShouldThrowAssertionExceptionWhenTypeIsNotGuardedAgainstNullConstructorParameters()
    {
      var exception = Assert.Throws<AssertionException>(() => typeof(NotGuardedObject).Should().HaveNullProtectedConstructors());
      StringAssert.Contains("Not guarded parameter: String b", exception.Message);
      StringAssert.Contains("Not guarded parameter: String dede", exception.Message);
      StringAssert.DoesNotContain("Not guarded parameter: Int32 a", exception.Message);
    }

    [Test]
    public void ShouldNotThrowAssertionExceptionWhenTypeIsGuardedAgainstNullConstructorParameters()
    {
      Assert.DoesNotThrow(() => typeof(GuardedObject).Should().HaveNullProtectedConstructors());
    }

    [Test]
    public void ShouldPassValueTypeAssertionForProperValueType()
    {
      typeof(ProperValueType).Should().HaveValueSemantics();
    }

    [Test]
    public void ShouldPassValueTypeAssertionForProperValueTypeWithInternalConstructor()
    {
      typeof(FileExtension).Should().HaveValueSemantics();
    }

    [Test]
    public void ShouldPreferInternalNonRecursiveConstructorsToPublicRecursiveOnes()
    {
      Assert.DoesNotThrow(() => Any.Instance<DirectoryPath>());
      Assert.DoesNotThrow(() => typeof(DirectoryPath).Should().HaveValueSemantics());
    }

    [Test]
    public void ShouldBeAbleToChooseInternalConstructorWhenThereisNoPublicOne()
    {
      Assert.DoesNotThrow(() => Any.Instance<FileNameWithoutExtension>());
      Assert.DoesNotThrow(() => typeof(FileNameWithoutExtension).Should().HaveValueSemantics());
    }

    [Test]
    public void ShouldAllowSpecifyingConstructorArgumentsNotTakenIntoAccountDuringValueBehaviorCheck()
    {
      typeof(ProperValueTypeWithOneArgumentIdentity)
        .Should().HaveValueSemantics(ValueTypeTraits.Custom.SkipConstructorArgument(0));

      Assert.Throws<AssertionException>(() => typeof(ProperValueTypeWithOneArgumentIdentity).Should().HaveValueSemantics());
    }

    [Test]
    public void ShouldAcceptProperFullValueTypesAndRejectBadOnes()
    {
      typeof(ProperValueType).Should().HaveValueSemantics();
      Assert.Throws<AssertionException>(() => typeof(ProperValueTypeWithoutEqualityOperator).Should().HaveValueSemantics());
    }

    [Test]
    public void ShouldWorkForStructuresWithDefaultEquality()
    {
      typeof(Maybe<string>).Should().HaveValueSemantics();
    }

    [Test]
    public void ShouldWorkForPrimitves()
    {
      typeof(int).Should().HaveValueSemantics();
    }

    [Test]
    public void ShouldFailUpperCaseAssertionOnLowerCaseStringAndPassOnUpperCaseString()
    {
      var s = Any.String();
      Assert.Throws<AssertionException>(() => s.ToLower().Should().BeUppercase() );
      Assert.DoesNotThrow(() => s.ToUpper().Should().BeUppercase());
    }

    [Test]
    public void ShouldFailLowerCaseAssertionOnUpperCaseStringAndPassOnLowerCaseString()
    {
      var s = Any.String();
      Assert.Throws<AssertionException>(() => s.ToUpper().Should().BeLowercase());
      Assert.DoesNotThrow(() => s.ToLower().Should().BeLowercase());
    }

    [Test]
    public void ShouldFailUpperCaseAssertionOnLowerCaseCharAndPassOnUpperCaseChar()
    {
      var c = Any.AlphaChar();
      Assert.Throws<AssertionException>(() => char.ToLower(c).Should().BeUppercase());
      Assert.DoesNotThrow(() => char.ToUpper(c).Should().BeUppercase());
    }

    [Test]
    public void ShouldFailLowerCaseAssertionOnUpperCaseCharAndPassOnLowerCaseChar()
    {
      var c = Any.AlphaChar();
      Assert.Throws<AssertionException>(() => char.ToUpper(c).Should().BeLowercase());
      Assert.DoesNotThrow(() => char.ToLower(c).Should().BeLowercase());
    }

    [Test]
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

    [Test]
    public void AllowAssertingWhetherConstClassHasUniqueValues()
    {
      new Action(() => typeof(ConstsWithUniqueValues).Should().HaveUniqueConstants())
        .Should().NotThrow();

      new Action(() => typeof(ConstsWithRepeatingValues).Should().HaveUniqueConstants())
        .Should().ThrowExactly<DuplicateConstantException>()
        .WithMessage("Val1 <0> is a duplicate of Val3 <0>");
    }

    [Test]
    public void ShouldFailNoStaticFieldsAssertionWhenTypeHasStaticFields()
    {
      new Action(() =>
          typeof(TypeWithStaticField).Should().NotHaveStaticFields())
        .Should().Throw<Exception>();
    }

    [Test]
    public void ShouldPassNoStaticFieldsAssertionWhenTypeHasStaticFields()
    {
      new Action(() =>
          typeof(TypeWithNoStaticField).Should().NotHaveStaticFields())
        .Should().NotThrow<Exception>();
    }

    [Test]
    public void ShouldPassUppercaseAssertionOnUppercaseString()
    {
       Any.UpperCaseString().Should().BeUppercase();
    }

    [Test]
    public void ShouldFailUppercaseAssertionOnLowercaseString()
    {

      new Action(() => (Any.UpperCaseString() + "a").Should().BeUppercase())
        .Should().Throw<AssertionException>();
    }
    [Test]
    public void ShouldPassLowercaseAssertionOnLowercaseString()
    {
      Any.LowerCaseString().Should().BeLowercase();
    }

    [Test]
    public void ShouldFailLowercaseAssertionOnUppercaseString()
    {

      new Action(() => (Any.LowerCaseString() + "A").Should().BeLowercase())
        .Should().Throw<AssertionException>();
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





