using System;
using System.Collections.Generic;
using Core.Maybe;

namespace TddXt.XFluentAssert.TypeReflection.Interfaces;

internal interface IType
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