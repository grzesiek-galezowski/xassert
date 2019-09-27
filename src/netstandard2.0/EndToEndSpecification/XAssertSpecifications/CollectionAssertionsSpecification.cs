using System;
using FluentAssertions;
using TddXt.XFluentAssert.Root;
using Xunit;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications
{
    public class CollectionAssertionsSpecification
    {

        [Fact]
        public void ShouldAllowTestingEquality()
        {
            var coll1 = new[] {1, 2, 3};
            var coll2 = new[] {3, 2, 1};
            var coll3 = new[] {1, 2, 3};

            coll1.Should().Be(coll3);
            coll1.Invoking(c => c.Should().Be(coll2)).Should().Throw<Exception>();
        }

    }
}