using System;
using FluentAssertions;
using Xunit;
using FluentAssertionsEnumerableExtensions = TddXt.XFluentAssert.Api.FluentAssertionsEnumerableExtensions;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications
{
  public class CollectionAssertionsSpecification
  {

    [Fact]
    public void ShouldAllowTestingEquality()
    {
      var coll1 = new[] { 1, 2, 3 };
      var coll2 = new[] { 3, 2, 1 };
      var coll3 = new[] { 1, 2, 3 };

      FluentAssertionsEnumerableExtensions.Be(coll1.Should(), coll3);
      coll1.Invoking(c => FluentAssertionsEnumerableExtensions.Be(c.Should(), coll2)).Should().Throw<Exception>();
    }

    [Fact]
    public void ShouldUseEqualsForEquality()
    {
      var coll1 = new[] { new AlwaysNotEqual(1) };
      var coll2 = new[] { new AlwaysNotEqual(1) };

      coll1.Invoking(c => FluentAssertionsEnumerableExtensions.Be(c.Should(), coll2)).Should().Throw<Exception>();
    }

    private class AlwaysNotEqual
    {
      private int _a;

      public AlwaysNotEqual(int a)
      {
        this._a = a;
      }

      public override bool Equals(object obj)
      {
        return false;
      }

    }

  }
}