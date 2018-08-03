using System;
using TypeReflection.ImplementationDetails;
using TypeReflection.Interfaces;

namespace TypeReflection
{
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

    public static IBinaryOperator<T, bool> Equality()
    {

      return BinaryOperator<T, bool>.Wrap(Type.Equality());
    }

    public static IBinaryOperator<T, bool> Inequality()
    {
      return BinaryOperator<T, bool>.Wrap(Type.Inequality());
    }

    public static bool Is<T1>()
    {
      return typeof (T) == typeof (T1);
    }
  }
}