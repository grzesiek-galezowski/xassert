using System;
using System.Collections.Generic;
using TddEbook.TddToolkit.CommonTypes;

namespace TypeReflection.Interfaces
{
  public interface IType2
  {
    bool HasPublicParameterlessConstructor();
    bool IsImplementationOfOpenGeneric(Type openGenericType);
    bool IsConcrete();
    IEnumerable<IFieldWrapper2> GetAllInstanceFields();
    IEnumerable<IFieldWrapper2> GetAllStaticFields();
    IEnumerable<IFieldWrapper2> GetAllConstants();
    IEnumerable<IPropertyWrapper> GetAllPublicInstanceProperties();
    Maybe<IConstructorWrapper2> PickConstructorWithLeastNonPointersParameters();
    IBinaryOperator Equality();
    IBinaryOperator Inequality();
    bool IsInterface();
    IEnumerable<IEventWrapper2> GetAllNonPublicEventsWithoutExplicitlyImplemented();
    IEnumerable<IConstructorWrapper2> GetAllPublicConstructors();
    IEnumerable<IFieldWrapper2> GetAllPublicInstanceFields();
    IEnumerable<IPropertyWrapper> GetPublicInstanceWritableProperties();
    IEnumerable<IMethod> GetAllPublicInstanceMethodsWithReturnValue();
    bool HasConstructorWithParameters();
    bool CanBeAssignedNullValue();
    Type ToClrType();
    bool IsException();
    bool HasPublicConstructorCountOfAtMost(int i);
    bool IsOpenGeneric(Type type);
  }
}