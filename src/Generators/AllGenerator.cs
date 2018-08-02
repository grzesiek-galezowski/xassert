using System;

namespace TddEbook.TddToolkit.Generators
{
  public class AllGenerator : OmniGenerator
  {
    public AllGenerator(ProxyBasedGenerator genericGenerator)
    {
      _genericGenerator = genericGenerator;
    }

    public const int Many = 3;

    private readonly ProxyBasedGenerator _genericGenerator;


    public object Instance(Type type)
    {
      return _genericGenerator.Instance(type);
    }

    public T InstanceOf<T>()
    {
      return _genericGenerator.InstanceOf<T>();
    }

    public T Instance<T>()
    {
      return _genericGenerator.Instance<T>();
    }

    public T Dummy<T>()
    {
      return _genericGenerator.Dummy<T>();
    }

    public T OtherThan<T>(params T[] omittedValues)
    {
      return _genericGenerator.OtherThan(omittedValues);
    }
  }
}