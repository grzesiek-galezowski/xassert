using TddXt.XFluentAssert.TypeReflection.ImplementationDetails;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection
{
  public static class TypeOf<T>
  {
    private static readonly IType Type;

    static TypeOf()
    {
      Type = SmartType.For(typeof(T));
    }

    public static IAmBinaryOperator<T, bool> Equality()
    {
      return BinaryOperator<T, bool>.Wrap(Type.EqualityOperator());
    }

    public static IAmBinaryOperator<T, bool> InequalityOperator()
    {
      return BinaryOperator<T, bool>.Wrap(Type.InequalityOperator());
    }
  }
}