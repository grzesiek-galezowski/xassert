using Castle.DynamicProxy;
using TddEbook.TddToolkit.CommonTypes;

namespace TddEbook.TddToolkit.TypeResolution.Interceptors
{
  public class ExplodingInterceptor : IInterceptor
  {
      public void Intercept(IInvocation invocation)
      {
        throw new BoooooomException(invocation.Method.Name);
      }
  }
}

