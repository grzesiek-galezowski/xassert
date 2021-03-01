using System;
using FluentAssertions;
using TddXt.AnyRoot.Strings;
using TddXt.XFluentAssert.AssertionConstraints;
using Xunit;
using Xunit.Sdk;
using static TddXt.AnyRoot.Root;

namespace AssertionConstraintsSpecification
{
  public class ConstraintsViolationsSpecification
  {
    [Fact]
    public void ShouldNotThrowExceptionWhenNoViolationsHaveBeenAdded()
    {
      //GIVEN
      var violations = new ConstraintsViolations();

      //WHEN - THEN
      new Action(violations.AssertNone).Should().NotThrow();
    }

    [Fact]
    public void ShouldThrowExceptionWhenAtLeastOneViolationWasAdded()
    {
      //GIVEN
      var violations = new ConstraintsViolations();
      violations.Add(Any.String());

      //WHEN - THEN
      new Action(violations.AssertNone).Should().ThrowExactly<XunitException>();
    }

    [Fact]
    public void ShouldThrowExceptionContainingAllViolationMessagesWhenMoreThanOneViolationWasAdded()
    {
      //GIVEN
      var violations = new ConstraintsViolations();
      var violation1 = Any.String();
      var violation2 = Any.String();
      var violation3 = Any.String();
      violations.Add(violation1);
      violations.Add(violation2);
      violations.Add(violation3);

      //WHEN - THEN
      new Action(violations.AssertNone).Should().Throw<Exception>()
        .And.Message.Should().ContainAll(violation1, violation2, violation3);
    }

    [Fact]
    public void ShouldBeAbleToGenerateSeededStrings()
    {
      //WHEN
      const string Seed = "xyz";
      var violation1 = Any.String(Seed);
      var violation2 = Any.String(Seed);

      //THEN
      violation1.Should().StartWith(Seed);
      violation2.Should().StartWith(Seed);
      violation1.Should().NotBe(violation2);
    }
  }
}
