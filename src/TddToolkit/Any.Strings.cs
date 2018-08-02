namespace TddEbook.TddToolkit
{

  public partial class Any
  {
    public static string String()
    {
      return Generate.String();
    }

    public static string String(string seed)
    {
      return Generate.String(seed);
    }
  }
}
