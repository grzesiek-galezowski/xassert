using System;
using System.Collections.Generic;
using CommonTypes;

namespace TypeReflection.Interfaces
{
  public interface IType
  {
    bool HasPublicParameterlessConstructor();
    bool IsImplementationOfOpenGeneric(Type openGenericType);
    bool IsConcrete();
    IEnumerable<IAmField> GetAllInstanceFields();
    IEnumerable<IAmField> GetAllStaticFields();
    IEnumerable<IAmField> GetAllConstants();
    IEnumerable<IAmProperty> GetAllPublicInstanceProperties();
    Maybe<ICreateObjects> PickConstructorWithLeastNonPointersParameters();
    IAmBinaryOperator Equality();
    IAmBinaryOperator Inequality();
    IEnumerable<IAmEvent> GetAllNonPublicEventsWithoutExplicitlyImplemented();
    IEnumerable<ICreateObjects> GetAllPublicConstructors();
    bool HasConstructorWithParameters();
    bool CanBeAssignedNullValue();
    Type ToClrType();
    bool IsException();
  }
}