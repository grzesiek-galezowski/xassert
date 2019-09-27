using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using FluentAssertions.Reflection;
using TddXt.XFluentAssert.ReflectionAssertions;
using TddXt.XFluentAssert.TypeReflection;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssertRoot
{
  public static class FluentAssertionsAssemblyExtensions
  {
    public static AndConstraint<AssemblyAssertions> NotHaveHiddenEvents(this AssemblyAssertions assertions)
    {
      Assembly assembly = assertions.Subject;
      var nonPublicEvents = new List<IAmEvent>();

      foreach (var type in assembly.GetTypes().Select(SmartType.For))
      {
        nonPublicEvents.AddRange(type.GetAllNonPublicEventsWithoutExplicitlyImplemented());
      }

      nonPublicEvents.Should()
        .BeEmpty("assembly " + assembly + " should not contain non-public events, but: " + Environment.NewLine + ReflectionElementsList.NonPublicEventsFoundMessage(nonPublicEvents));
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
  }
}