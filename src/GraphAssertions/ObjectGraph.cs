using KellermanSoftware.CompareNetObjects;

namespace GraphAssertions
{
  using System;
  using System.Collections.Generic;
  using System.Linq.Expressions;

  using TddXt.XAssert.CommonTypes;
  using TddXt.XAssert.TypeReflection;

  public static class ObjectGraph
  {
    public static CompareLogic Comparison()
    {
      var comparisonMechanism = new CompareLogic
      {
        Config = new ComparisonConfig
        {
          CompareChildren = true,
          CompareFields = true,
          ComparePrivateFields = true,
          ComparePrivateProperties = true,
          CompareProperties = true,
          CompareReadOnly = true,
          CompareStaticFields = false,
          CompareStaticProperties = false,
          MaxDifferences = 1
        }
      };
      comparisonMechanism.Config.CustomComparers.Add(new ReflectionOrProxyComparer());
      return comparisonMechanism;
    }

    public static ComparisonResult Compare<T>(T expected, T actual, string[] skippedPropertiesOrFields)
    {
      var comparison = ObjectGraph.Comparison();
      foreach (var skippedMember in skippedPropertiesOrFields)
      {
        comparison.Config.MembersToIgnore.Add(skippedMember);
      }
      var result = comparison.Compare(expected, actual);
      return result;
    }

    public static ComparisonResult Compare<T>(T expected, T actual, IEnumerable<Expression<Func<T, object>>> skippedPropertiesOrFields)
    {
      var comparison = ObjectGraph.Comparison();
      foreach (var skippedPropertyOrField in skippedPropertiesOrFields)
      {
        var property = Property.FromUnaryExpression(skippedPropertyOrField);
        var field = Field.FromUnaryExpression(skippedPropertyOrField);
        if (property.HasValue)
        {
          Ignore(property, comparison);
        }
        else if (field.HasValue)
        {
          Ignore(field, comparison);
        }
      }
      var result = comparison.Compare(expected, actual);
      return result;
    }

    private static void Ignore(Maybe<Field> field, ICompareLogic comparison)
    {
      comparison.Config.MembersToIgnore.Add(field.Value.Name);
    }

    private static void Ignore(Maybe<Property> property, ICompareLogic comparison)
    {
      comparison.Config.MembersToIgnore.Add(property.Value.Name);
      comparison.Config.MembersToIgnore.Add($"<{property.Value.Name}>k__BackingField");
    }
  }
}