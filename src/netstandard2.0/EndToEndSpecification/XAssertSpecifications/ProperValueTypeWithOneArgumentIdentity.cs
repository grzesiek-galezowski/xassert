using System;
using System.Collections.Generic;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications;

public sealed class ProperValueTypeWithOneArgumentIdentity : IEquatable<ProperValueTypeWithOneArgumentIdentity>
{
  public bool Equals(ProperValueTypeWithOneArgumentIdentity other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return Equals(_a, other._a);
  }

  public override bool Equals(object obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != GetType()) return false;
    return Equals((ProperValueTypeWithOneArgumentIdentity)obj);
  }

  public override int GetHashCode()
  {
    unchecked
    {
      return (_a != null ? _a.GetHashCode() : 0);
    }
  }

  public static bool operator ==(
    ProperValueTypeWithOneArgumentIdentity left, ProperValueTypeWithOneArgumentIdentity right)
  {
    if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
    {
      return true;
    }
    else if (ReferenceEquals(left, null))
    {
      return false;
    }
    else
    {
      return left.Equals(right);
    }
  }

  public static bool operator !=(
    ProperValueTypeWithOneArgumentIdentity left, ProperValueTypeWithOneArgumentIdentity right)
  {
    return !Equals(left, right);
  }

  private readonly IEnumerable<int> _anArray;
  private readonly int _a;

  public ProperValueTypeWithOneArgumentIdentity(IEnumerable<int> anArray, int a)
  {
    _anArray = anArray;
    _a = a;
  }
}