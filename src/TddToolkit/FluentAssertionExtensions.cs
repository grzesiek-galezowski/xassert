namespace TddEbook.TddToolkit
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Reflection;

  using AssertionConstraints;

  using FluentAssertions;
  using FluentAssertions.Primitives;
  using FluentAssertions.Reflection;
  using FluentAssertions.Types;

  using GraphAssertions;

  using LockAssertions;

  using TypeReflection;
  using TypeReflection.Interfaces;

  using ValueActivation;

  using ValueObjectConstraints;

  public static class FluentAssertionExtensions
  {

    public static AndConstraint<CharAssertions> BeUppercase(this CharAssertions assertions)
    {
      char c = assertions.CharSubject;
      CharExtensions.Should(c).Be(char.ToUpperInvariant(c));
      return new AndConstraint<CharAssertions>(assertions);
    }

    public static AndConstraint<CharAssertions> BeLowercase(this CharAssertions assertions)
    {
      char c = assertions.CharSubject;
      CharExtensions.Should(c).Be(char.ToLowerInvariant(c));
      return new AndConstraint<CharAssertions>(assertions);
    }


    public static AndConstraint<StringAssertions> BeUppercase(this StringAssertions assertions)
    {
      string str = assertions.Subject;
      str.Should().Be(str.ToUpperInvariant());
      return new AndConstraint<StringAssertions>(assertions);
    }

    public static AndConstraint<StringAssertions> BeLowercase(this StringAssertions assertions)
    {
      string str = assertions.Subject;
      str.Should().Be(str.ToLowerInvariant());
      return new AndConstraint<StringAssertions>(assertions);
    }

    public static AndConstraint<TypeAssertions> HaveUniqueConstants(this TypeAssertions assertions)
    {
      var constants = SmartType.For(assertions.Subject).GetAllConstants();
      foreach (var constant in constants)
      {
        foreach (var otherConstant in constants)
        {
          constant.AssertNotDuplicateOf(otherConstant);
        }
      }

      return new AndConstraint<TypeAssertions>(assertions);
    }

    public static AndConstraint<TypeAssertions> NotHaveStaticFields(this TypeAssertions assertions)
    {
      Type type = assertions.Subject;
      var staticFields = new List<IAmField>(SmartType.For(type).GetAllStaticFields());

      staticFields.Should()
        .BeEmpty("SmartType " + type + " should not contain static fields, but: " + Environment.NewLine + ReflectionElementsList.Format(staticFields));
      return new AndConstraint<TypeAssertions>(assertions);
    }

    public static AndConstraint<AssemblyAssertions> NotHaveStaticFields(this AssemblyAssertions assertions)
    {
      Assembly assembly = assertions.Subject;
      var staticFields = new List<IAmField>();
      foreach (var type in assembly.GetTypes())
      {
        staticFields.AddRange(SmartType.For(type).GetAllStaticFields());
      }

      staticFields.Should()
        .BeEmpty(
          "assembly " + assembly + " should not contain static fields, but: " + Environment.NewLine + ReflectionElementsList.Format(staticFields));
      return new AndConstraint<AssemblyAssertions>(assertions);
    }

    public static AndConstraint<AssemblyAssertions> NotHaveHiddenEvents(this AssemblyAssertions assertions)
    {
      Assembly assembly = assertions.Subject;
      var nonPublicEvents = new List<IAmEvent>();
        
      foreach (var type in assembly.GetTypes().Select(SmartType.For))
      {
        nonPublicEvents.AddRange(type.GetAllNonPublicEventsWithoutExplicitlyImplemented());
      }

      nonPublicEvents.Should()
        .BeEmpty("assembly " + assembly + " should not contain non-public events, but: " + Environment.NewLine + ReflectionElementsList.Format(nonPublicEvents));
      return new AndConstraint<AssemblyAssertions>(assertions);
    }

    public static AndConstraint<AssemblyAssertions> HaveOnlyTypesWithSingleConstructor(this AssemblyAssertions assertions)
    {
      Assembly assembly = assertions.Subject;
      var constructorLimitsExceeded = new List<Tuple<Type, int>>();
        
      foreach (var type in assembly.GetTypes())
      {
        var constructorCount = SmartType.For(type).GetAllPublicConstructors().Count();
        if (constructorCount > 1)
        {
          constructorLimitsExceeded.Add(Tuple.Create(type, constructorCount)); 
        }
      }

      constructorLimitsExceeded.Any().Should()
        .BeFalse("assembly " + assembly +
                 " should not contain types with more than one constructor, but: " +
                 Environment.NewLine + ReflectionElementsList.Format(constructorLimitsExceeded));
      return new AndConstraint<AssemblyAssertions>(assertions);
    }


    public static AndConstraint<AssemblyAssertions> NotReferenceAssemblyWith(this AssemblyAssertions assertions, Type type)
    {
      assertions.Subject.Should().NotReference(type.Assembly);
      return new AndConstraint<AssemblyAssertions>(assertions);
    }

    public static AndConstraint<ObjectAssertions> BeLike<T>(this ObjectAssertions o, T expected)
    {
      var comparison = ObjectGraph.Comparison();
      var result = comparison.Compare(expected, o.Subject);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ObjectAssertions> NotBeLike<T>(this ObjectAssertions o, T expected)
    {
      var comparison = ObjectGraph.Comparison();
      var result = comparison.Compare(expected, o.Subject);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ObjectAssertions> NotBeLike<T>(
      this ObjectAssertions o,
      T expected,
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, (T)o.Subject, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<ObjectAssertions> BeLike<T>(
      this ObjectAssertions o,
      T expected, 
      params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, (T)o.Subject, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
      return new AndConstraint<ObjectAssertions>(o);
    }

    public static AndConstraint<TypeAssertions> HaveValueSemantics(this TypeAssertions o)
    {
      o.Subject.Should().HaveValueSemantics(ValueTypeTraits.Default());
      return new AndConstraint<TypeAssertions>(o);
    }

    public static AndConstraint<TypeAssertions> HaveValueSemantics(this TypeAssertions o, ValueTypeTraits traits)
    {
      Type type = o.Subject;
      if (!ValueObjectWhiteList.Contains(type))
      {
        var activator = ValueObjectActivator.FreshInstance(type);
        var constraints = XAssert.CreateConstraintsBasedOn(type, traits, activator);
        AssertionConstraintsEngine.TypeAdheresTo(constraints);
      }

      return new AndConstraint<TypeAssertions>(o);
    }


    public static void SynchronizeAccessTo<T>(this ObjectAssertions assertions,
      Action<T> callToCheck,
      LockAssertions lockAssertions,
      T wrappedObjectMock) 
      where T : class
    {
      SynchronizationAssertions.Synchronizes(
        (T)assertions.Subject, 
        callToCheck, 
        lockAssertions, 
        wrappedObjectMock);
    }

    public static void SynchronizeAccessTo<T, TReturn>(this ObjectAssertions assertions,
      Func<T, TReturn> callToCheck,
      LockAssertions lockAssertions,
      T wrappedObjectMock)
      where T : class
    {
      SynchronizationAssertions.Synchronizes(
        (T)assertions.Subject,
        callToCheck,
        lockAssertions,
        wrappedObjectMock);
    }
  }
}
