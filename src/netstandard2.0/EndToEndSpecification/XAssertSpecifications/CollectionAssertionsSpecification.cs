using System;
using AwesomeAssertions;
using TddXt.XFluentAssert.Api;
using Xunit;
using AwesomeAssertionsEnumerableExtensions = TddXt.XFluentAssert.Api.FluentAssertionsEnumerableExtensions;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications;

public class CollectionAssertionsSpecification
{

  [Fact]
  public void ShouldAllowTestingEquality()
  {
    var coll1 = new[] { 1, 2, 3 };
    var coll2 = new[] { 3, 2, 1 };
    var coll3 = new[] { 1, 2, 3 };

    coll1.Should().Be(coll3);
    coll1.Invoking(c => c.Should().Be(coll2)).Should().Throw<Exception>();
  }

  [Fact]
  public void ShouldUseEqualsForEquality()
  {
    var coll1 = new[] { new AlwaysNotEqual(1) };
    var coll2 = new[] { new AlwaysNotEqual(1) };

    coll1.Invoking(c => c.Should().Be(coll2)).Should().Throw<Exception>();
  }

  private class AlwaysNotEqual(int a)
  {
    private int _a = a;

    public override bool Equals(object obj)
    {
      return false;
    }

  }

}