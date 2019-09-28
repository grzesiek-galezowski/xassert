namespace TddXt.XFluentAssert.CommonTypes
{
  using System;

  public static class Maybe
  {
    public static Maybe<T> FromNullable<T>(T instance) where T : class
    {
      return new Maybe<T>(instance);
    }

    public static Maybe<T> Just<T>(T instance) where T : class
    {
      if (instance == null)
      {
        throw new Exception("No instance of type " + typeof(T));
      }
      return new Maybe<T>(instance);
    }

  }

  public struct Maybe<T> where T : class
  {
    private readonly T _value;

    public Maybe(T instance)
      : this()
    {
      if (instance != null)
      {
        HasValue = true;
        _value = instance;
      }
    }

    public bool HasValue { get; }

    public T Value()
    {
      if (HasValue)
      {
        return _value;
      }
      else
      {
        throw new Exception("No instance of type " + typeof(T));
      }
    }

    public static Maybe<T> Nothing { get; } = new Maybe<T>();

    public Maybe<T> Otherwise(Maybe<T> alternative)
    {
      return HasValue ? this : alternative;
    }

    public static implicit operator Maybe<T>(T instance)
    {
      return Maybe.FromNullable(instance);
    }

    public override string ToString()
    {
      return HasValue ? Value().ToString() : "<Nothing>";
    }
  }

}
