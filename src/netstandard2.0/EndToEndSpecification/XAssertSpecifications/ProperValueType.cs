using System.Collections.Generic;
using Value;
using System;
using System.Linq;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications;

public sealed class StructByValue<T> : IEquatable<StructByValue<T>> where T : struct
{
  private readonly T _element;

  public StructByValue(T element)
  {
    _element = element;
  }

  public bool Equals(StructByValue<T>? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return _element.Equals(other._element);
  }

  public override bool Equals(object? obj)
  {
    return ReferenceEquals(this, obj) || obj is StructByValue<T> other && Equals(other);
  }

  public override int GetHashCode()
  {
    return _element.GetHashCode();
  }

  public static bool operator ==(StructByValue<T>? left, StructByValue<T>? right)
  {
    return Equals(left, right);
  }

  public static bool operator !=(StructByValue<T>? left, StructByValue<T>? right)
  {
    return !Equals(left, right);
  }
}

public sealed class ProperValueType : IEquatable<ProperValueType>
{
  public bool Equals(ProperValueType other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return _a == other._a &&  
           _anArray != null && 
           other._anArray != null && 
           _anArray.SequenceEqual(other._anArray);
  }

  public override bool Equals(object obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != GetType()) return false;
    return Equals((ProperValueType)obj);
  }

  public override int GetHashCode()
  {
    unchecked
    {
      return (_a * 397) ^ (_anArray != null ? 
        new ListByValue<StructByValue<int>>(_anArray.Select(i => new StructByValue<int>(i)).ToList()).GetHashCode() : 0);
    }
  }

  public static bool operator ==(ProperValueType left, ProperValueType right)
  {
    return Equals(left, right);
  }

  public static bool operator !=(ProperValueType left, ProperValueType right)
  {
    return !Equals(left, right);
  }

  private readonly int _a;
  private readonly int[] _anArray;

  public ProperValueType(int a, int[] anArray)
  {
    _a = a;
    _anArray = anArray;
  }
}

public sealed class ProperValueTypeDerivedFromLibrary : ValueType<ProperValueTypeDerivedFromLibrary>
{

  private readonly int _a;
  private readonly string _str;

  public ProperValueTypeDerivedFromLibrary(int a, string str)
  {
    _a = a;
    _str = str;
  }

  protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
  {
    yield return _a;
    yield return _str;
  }
}