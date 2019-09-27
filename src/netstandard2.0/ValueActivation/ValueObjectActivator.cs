namespace TddXt.XFluentAssert.ValueActivation
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using FluentAssertions;

  using AnyRoot;
  using TypeReflection;

  public class ValueObjectActivator
  {
    private readonly ObjectGenerator _objectGenerator;

    private List<object> _constructorArguments;

    public ValueObjectActivator(ObjectGenerator objectGenerator, Type type)
    {
      _objectGenerator = objectGenerator;
      TargetType = type;
    }

    public static ValueObjectActivator FreshInstance(Type type)
    {
      return new ValueObjectActivator(new ObjectGenerator(SmartType.For(type)), type);
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
      return _objectGenerator.GetConstructorParametersCount();
    }

    public object CreateInstanceAsValueObjectWithModifiedParameter(int i)
    {
      var modifiedArguments = _constructorArguments.ToList();
      modifiedArguments[i] = Root.Any.InstanceAsObject(modifiedArguments[i].GetType());
      return _objectGenerator.GenerateInstance(modifiedArguments.ToArray());
    }

    public Type TargetType { get; }

    private object CreateInstanceWithNewConstructorArguments()
    {
      _constructorArguments = _objectGenerator.GenerateConstructorParameters(Root.Any.InstanceAsObject);
      return CreateInstanceWithCurrentConstructorArguments();
    }

    private object CreateInstanceWithCurrentConstructorArguments()
    {
      return _objectGenerator.GenerateInstance(_constructorArguments.ToArray());
    }

  }
}