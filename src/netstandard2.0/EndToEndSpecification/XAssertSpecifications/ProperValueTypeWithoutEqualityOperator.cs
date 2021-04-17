using System;

namespace TddXt.XFluentAssert.EndToEndSpecification.XAssertSpecifications
{
  public sealed class ProperValueTypeWithoutEqualityOperator : IEquatable<ProperValueTypeWithoutEqualityOperator>
  {
    public bool Equals(ProperValueTypeWithoutEqualityOperator other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return _a == other._a;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((ProperValueTypeWithoutEqualityOperator)obj);
    }

    public override int GetHashCode()
    {
      return _a.GetHashCode();
    }

    private readonly int _a;

    public ProperValueTypeWithoutEqualityOperator(int a)
    {
      _a = a;
    }
  }
}