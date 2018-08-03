using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TddEbook.TddToolkit.Reflection
{
  public class Method
  {
    public static Method Of<T>(Expression<Action<T>> expression)
    {
      return new Method((expression.Body as MethodCallExpression).Method);
    }

    public bool HasAttribute(Type attributeType, Attribute expectedAttribute)
    {
      var attrs = Attribute.GetCustomAttributes(_methodInfo, attributeType);
      var any = attrs.Any(
        currentAttribute => Are.Alike(expectedAttribute, currentAttribute)
        );
      return any;
    }

    private Method(MethodInfo method)
    {
      _methodInfo = method;
    }

    private readonly MethodInfo _methodInfo;
  }
}