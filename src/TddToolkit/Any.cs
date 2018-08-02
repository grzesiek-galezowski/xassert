using System.Diagnostics.CodeAnalysis;
using TddEbook.TddToolkit.Generators;
using TddEbook.TddToolkit.Subgenerators;
using Type = System.Type;

namespace TddEbook.TddToolkit
{
  public partial class Any
  {
    private static readonly AllGenerator Generate = AllGeneratorFactory.Create();

    public static T Instance<T>()
    {
      return Generate.Instance<T>();
    }

    public static object Instance(Type type)
    {
      return Generate.Instance(type);
    }
  }


}

