namespace TddEbook.TddToolkit.CommonTypes
{
  public static class CircularList
  {
    public static CircularList<T> CreateStartingFrom0<T>(params T[] items)
    {
      return new CircularList<T>(0, items);
    }
  }

  public class CircularList<T>
  {
    private readonly T[] _items;
    private int _startingIndex;

    public CircularList(int startingIndex, params T[] items)
    {
      _items = items;
      _startingIndex = startingIndex;
    }

    public T Next()
    {
      if(_startingIndex > _items.Length - 1)
      {
        _startingIndex = 0; 
      }
      var result = _items[_startingIndex];
      _startingIndex++;
      return result;
    }
  }
}

