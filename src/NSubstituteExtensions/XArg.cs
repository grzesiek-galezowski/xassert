﻿using System;
using System.Collections;
using FluentAssertions;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;
using NSubstituteExtensions.ImplementationDetails;
using TddEbook.TddToolkit;

namespace NSubstituteExtensions
{
  public static class XArg
  {
    public static T IsLike<T>(T expected)
    {
      return Passing<T>(actual => XAssert.Alike(expected, actual));
    }

    public static T IsNotLike<T>(T expected)
    {
      return Passing<T>(actual => XAssert.NotAlike(expected, actual));
    }

    public static T Passing<T>(params Action<T>[] assertions)
    {
      assertions
        .Should().NotBeEmpty("at least one condition should be specified");

      var lambdaMatcher = new LambdaArgumentMatcher<T>(assertions);
      EnqueueMatcher<T>(lambdaMatcher);
      return default(T);
    }

    public static T EquivalentTo<T>(T expected)
    {
      return Passing<T>(actual => actual.Should().BeEquivalentTo(expected));
    }

    private static void EnqueueMatcher<T>(IArgumentMatcher lambdaMatcher)
    {
      SubstitutionContext.Current.EnqueueArgumentSpecification(
        new ArgumentSpecification(typeof(T), 
          lambdaMatcher));
    }

    public static T SequenceEqualTo<T>(T expected) where T : IEnumerable
    {
      return Passing<T>(actual => actual.Should().Equal(expected));
    }

    public static T NotSequenceEqualTo<T>(T expected) where T : IEnumerable
    {
      return Passing<T>(actual => actual.Should().NotEqual(expected));
    }

  }
}