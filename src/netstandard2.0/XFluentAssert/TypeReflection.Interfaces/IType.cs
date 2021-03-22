using System;
using System.Collections.Generic;

using TddXt.XFluentAssert.CommonTypes;

namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
  public interface IType
  {
    IEnumerable<IAmField> GetAllInstanceFields();

    IEnumerable<IAmField> GetAllStaticFields();

    IEnumerable<IAmField> GetAllConstants();

    IEnumerable<IAmProperty> GetAllPublicInstanceProperties();

    Maybe<ICreateObjects> PickConstructorWithLeastNonPointersParameters();

    IAmBinaryOperator EqualityOperator();

    IAmBinaryOperator InequalityOperator();

    IEnumerable<IAmEvent> GetAllNonPublicEventsWithoutExplicitlyImplemented();

    IEnumerable<ICreateObjects> GetAllPublicConstructors();

    bool HasConstructorWithParameters();

    bool CanBeAssignedNullValue();

    Type ToClrType();

    bool IsException();

    IEnumerable<IAmEvent> GetAllEvents();

    Maybe<IAmBinaryOperator> EquatableEquality();
  }

}