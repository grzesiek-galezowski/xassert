namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  public interface IAmBinaryOperator<in T, out TResult>
  {
    TResult Evaluate(T instance1, T instance2);
  }

  public interface IAmBinaryOperator //todo add binary predicate
  {
    object Evaluate(object instance1, object instance2);
  }

}
