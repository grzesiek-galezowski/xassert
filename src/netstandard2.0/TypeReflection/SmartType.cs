namespace TddXt.XFluentAssert.TypeReflection
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Runtime.CompilerServices;

  using TddXt.XFluentAssert.CommonTypes;
  using TddXt.XFluentAssert.ConstructorRetrieval;
  using TddXt.XFluentAssert.TypeReflection.ImplementationDetails;
  using TddXt.XFluentAssert.TypeReflection.Interfaces;

  public interface ISmartType : IType, IConstructorQueries
  {
  }

  public class SmartType : ISmartType
  {
    private readonly Type _type;
    private readonly ConstructorRetrieval _constructorRetrieval;
    private readonly TypeInfo _typeInfo;

    public SmartType(Type type, ConstructorRetrieval constructorRetrieval)
    {
      this._type = type;
      this._constructorRetrieval = constructorRetrieval;
      this._typeInfo = this._type.GetTypeInfo();
    }

    public bool HasPublicParameterlessConstructor()
    {
      return this.GetPublicParameterlessConstructor().HasValue || this._typeInfo.IsPrimitive || this._typeInfo.IsAbstract;
    }

    public Maybe<ICreateObjects> GetNonPublicParameterlessConstructorInfo()
    {
      var constructorInfo = this._type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
      if (constructorInfo != null)
      {
        return Maybe.Just(DefaultParameterlessConstructor.ForOrdinaryType(constructorInfo));
      }
      else
      {
        return Maybe<ICreateObjects>.Nothing;
      }
    }

    public Maybe<ICreateObjects> GetPublicParameterlessConstructor()
    {

      var constructorInfo = this._type.GetConstructor(
        BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
      if (constructorInfo == null)
      {
        return Maybe<ICreateObjects>.Nothing;
      }
      else
      {
        return Maybe.Just(DefaultParameterlessConstructor.ForOrdinaryType(constructorInfo));
      }
    }

    public bool IsImplementationOfOpenGeneric(Type openGenericType)
    {
      return this._typeInfo.GetInterfaces().Any(
        ifaceType => IsOpenGeneric(ifaceType, openGenericType));
    }

    private static bool IsOpenGeneric(Type checkedType, Type openGenericType)
    {
      return checkedType.GetTypeInfo().IsGenericType && 
             checkedType.GetGenericTypeDefinition() == openGenericType;
    }

    public bool IsConcrete()
    {
      return !this._typeInfo.IsAbstract && !this._typeInfo.IsInterface;
    }

    public IEnumerable<IAmField> GetAllInstanceFields()
    {
      var fields = this._typeInfo.GetFields(
        BindingFlags.Instance 
        | BindingFlags.Public 
        | BindingFlags.NonPublic);
      return fields.Select(f => new Field(f));
    }

    public IEnumerable<IAmField> GetAllStaticFields()
    {
      //bug first convert to field wrappers and then ask questions, not the other way round.
      //bug GetAllFields() should return field wrappers
      return GetAllFields(this._type).Where(fieldInfo =>
                                       fieldInfo.IsStatic &&
                                       !new Field(fieldInfo).IsConstant() &&
                                       !IsCompilerGenerated(fieldInfo) &&
                                       !IsDelegate(fieldInfo.FieldType))
                                .Select(f => new Field(f));
    }

    public IEnumerable<IAmField> GetAllConstants()
    {
      return GetAllFields(this._type).Select(f => new Field(f)).Where(f => f.IsConstant());
    }

    public IEnumerable<IAmProperty> GetAllPublicInstanceProperties()
    {
      var properties = this._typeInfo.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      return properties.Select(p => new ImplementationDetails.Property(p));
    }

    public Maybe<ICreateObjects> PickConstructorWithLeastNonPointersParameters()
    {
      ICreateObjects leastParamsConstructor = null;

      var constructors = For(this._type).GetAllPublicConstructors();
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

      return Maybe.FromNullable(leastParamsConstructor);
    }

    private const string OpEquality = "op_Equality";
    private const string OpInequality = "op_Inequality";

    private Maybe<MethodInfo> EqualityMethod()
    {
      var equality = this._typeInfo.GetMethod(OpEquality);

      return equality == null ? Maybe<MethodInfo>.Nothing : new Maybe<MethodInfo>(equality);
    }

    private Maybe<MethodInfo> InequalityMethod()
    {
      var inequality = this._typeInfo.GetMethod(OpInequality);

      return inequality == null ? Maybe<MethodInfo>.Nothing : new Maybe<MethodInfo>(inequality);
    }

    private Maybe<MethodInfo> ValueTypeEqualityMethod()
    {
      return this._typeInfo.IsValueType ?
               Maybe.Just(this.GetType().GetTypeInfo().GetMethod(nameof(ValuesEqual)))
               : Maybe<MethodInfo>.Nothing;

    }

    private Maybe<MethodInfo> ValueTypeInequalityMethod()
    {
      return this._typeInfo.IsValueType ?
               Maybe.Just(this.GetType().GetTypeInfo().GetMethod(nameof(ValuesNotEqual))) 
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

    public IAmBinaryOperator Equality()
    {
      return BinaryOperator.Wrap(this.EqualityMethod(), this.ValueTypeEqualityMethod(), "operator ==");
    }

    public IAmBinaryOperator Inequality()
    {
      return BinaryOperator.Wrap(this.InequalityMethod(), this.ValueTypeInequalityMethod(), "operator !=");
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
      return this._typeInfo.GetEvents(
        BindingFlags.NonPublic 
        | BindingFlags.Instance
        | BindingFlags.DeclaredOnly)
                  .Where(IsNotExplicitlyImplemented)
                  .Select(e => new Event(e));
    }

    public IEnumerable<IAmEvent> GetAllEvents()
    {
      return this._typeInfo.GetEvents(
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
      return this._constructorRetrieval.RetrieveFrom(this);
    }

    public List<ICreateObjects> GetInternalConstructorsWithoutRecursiveParameters()
    {
      return this.GetInternalConstructors().Where(c => c.IsNotRecursive()).ToList();
    }

    private List<ICreateObjects> GetInternalConstructors()
    {
      var constructorInfos = this._typeInfo.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
      var enumerable = constructorInfos.Where(CreationMethod.IsInternal);

      var wrappers = enumerable.Select(c => (ICreateObjects) CreationMethod.FromConstructorInfo(c)).ToList();
      return wrappers;
    }

    public List<CreationMethod> TryToObtainPublicConstructors()
    {
      return this._typeInfo.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
        .Select(c => CreationMethod.FromConstructorInfo(c)).ToList();
    }

    public IEnumerable<ICreateObjects> TryToObtainPublicConstructorsWithoutRecursiveArguments()
    {
      return this.TryToObtainPublicConstructors().Where(c => c.IsNotRecursive());
    }

    public IEnumerable<ICreateObjects> TryToObtainPublicConstructorsWithRecursiveArguments()
    {
      return this.TryToObtainPublicConstructors().Where(c => c.IsRecursive());
    }

    public IEnumerable<ICreateObjects> TryToObtainInternalConstructorsWithRecursiveArguments()
    {
      return this.GetInternalConstructors().Where(c => c.IsRecursive()).ToList();
    }

    public IEnumerable<ICreateObjects> TryToObtainPrimitiveTypeConstructor()
    {
      return DefaultParameterlessConstructor.ForValue(this._type);
    }

    public IEnumerable<ICreateObjects> TryToObtainPublicStaticFactoryMethodWithoutRecursion()
    {
      return this._typeInfo.GetMethods(BindingFlags.Static | BindingFlags.Public)
        .Where(m => !m.IsSpecialName)
        .Where(IsNotImplicitCast)
        .Where(IsNotExplicitCast)
        .Select(CreationMethod.FromStaticMethodInfo)
        .Where(c => c.IsFactoryMethod());
    }

    public bool HasConstructorWithParameters()
    {
      return this._typeInfo.IsPrimitive;
    }

    public bool CanBeAssignedNullValue()
    {
      return !this._typeInfo.IsValueType && !this._typeInfo.IsPrimitive;
    }

    public Type ToClrType()
    {
      return this._type; //todo at the very end, this should be removed
    }

    public bool IsException()
    {
      return this._type == typeof(Exception) ||
        this._typeInfo.IsSubclassOf(typeof(Exception));
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

}