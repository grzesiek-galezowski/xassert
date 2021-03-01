using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using TddXt.XFluentAssert.TypeReflection;

namespace TddXt.XFluentAssert.ValueActivation
{
  public class ValueObjectActivator
  {
    private readonly ObjectGenerator _objectGenerator;

    private List<object> _constructorArguments;
    private readonly Func<Type, object> _objectFactory;

    public ValueObjectActivator(ObjectGenerator objectGenerator, Type type, Func<Type, object> objectFactory)
    {
      _objectGenerator = objectGenerator;
      TargetType = type;
      _objectFactory = objectFactory;
    }

    public static ValueObjectActivator FreshInstance(Type type, Func<Type, object> objectFactory)
    {
      return new ValueObjectActivator(new ObjectGenerator(SmartType.For(type)), type, objectFactory);
    }

    public object CreateInstanceAsValueObjectWithFreshParameters()
    {
      var instance = DefaultValue.Of(TargetType);
      DomainAssertions.AssertDoesNotThrow(
        () => instance = CreateInstanceWithNewConstructorArguments(), 
        $"{TargetType} cannot even be created as a value object");
      DomainAssertions.AssertAreNotEqual(instance.GetType(), TargetType);
      return instance;
    }

    public object CreateInstanceAsValueObjectWithPreviousParameters()
    {
      var instance = DefaultValue.Of(TargetType);
      DomainAssertions.AssertDoesNotThrow(
        () => instance = CreateInstanceWithCurrentConstructorArguments(),
        $"{TargetType} cannot even be created as a value object");
      DomainAssertions.AssertAreNotEqual(instance.GetType(), TargetType);
      return instance;
    }

    public int GetConstructorParametersCount()
    {
      return _objectGenerator.GetConstructorParametersCount();
    }

    public object CreateInstanceAsValueObjectWithModifiedParameter(int i)
    {
      var modifiedArguments = _constructorArguments.ToList();
      modifiedArguments[i] = _objectFactory(modifiedArguments[i].GetType());
      return _objectGenerator.GenerateInstance(modifiedArguments.ToArray());
    }

    public Type TargetType { get; }

    private object CreateInstanceWithNewConstructorArguments()
    {
      _constructorArguments = _objectGenerator.GenerateConstructorParameters(_objectFactory);
      return CreateInstanceWithCurrentConstructorArguments();
    }

    private object CreateInstanceWithCurrentConstructorArguments()
    {
      return _objectGenerator.GenerateInstance(_constructorArguments.ToArray());
    }

  }
}