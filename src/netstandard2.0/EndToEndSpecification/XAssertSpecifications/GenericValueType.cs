using System;
using System.Collections.Generic;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications;

public class GenericValueType<T>(T field) : IEquatable<GenericValueType<T>>
{
  public bool Equals(GenericValueType<T> other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return EqualityComparer<T>.Default.Equals(_field, other._field);
  }

  public override bool Equals(object obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != GetType()) return false;
    return Equals((GenericValueType<T>)obj);
  }

  public override int GetHashCode()
  {
    return EqualityComparer<T>.Default.GetHashCode(_field);
  }

  public static bool operator ==(GenericValueType<T> left, GenericValueType<T> right)
  {
    return Equals(left, right);
  }

  public static bool operator !=(GenericValueType<T> left, GenericValueType<T> right)
  {
    return !Equals(left, right);
  }

  private readonly T _field = field;
}