namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  using TddXt.XFluentAssert.TypeReflection.Interfaces;

  public class CreationMethod : ICreateObjects
  {
    public static CreationMethod FromConstructorInfo(ConstructorInfo constructor)
    {
      return new CreationMethod(constructor, constructor.Invoke, 
        constructor.DeclaringType, 
        new CreationMethodParameters(constructor.GetParameters()));
    }

    public static CreationMethod FromStaticMethodInfo(MethodInfo m)
    {
      return new CreationMethod(m, args => m.Invoke(null, args), 
        m.ReturnType, 
        new CreationMethodParameters(m.GetParameters()));
    }

    private readonly MethodBase _constructor;
    private readonly Type _returnType;
    private readonly Func<object[], object> _invocation;
    private readonly CreationMethodParameters _creationMethodParameters;

    private CreationMethod(
      MethodBase constructor, 
      Func<object[], object> invocation, 
      Type returnType, CreationMethodParameters creationMethodParameters)
    {
      this._constructor = constructor;
      this._creationMethodParameters = creationMethodParameters;
      this._returnType = returnType;
      this._invocation = invocation;
    }

    public bool HasNonPointerArgumentsOnly()
    {
      if(!this._creationMethodParameters.ContainAnyPointer())
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public bool HasLessParametersThan(int numberOfParams)
    {
      if (this._creationMethodParameters.AreLessThan(numberOfParams))
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public int GetParametersCount()
    {
      return this._creationMethodParameters.Count();
    }

    public bool HasAbstractOrInterfaceArguments()
    {

      return this._creationMethodParameters.IsAnyAbstractInterface();
    }

    public List<object> GenerateAnyParameterValues(Func<Type, object> instanceGenerator)
    {
      var constructorValues = new List<object>();
      this._creationMethodParameters.FillWithGeneratedValues(instanceGenerator, constructorValues);
      return constructorValues;
    }

    public string GetDescriptionForParameter(int i)
    {
      return this._creationMethodParameters.GetDescriptionFor(i);
    }

    public object InvokeWithParametersCreatedBy(Func<Type, object> instanceGenerator)
    {
      return this._invocation(this.GenerateAnyParameterValues(instanceGenerator).ToArray());
    }

    public object InvokeWith(IEnumerable<object> constructorParameters)
    {
      return this._invocation(constructorParameters.ToArray());
    }

    public override string ToString()
    {
      var description = this._constructor.DeclaringType.Name + "(";
      description += this._creationMethodParameters + ")";
      return description;
    }

    public bool IsInternal()
    {
      return IsInternal(this._constructor);
    }

    public static bool IsInternal(MethodBase c)
    {
      return c.IsAssembly && !c.IsPublic && !c.IsStatic;
    }

    public bool IsFactoryMethod()
    {
      return this._constructor.DeclaringType == this._returnType;
    }

    public bool IsNotRecursive()
    {
      return !this.IsRecursive();
    }

    public bool IsRecursive()
    {
      return this._creationMethodParameters.IsAnyOfType(this._returnType);
    }
  }
}