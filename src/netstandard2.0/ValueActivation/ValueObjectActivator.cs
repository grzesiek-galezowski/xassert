﻿namespace TddXt.XFluentAssert.ValueActivation
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using FluentAssertions;

  using TddXt.AnyRoot;
  using TddXt.XFluentAssert.TypeReflection;

  public class ValueObjectActivator
  {
    private readonly FallbackTypeGenerator _fallbackTypeGenerator;

    private List<object> _constructorArguments;

    public ValueObjectActivator(FallbackTypeGenerator fallbackTypeGenerator, Type type)
    {
      _fallbackTypeGenerator = fallbackTypeGenerator;
      TargetType = type;
    }

    public static ValueObjectActivator FreshInstance(Type type)
    {
      return new ValueObjectActivator(new FallbackTypeGenerator(SmartType.For(type)), type);
    }

    public object CreateInstanceAsValueObjectWithFreshParameters()
    {
      var instance = DefaultValue.Of(TargetType);
      this.Invoking(_ => { instance = _.CreateInstanceWithNewConstructorArguments(); }).Should()
        .NotThrow(TargetType + " cannot even be created as a value object");
      instance.GetType().Should().Be(TargetType);
      return instance;
    }

    public object CreateInstanceAsValueObjectWithPreviousParameters()
    {
      var instance = DefaultValue.Of(TargetType);
      this.Invoking(_ => { instance = _.CreateInstanceWithCurrentConstructorArguments(); }).Should()
        .NotThrow(TargetType + " cannot even be created as a value object");
      instance.GetType().Should().Be(TargetType);
      return instance;
    }

    public int GetConstructorParametersCount()
    {
      return _fallbackTypeGenerator.GetConstructorParametersCount();
    }

    public object CreateInstanceAsValueObjectWithModifiedParameter(int i)
    {
      var modifiedArguments = _constructorArguments.ToList();
      modifiedArguments[i] = Root.Any.InstanceAsObject(modifiedArguments[i].GetType());
      return _fallbackTypeGenerator.GenerateInstance(modifiedArguments.ToArray());
    }

    public Type TargetType { get; }

    private object CreateInstanceWithNewConstructorArguments()
    {
      _constructorArguments = _fallbackTypeGenerator.GenerateConstructorParameters(Root.Any.InstanceAsObject);
      return CreateInstanceWithCurrentConstructorArguments();
    }

    private object CreateInstanceWithCurrentConstructorArguments()
    {
      return _fallbackTypeGenerator.GenerateInstance(_constructorArguments.ToArray());
    }

  }
}