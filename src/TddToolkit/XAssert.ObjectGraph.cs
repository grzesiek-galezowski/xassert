using System;
using System.Linq.Expressions;
using System.Reflection;
using Albedo;
using FluentAssertions;
using GraphAssertions;

namespace TddEbook.TddToolkit
{
  public partial class XAssert
  {
    public static void Alike<T>(T expected, T actual)
    {
      actual.Should().BeLike(expected);
    }

    public static void NotAlike<T>(T expected, T actual)
    {
      actual.Should().NotBeLike(expected);
    }

    public static void Alike<T>(T expected, T actual, params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, actual, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
    }

    public static void NotAlike<T>(T expected, T actual, params Expression<Func<T, object>>[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, actual, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
    }

    public static void Alike<T>(T expected, T actual, params string[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, actual, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeFalse(result.DifferencesString);
    }

    public static void NotAlike<T>(T expected, T actual, params string[] skippedPropertiesOrFields)
    {
      var result = ObjectGraph.Compare(expected, actual, skippedPropertiesOrFields);
      result.ExceededDifferences.Should().BeTrue(result.DifferencesString);
    }

    public static void Contains(Object o, Type t) // todo this is unfinished!!!!
    {
      var propertiesAndFields = o.GetType().GetPropertiesAndFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.NonPublic);
      new SearchVisitor(o, t);
    }

  }

  //bug this is unfinished and does not work yet!!
  public class SearchVisitor : ReflectionVisitor<Boolean>
  {
    private readonly object target;
    private readonly Type _searchedType;
    private bool value = false;

    public SearchVisitor(object target, Type searchedType)
    {
      this.target = target;
      _searchedType = searchedType;
    }

    public override bool Value
    {
      get { return this.value; }
    }

    public override IReflectionVisitor<bool> Visit(
      FieldInfoElement fieldInfoElement)
    {
      if (_searchedType == fieldInfoElement.FieldInfo.FieldType)
      {
        value = true;
      }
      return this;
    }

    public override IReflectionVisitor<bool> Visit(
      PropertyInfoElement propertyInfoElement)
    {
      if (_searchedType == propertyInfoElement.PropertyInfo.PropertyType)
      {
        value = true;
      }
      return this;
    }
  }
}
