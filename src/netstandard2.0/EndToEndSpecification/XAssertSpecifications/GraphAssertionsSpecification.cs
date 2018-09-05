using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using FluentAssertions;
using NSubstitute;
using TddXt.XFluentAssert.Root;
using Xunit;
using Xunit.Sdk;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications
{
  public class GraphAssertionsSpecification
  {
    [Fact]
    public void ShouldAllowAssertingOnTypeDependency()
    {
      //TODO split into several facts
      new A1().Should().DependOn<int>();
      new A1().Should().DependOn<A2>();
      new A1().Should().DependOn<A3>();
      new A1().Should().DependOn<B2>();
      new A1().Should().DependOn<B3>();

      new Action(() => new A1().Should().DependOn<SecurityAttribute>())
        .Should().ThrowExactly<XunitException>()
        .WithMessage("Could not find " +
                     "System.Security.Permissions.SecurityAttribute " +
                     "anywhere in dependency graph");

      new Action(() => new A1().Should().DependOn<A1>())
        .Should().ThrowExactly<XunitException>()
        .WithMessage("Could not find " +
                     "TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.A1 " +
                     "anywhere in dependency graph");
    }



    [Fact]
    public void ShouldAllowAssertingOnInstanceDependency()
    {
      //TODO split into several facts
      var a3 = new A3();
      var b3 = new B3();
      var abc = "abc";
      var num = 123;
      var a2 = new A2(a3, b3, abc, num);
      var b2 = new B2(b3);
      var a1 = new A1(a2, b2);

      a1.Should().DependOn(a2);
      a1.Should().DependOn(b2);
      a1.Should().DependOn(b3);
      a1.Should().DependOn(abc);
      a1.Should().DependOn(num);

      new Action(() => a1.Should().DependOn(new A2()))
        .Should().ThrowExactly<XunitException>().WithMessage(
          @"Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.A2 anywhere in dependency graph however, another instance of this type was found within the following paths: 
[Root(A1)]->[_a2(A2)]->[_a3(A3)]
[Root(A1)]->[_a2(A2)]->[_b3(B3)]
[Root(A1)]->[_a2(A2)]->[_str(String)]->[m_stringLength(Int32)]
[Root(A1)]->[_a2(A2)]->[_str(String)]->[m_firstChar(Char)]
[Root(A1)]->[_a2(A2)]->[_str(String)]->[FirstChar(Char)]
[Root(A1)]->[_a2(A2)]->[_str(String)]->[Length(Int32)]
[Root(A1)]->[_a2(A2)]->[_num(Int32)]");
      new Action(() => a1.Should().DependOn(new B2()))
        .Should().ThrowExactly<XunitException>().WithMessage(@"Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.B2 anywhere in dependency graph however, another instance of this type was found within the following paths: 
[Root(A1)]->[_b2(B2)]->[_b3(B3)]");
      new Action(() => a1.Should().DependOn(new B3()))
        .Should().ThrowExactly<XunitException>().WithMessage(@"Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.B3 anywhere in dependency graph however, another instance of this type was found within the following paths: 
[Root(A1)]->[_a2(A2)]->[_b3(B3)]
[Root(A1)]->[_b2(B2)]->[_b3(B3)]");
      new Action(() => a1.Should().DependOn(abc + "a"))
        .Should().ThrowExactly<XunitException>().WithMessage(@"Could not find the particular instance: abca anywhere in dependency graph however, another instance of this type was found within the following paths: 
[Root(A1)]->[_a2(A2)]->[_str(String)]->[m_stringLength(Int32)]
[Root(A1)]->[_a2(A2)]->[_str(String)]->[m_firstChar(Char)]
[Root(A1)]->[_a2(A2)]->[_str(String)]->[FirstChar(Char)]
[Root(A1)]->[_a2(A2)]->[_str(String)]->[Length(Int32)]");
      new Action(() => a1.Should().DependOn(num + 1))
        .Should().ThrowExactly<XunitException>().WithMessage(@"Could not find the particular instance: 124 anywhere in dependency graph however, another instance of this type was found within the following paths: 
[Root(A1)]->[_a2(A2)]->[_str(String)]->[m_stringLength(Int32)]
[Root(A1)]->[_a2(A2)]->[_str(String)]->[Length(Int32)]
[Root(A1)]->[_a2(A2)]->[_num(Int32)]");

      new Action(() => a1.Should().DependOn(a1))
        .Should().ThrowExactly<XunitException>().WithMessage(@"Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.A1 anywhere in dependency graph");
      new Action(() => a2.Should().DependOn(a1))
        .Should().ThrowExactly<XunitException>().WithMessage("Could not find the particular instance: TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications.A1 anywhere in dependency graph");

      new Action(() => abc.Should().DependOn(22))
        .Should().ThrowExactly<XunitException>().WithMessage(@"Could not find the particular instance: 22 anywhere in dependency graph however, another instance of this type was found within the following paths: 
[Root(String)]->[m_stringLength(Int32)]
[Root(String)]->[Length(Int32)]");

      abc.Should().DependOn(3);

    }

    [Fact]
    public void ShouldNotFailWhenInvokedOnObjectWithProxies()
    {
      var enumerable = Substitute.For<IEnumerable<int>>();
      new Action(() =>
          new MyObjectImpl(Substitute.For<IEnumerable<int>>()).Should().DependOn("lol"))
        .Should().ThrowExactly<XunitException>();

      new Action(() => { new MyObjectImpl(enumerable).Should().DependOn(enumerable); })
        .Should().NotThrow();

      new Action(() => new MyObjectImpl(Substitute.For<IEnumerable<int>>())
          .Should().DependOn(Substitute.For<IEnumerable<int>>()))
        .Should().NotThrow();

    }

    private class MyObjectImpl 
    {
      private readonly IEnumerable<int> _s;

      public MyObjectImpl(IEnumerable<int> s)
      {
        _s = s;
        //throw new NotImplementedException();
      }
    }
  }
}