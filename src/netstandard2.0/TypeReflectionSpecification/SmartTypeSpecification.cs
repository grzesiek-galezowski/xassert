﻿using System;
using TddXt.XFluentAssert.TypeReflection;

using Xunit;

namespace TddXt.XFluentAssert.TypeReflectionSpecification;

public class SmartTypeSpecification
{
  [Theory]
  [InlineData(typeof(Exception))]
  [InlineData(typeof(ArgumentException))]
  [InlineData(typeof(InvalidCastException))]
  [InlineData(typeof(SystemException))]
  public void ShouldReportWhenItIsDerivedFromException(Type exceptionType)
  {
    Assert.True(SmartType.For(exceptionType).IsException());
  }

  [Fact]
  public void ShouldReportWhenItIsNotDerivedFromException()
  {
    Assert.False(SmartType.For(typeof(string)).IsException());
  }

}