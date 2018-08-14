using System;
using NUnit.Framework;
using TddEbook.TddToolkit;

namespace TypeReflectionSpecification
{
  using TddXt.XAssert.TypeReflection;

  public class SmartTypeSpecification
  {
    [TestCase(typeof(Exception))]
    [TestCase(typeof(ArgumentException))]
    [TestCase(typeof(InvalidCastException))]
    [TestCase(typeof(SystemException))]
    public void ShouldReportWhenItIsDerivedFromException(Type exceptionType)
    {
      Assert.True(SmartType.For(exceptionType).IsException());
    }
    
    [Test]
    public void ShouldReportWhenItIsNotDerivedFromException()
    {
      Assert.False(SmartType.For(typeof(string)).IsException());
    }

  }
}
