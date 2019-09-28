namespace TddXt.XFluentAssert.TypeReflection
{
  using System;

  using ImplementationDetails;
  using Interfaces;

  public static class TypeOf<T>
  {
    private static readonly IType Type;

    static TypeOf()
    {
      Type = SmartType.For(typeof (T));
    }

    public static bool HasParameterlessConstructor()
    {
      return Type.HasPublicParameterlessConstructor();
    }

    public static bool IsImplementationOfOpenGeneric(Type openGenericType)
    {
      return Type.IsImplementationOfOpenGeneric(openGenericType);
    }

    public static bool IsConcrete()
    {
      return Type.IsConcrete();
    }

    public static IAmBinaryOperator<T, bool> Equality()
    {

      return BinaryOperator<T, bool>.Wrap(Type.EqualityOperator());
    }

    public static IAmBinaryOperator<T, bool> InequalityOperator()
    {
      return BinaryOperator<T, bool>.Wrap(Type.InequalityOperator());
    }

    public static bool Is<T1>()
    {
      return typeof (T) == typeof (T1);
    }
  }
}