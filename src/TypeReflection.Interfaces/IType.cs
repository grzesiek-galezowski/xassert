using System;
using System.Collections.Generic;
using TddEbook.TddToolkit.CommonTypes;

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
    Maybe<IConstructorWrapper> PickConstructorWithLeastNonPointersParameters();
    IBinaryOperator Equality();
    IBinaryOperator Inequality();
    IEnumerable<IEventWrapper2> GetAllNonPublicEventsWithoutExplicitlyImplemented();
    IEnumerable<IConstructorWrapper> GetAllPublicConstructors();
    bool HasConstructorWithParameters();
    bool CanBeAssignedNullValue();
    Type ToClrType();
    bool IsException();
  }
}