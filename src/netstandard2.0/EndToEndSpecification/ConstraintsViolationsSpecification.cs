namespace TddXt.XFluentAssert.EndToEndSpecification
{
  using System;

  using FluentAssertions;

  using TddXt.AnyRoot;
  using TddXt.AnyRoot.Strings;
  using TddXt.XFluentAssert.AssertionConstraints;

  using Xunit;
  using Xunit.Sdk;

  //bug this isn't end to end
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
      violations.Add(Root.Any.String());

      //WHEN - THEN
      new Action(violations.AssertNone).Should().ThrowExactly<XunitException>();
    }

    [Fact]
    public void ShouldThrowExceptionContainingAllViolationMessagesWhenMoreThanOneViolationWasAdded()
    {
      //GIVEN
      var violations = new ConstraintsViolations();
      var violation1 = Root.Any.String();
      var violation2 = Root.Any.String();
      var violation3 = Root.Any.String();
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
      const string seed = "xyz";
      var violation1 = Root.Any.String(seed);
      var violation2 = Root.Any.String(seed);

      //THEN
      violation1.Should().StartWith(seed);
      violation2.Should().StartWith(seed);
      violation1.Should().NotBe(violation2);
    }
  }
}
