using FluentAssertions;
using TddXt.XFluentAssert.TypeReflection.Interfaces;
using Xunit;

public class NewEqualityAssertionSpecification
{
  [Fact]
  public void ShouldXXXXX()
  {
    typeof(MyIntWrapper)
      .Should().HaveCorrectEquality(
        EqualityArg.For(() => 1, () => 2)
      );
  }

}