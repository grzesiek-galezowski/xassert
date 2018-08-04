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
    IEnumerable<IFieldWrapper> GetAllInstanceFields();
    IEnumerable<IFieldWrapper> GetAllStaticFields();
    IEnumerable<IFieldWrapper> GetAllConstants();
    IEnumerable<IPropertyWrapper> GetAllPublicInstanceProperties();
    Maybe<ICreateObjects> PickConstructorWithLeastNonPointersParameters();
    IAmBinaryOperator Equality();
    IAmBinaryOperator Inequality();
    IEnumerable<IEventWrapper> GetAllNonPublicEventsWithoutExplicitlyImplemented();
    IEnumerable<ICreateObjects> GetAllPublicConstructors();
    bool HasConstructorWithParameters();
    bool CanBeAssignedNullValue();
    Type ToClrType();
    bool IsException();
  }
}