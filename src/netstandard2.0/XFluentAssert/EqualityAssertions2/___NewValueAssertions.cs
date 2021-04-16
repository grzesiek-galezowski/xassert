using System;
using System.Linq;
using FluentAssertions;
using Functional.Maybe;

namespace TddXt.XFluentAssert.EqualityAssertions2
{
  public static class ___NewValueAssertions
  {
    public static void AssertIsProperValueObject<T>(Func<T>[] equalInstances, Func<T>[] otherInstances)
    {
      equalInstances.Should().NotBeNullOrEmpty();
      otherInstances.Should().NotBeNullOrEmpty();

      //equal to itself
      var instances = equalInstances.Select(i => i());
      foreach (var instance in instances)
      {
        instance.Should().Be(instance);
      }

      //equal to other equals
      foreach (var instanceFactory1 in equalInstances)
      {
        instanceFactory1().Equals(null).Should().BeFalse();

        //equality with null
        if (!typeof(T).IsValueType)
        {
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory1(), default)
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(default, instanceFactory1())
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory1(), default)
            .Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(default, instanceFactory1())
            .Should().BeTrue(); //bug message
        }

        foreach (var instanceFactory2 in equalInstances)
        {
          instanceFactory1().Equals(instanceFactory2()).Should().BeTrue(); //bug message
          instanceFactory2().Equals(instanceFactory1()).Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory1(), instanceFactory2())
            .Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory2(), instanceFactory1())
            .Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory1(), instanceFactory2())
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory2(), instanceFactory1())
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.EquatableEquality().Do(equality =>
          {
            equality.Evaluate(instanceFactory1(), instanceFactory2()).Should().BeTrue(); //bug message
            equality.Evaluate(instanceFactory2(), instanceFactory1()).Should().BeTrue(); //bug message
          });

          instanceFactory1().GetHashCode().Should().Be(instanceFactory2().GetHashCode()); //bug message

        }
      }

      //bug check for empty arrays
      //bug check for nulls

      foreach (var instanceFactory1 in equalInstances)
      {
        foreach (var instanceFactory2 in otherInstances)
        {
          instanceFactory1().Equals(instanceFactory2()).Should().BeFalse(); //bug message
          instanceFactory2().Equals(instanceFactory1()).Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory1(), instanceFactory2())
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.Equality().Evaluate(instanceFactory2(), instanceFactory1())
            .Should().BeFalse(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory1(), instanceFactory2())
            .Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.InequalityOperator().Evaluate(instanceFactory2(), instanceFactory1())
            .Should().BeTrue(); //bug message
          TypeReflection.TypeOf<T>.EquatableEquality().Do(equality =>
          {
            equality.Evaluate(instanceFactory1(), instanceFactory2()).Should().BeFalse(); //bug message
            equality.Evaluate(instanceFactory2(), instanceFactory1()).Should().BeFalse(); //bug message
          });
          instanceFactory1().GetHashCode().Should().NotBe(instanceFactory2().GetHashCode()); //bug message
        }
      }
    }
  }
}