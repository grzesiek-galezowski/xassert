namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  internal interface IAmBinaryOperator<in T, out TResult>
  {
    TResult Evaluate(T? instance1, T? instance2);
  }

  internal interface IAmBinaryOperator //todo add binary predicate
  {
    object Evaluate(object? instance1, object? instance2);
  }

}
