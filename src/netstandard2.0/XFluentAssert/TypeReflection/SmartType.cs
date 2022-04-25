using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Core.Maybe;
using TddXt.XFluentAssert.ConstructorRetrieval;
using TddXt.XFluentAssert.TypeReflection.ImplementationDetails;
using TddXt.XFluentAssert.TypeReflection.Interfaces;

namespace TddXt.XFluentAssert.TypeReflection;

internal interface ISmartType : IType, IConstructorQueries
{
}

internal class SmartType : ISmartType
{
  private readonly Type _type;
  private readonly IConstructorRetrieval _constructorRetrieval;
  private readonly TypeInfo _typeInfo;

  public SmartType(Type type, IConstructorRetrieval constructorRetrieval)
  {
    _type = type;
    _constructorRetrieval = constructorRetrieval;
    _typeInfo = _type.GetTypeInfo();
  }

  public Maybe<ICreateObjects> GetNonPublicParameterlessConstructorInfo()
  {
    var constructorInfo = _type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
    if (constructorInfo != null)
    {
      return DefaultParameterlessConstructor.ForOrdinaryType(constructorInfo).Just();
    }
    else
    {
      return Maybe<ICreateObjects>.Nothing;
    }
  }

  public Maybe<ICreateObjects> GetPublicParameterlessConstructor()
  {

    var constructorInfo = _type.GetConstructor(
      BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
    if (constructorInfo == null)
    {
      return Maybe<ICreateObjects>.Nothing;
    }
    else
    {
      return DefaultParameterlessConstructor.ForOrdinaryType(constructorInfo).Just();
    }
  }

  public IEnumerable<IAmField> GetAllInstanceFields()
  {
    var fields = _typeInfo.GetFields(
      BindingFlags.Instance
      | BindingFlags.Public
      | BindingFlags.NonPublic);
    return fields.Select(f => new Field(f));
  }

  public IEnumerable<IAmField> GetAllStaticFields()
  {
    //bug first convert to field wrappers and then ask questions, not the other way round.
    //bug GetAllFields() should return field wrappers
    return GetAllFields(_type).Where(fieldInfo =>
        fieldInfo.IsStatic &&
        !new Field(fieldInfo).IsConstant() &&
        !IsCompilerGenerated(fieldInfo) &&
        !IsDelegate(fieldInfo.FieldType))
      .Select(f => new Field(f));
  }

  public IEnumerable<IAmField> GetAllConstants()
  {
    return GetAllFields(_type).Select(f => new Field(f)).Where(f => f.IsConstant());
  }

  public IEnumerable<IAmProperty> GetAllPublicInstanceProperties()
  {
    var properties = _typeInfo.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    return properties.Select(p => new ImplementationDetails.Property(p));
  }

  public Maybe<ICreateObjects> PickConstructorWithLeastNonPointersParameters()
  {
    ICreateObjects leastParamsConstructor = null;

    var constructors = For(_type).GetAllPublicConstructors();
    var numberOfParams = int.MaxValue;

    foreach (var typeConstructor in constructors)
    {
      if (
        typeConstructor.HasNonPointerArgumentsOnly()
        && typeConstructor.HasLessParametersThan(numberOfParams))
      {
        leastParamsConstructor = typeConstructor;
        numberOfParams = typeConstructor.GetParametersCount();
      }
    }

    return leastParamsConstructor!.ToMaybe();
  }

  private const string OpEquality = "op_Equality";
  private const string OpInequality = "op_Inequality";

  private Maybe<MethodInfo> EqualityOpMethod()
  {
    var equality = _typeInfo.GetMethod(OpEquality,
      BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static);

    return equality.ToMaybe();
  }

  private Maybe<MethodInfo> InequalityOpMethod()
  {
    var inequality = _typeInfo.GetMethod(OpInequality,
      BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static);

    return inequality.ToMaybe();
  }

  private Maybe<MethodInfo> ValueTypeEqualityMethod()
  {
    return _typeInfo.IsValueType ?
      GetType().GetTypeInfo().GetMethod(nameof(ValuesEqual)).Just()
      : Maybe<MethodInfo>.Nothing;

  }

  private Maybe<MethodInfo> ValueTypeInequalityMethod()
  {
    return _typeInfo.IsValueType ?
      GetType().GetTypeInfo().GetMethod(nameof(ValuesNotEqual)).Just()
      : Maybe<MethodInfo>.Nothing;
  }

  public static bool ValuesEqual(object instance1, object instance2)
  {
    return Equals(instance1, instance2);
  }

  public static bool ValuesNotEqual(object instance1, object instance2)
  {
    return !Equals(instance1, instance2);
  }

  public IAmBinaryOperator EqualityOperator()
  {
    return BinaryOperator.Wrap(EqualityOpMethod(), ValueTypeEqualityMethod(), "operator ==");
  }

  public Maybe<IAmBinaryOperator> EquatableEquality()
  {
    var equatableInterfaces =
      _type.GetInterfaces()
        .Where(iface => iface.IsGenericType)
        .Where(iface => iface.GetGenericTypeDefinition() == typeof(IEquatable<>));

    if (equatableInterfaces.Any())
    {
      var equatableInterface = equatableInterfaces.Single();
      var equalsMethod = equatableInterface.GetMethods().Single();
      return new BinaryInstanceOperation(equalsMethod).Just<IAmBinaryOperator>();
    }
    else
    {
      return Maybe<IAmBinaryOperator>.Nothing;
    }
  }

  public IAmBinaryOperator InequalityOperator()
  {
    return BinaryOperator.Wrap(InequalityOpMethod(), ValueTypeInequalityMethod(), "operator !=");
  }

  public static ISmartType For(Type type)
  {
    return new SmartType(type, new ConstructorRetrievalFactory().Create());
  }

  public static ISmartType ForTypeOf(object obj)
  {
    return new SmartType(obj.GetType(), new ConstructorRetrievalFactory().Create());
  }

  private static bool IsCompilerGenerated(FieldInfo fieldInfo) //?? should it be defined on a type?
  {
    return fieldInfo.FieldType.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute), false);
  }

  private static IEnumerable<FieldInfo> GetAllFields(Type type)
  {
    return type.GetTypeInfo().GetNestedTypes().SelectMany(GetAllFields)
      .Concat(type.GetTypeInfo().GetFields(
        BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Static
        | BindingFlags.DeclaredOnly));
  }

  private static bool IsDelegate(Type type)
  {
    return typeof(MulticastDelegate).GetTypeInfo().IsAssignableFrom(
      type.GetTypeInfo().BaseType);
  }

  public IEnumerable<IAmEvent> GetAllNonPublicEventsWithoutExplicitlyImplemented()
  {
    return _typeInfo.GetEvents(
        BindingFlags.NonPublic
        | BindingFlags.Instance
        | BindingFlags.DeclaredOnly)
      .Where(IsNotExplicitlyImplemented)
      .Select(e => new Event(e));
  }

  public IEnumerable<IAmEvent> GetAllEvents()
  {
    return _typeInfo.GetEvents(
        BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Instance
        | BindingFlags.Static
        | BindingFlags.DeclaredOnly)
      .Select(e => new Event(e));
  }

  private static bool IsNotExplicitlyImplemented(EventInfo eventInfo)
  {
    var eventDeclaringType = eventInfo.DeclaringType;
    if (eventDeclaringType != null)
    {
      var interfaces = eventDeclaringType.GetTypeInfo().GetInterfaces();
      foreach (var @interface in interfaces)
      {
        var methodsImplementedInInterface = eventDeclaringType
          .GetInterfaceMap(@interface).TargetMethods;
        var addMethod = eventInfo.GetAddMethod(true);
        if (methodsImplementedInInterface.Where(m => m.IsPrivate).Contains(addMethod))
        {
          return false;
        }
      }
    }
    return true;
  }

  public IEnumerable<ICreateObjects> GetAllPublicConstructors()
  {
    return _constructorRetrieval.RetrieveFrom(this);
  }

  public List<ICreateObjects> GetInternalConstructorsWithoutRecursiveParameters()
  {
    return GetInternalConstructors().Where(c => c.IsNotRecursive()).ToList();
  }

  private List<ICreateObjects> GetInternalConstructors()
  {
    var constructorInfos = _typeInfo.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
    var enumerable = constructorInfos.Where(CreationMethod.IsInternal);

    var wrappers = enumerable.Select(c => (ICreateObjects)CreationMethod.FromConstructorInfo(c)).ToList();
    return wrappers;
  }

  public List<CreationMethod> TryToObtainPublicConstructors()
  {
    return _typeInfo.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
      .Select(c => CreationMethod.FromConstructorInfo(c)).ToList();
  }

  public IEnumerable<ICreateObjects> TryToObtainPublicConstructorsWithoutRecursiveArguments()
  {
    return TryToObtainPublicConstructors().Where(c => c.IsNotRecursive());
  }

  public IEnumerable<ICreateObjects> TryToObtainPublicConstructorsWithRecursiveArguments()
  {
    return TryToObtainPublicConstructors().Where(c => c.IsRecursive());
  }

  public IEnumerable<ICreateObjects> TryToObtainInternalConstructorsWithRecursiveArguments()
  {
    return GetInternalConstructors().Where(c => c.IsRecursive()).ToList();
  }

  public IEnumerable<ICreateObjects> TryToObtainPrimitiveTypeConstructor()
  {
    return DefaultParameterlessConstructor.ForValue(_type);
  }

  public IEnumerable<ICreateObjects> TryToObtainPublicStaticFactoryMethodWithoutRecursion()
  {
    return _typeInfo.GetMethods(BindingFlags.Static | BindingFlags.Public)
      .Where(m => !m.IsSpecialName)
      .Where(IsNotImplicitCast)
      .Where(IsNotExplicitCast)
      .Select(CreationMethod.FromStaticMethodInfo)
      .Where(c => c.IsFactoryMethod());
  }

  public bool HasConstructorWithParameters()
  {
    return _typeInfo.IsPrimitive;
  }

  public bool CanBeAssignedNullValue()
  {
    return !_typeInfo.IsValueType && !_typeInfo.IsPrimitive;
  }

  public Type ToClrType()
  {
    return _type; //todo at the very end, this should be removed
  }

  public bool IsException()
  {
    return _type == typeof(Exception) ||
           _typeInfo.IsSubclassOf(typeof(Exception));
  }

  private static bool IsNotExplicitCast(MethodInfo mi)
  {
    return !string.Equals(mi.Name, "op_Explicit", StringComparison.Ordinal);
  }

  private static bool IsNotImplicitCast(MethodInfo mi)
  {
    return !string.Equals(mi.Name, "op_Implicit", StringComparison.Ordinal);
  }
}