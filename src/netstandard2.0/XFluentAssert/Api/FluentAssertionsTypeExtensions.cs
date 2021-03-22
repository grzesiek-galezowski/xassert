﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Types;
using TddXt.AnyRoot;
using TddXt.XFluentAssert.Api.ConstraintsAssertions;
using TddXt.XFluentAssert.Api.ValueAssertions;
using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.ReflectionAssertions;
using TddXt.XFluentAssert.TypeReflection;
using TddXt.XFluentAssert.TypeReflection.Interfaces;
using TddXt.XFluentAssert.ValueActivation;
using TddXt.XFluentAssert.ValueObjectConstraints;

namespace TddXt.XFluentAssert.Api
{

  public static class FluentAssertionsTypeExtensions
  {

    public static AndConstraint<TypeAssertions> HaveValueSemantics(this TypeAssertions o)
    {
      o.Subject.Should().HaveValueSemantics(
        ValueTypeTraits.Default());
      return new AndConstraint<TypeAssertions>(o);
    }

    public static AndConstraint<TypeAssertions> HaveNullProtectedConstructors(
      this TypeAssertions o)
    {
      var smartType = SmartType.For(o.Subject);

      if (!smartType.HasConstructorWithParameters())
      {
        AssertionConstraintsEngine
          .TypeAdheresTo(new List<IConstraint> { new ConstructorsMustBeNullProtected(smartType) });
      }

      return new AndConstraint<TypeAssertions>(o);
    }

    public static AndConstraint<TypeAssertions> HaveValueSemantics(this TypeAssertions o,
      IKnowWhatValueTraitsToCheck traits)
    {
      Type type = o.Subject;
      if (!ValueObjectWhiteList.Contains(type))
      {
        var activator = ValueObjectActivator.FreshInstance(type, Root.Any.InstanceAsObject);
        var constraints = ValueObjectConstraints.AssertionConstraints.ForValueSemantics(type, traits, activator);
        AssertionConstraintsEngine.TypeAdheresTo(constraints);
      }

      return new AndConstraint<TypeAssertions>(o);
    }

    public static AndConstraint<TypeAssertions> HaveEventWithShortName(this TypeAssertions assertions,
      string eventName)
    {
      var allEvents = SmartType.For(assertions.Subject).GetAllEvents();

      allEvents.Should().Match(
        events => events.Any(ev => ev.HasName(eventName)),
        "Type " + assertions.Subject + " should define expected event " + eventName);

      return new AndConstraint<TypeAssertions>(assertions);
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
        .BeEmpty("SmartType " + type + " should not contain static fields, but: " + Environment.NewLine +
                 ReflectionElementsList.Format(staticFields));
      return new AndConstraint<TypeAssertions>(assertions);
    }
  }
}