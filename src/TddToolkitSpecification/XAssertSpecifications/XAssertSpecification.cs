using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommonTypes;
using FluentAssertions;
using NUnit.Framework;
using TddEbook.TddToolkit;
using TddEbook.TddToolkitSpecification.Fixtures;
using TddXt.AnyRoot.Strings;
using TypeReflection.ImplementationDetails;
using static TddXt.AnyRoot.Root;

namespace TddEbook.TddToolkitSpecification.XAssertSpecifications
{
  public class XAssertSpecification
  {

    [Test]
    public void ShouldThrowAssertionExceptionWhenTypeIsNotGuardedAgainstNullConstructorParameters()
    {
      var exception = Assert.Throws<AssertionException>(XAssert.HasNullProtectedConstructors<NotGuardedObject>);
      StringAssert.Contains("Not guarded parameter: String b", exception.Message);
      StringAssert.Contains("Not guarded parameter: String dede", exception.Message);
      StringAssert.DoesNotContain("Not guarded parameter: Int32 a", exception.Message);
    }

    [Test]
    public void ShouldNotThrowAssertionExceptionWhenTypeIsGuardedAgainstNullConstructorParameters()
    {
      Assert.DoesNotThrow(XAssert.HasNullProtectedConstructors<GuardedObject>);
    }

    [Test]
    public void ShouldPassValueTypeAssertionForProperValueType()
    {
      XAssert.IsValue<ProperValueType>();
    }

    [Test]
    public void ShouldPassValueTypeAssertionForProperValueTypeWithInternalConstructor()
    {
      XAssert.IsValue<FileExtension>();
    }

    [Test]
    public void ShouldPreferInternalNonRecursiveConstructorsToPublicRecursiveOnes()
    {
      Assert.DoesNotThrow(() => Any.Instance<DirectoryPath>());
      Assert.DoesNotThrow(() => XAssert.IsValue<DirectoryPath>());
    }

    [Test]
    public void ShouldBeAbleToChooseInternalConstructorWhenThereisNoPublicOne()
    {
      Assert.DoesNotThrow(() => Any.Instance<FileNameWithoutExtension>());
      Assert.DoesNotThrow(() => XAssert.IsValue<FileNameWithoutExtension>());
    }

    [Test]
    public void ShouldAllowSpecifyingConstructorArgumentsNotTakenIntoAccountDuringValueBehaviorCheck()
    {
      XAssert.IsValue<ProperValueTypeWithOneArgumentIdentity>(
        ValueTypeTraits.Custom.SkipConstructorArgument(0));

      Assert.Throws<AssertionException>(XAssert.IsValue<ProperValueTypeWithOneArgumentIdentity>);
    }

    [Test]
    public void ShouldAcceptProperFullValueTypesAndRejectBadOnes()
    {
      XAssert.IsValue<ProperValueType>();
      Assert.Throws<AssertionException>(XAssert.IsValue<ProperValueTypeWithoutEqualityOperator>);
    }

    [Test]
    public void ShouldWorkForStructuresWithDefaultEquality()
    {
      XAssert.IsValue<Maybe<string>>();
    }

    [Test]
    public void ShouldWorkForPrimitves()
    {
      XAssert.IsValue<int>();
    }

    [Test]
    public void ShouldFailUpperCaseAssertionOnLowerCaseStringAndPassOnUpperCaseString()
    {
      var s = Any.String();
      Assert.Throws<AssertionException>(() => XAssert.IsUpperCase(s.ToLower()) );
      Assert.DoesNotThrow(() => XAssert.IsUpperCase(s.ToUpper()));
    }

    [Test]
    public void ShouldFailLowerCaseAssertionOnUpperCaseStringAndPassOnLowerCaseString()
    {
      var s = Any.String();
      Assert.Throws<AssertionException>(() => XAssert.IsLowerCase(s.ToUpper()));
      Assert.DoesNotThrow(() => XAssert.IsLowerCase(s.ToLower()));
    }

    [Test]
    public void ShouldFailUpperCaseAssertionOnLowerCaseCharAndPassOnUpperCaseChar()
    {
      var c = Any.AlphaChar();
      Assert.Throws<AssertionException>(() => XAssert.IsUpperCase(char.ToLower(c)));
      Assert.DoesNotThrow(() => XAssert.IsUpperCase(char.ToUpper(c)));
    }

    [Test]
    public void ShouldFailLowerCaseAssertionOnUpperCaseCharAndPassOnLowerCaseChar()
    {
      var c = Any.AlphaChar();
      Assert.Throws<AssertionException>(() => XAssert.IsLowerCase(char.ToUpper(c)));
      Assert.DoesNotThrow(() => XAssert.IsLowerCase(char.ToLower(c)));
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
    public void AllowAssertingWhetherEnumHasUniqueValues()
    {
      XAssert.EnumHasUniqueValues<EnumWithUniqueValues>();
      Assert.Throws<AssertionException>(XAssert.EnumHasUniqueValues<EnumWithRepeatingValues>);
    }

    [Test]
    public void AllowAssertingWhetherConstClassHasUniqueValues()
    {
      new Action(() => XAssert.HasUniqueConstants<ConstsWithUniqueValues>())
        .Should().NotThrow();

      new Action(() => XAssert.HasUniqueConstants<ConstsWithRepeatingValues>())
        .Should().ThrowExactly<DuplicateConstantException>()
        .WithMessage("Val1 <0> is a duplicate of Val3 <0>");
    }
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





