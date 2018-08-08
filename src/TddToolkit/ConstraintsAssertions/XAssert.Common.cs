using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssertionConstraints;
using EqualsAssertions;
using EqualsAssertions.EqualityOperator;
using EqualsAssertions.InequalityOperator;
using TypeReflection;
using ValueActivation;
using ValueObjectConstraints;

namespace TddEbook.TddToolkit
{
  public partial class XAssert
  {
    public static void IsValue<T>()
    {
      IsValue<T>(ValueTypeTraits.Default());
    }

    public static void HasNullProtectedConstructors<T>()
    {
      var type = SmartType.For(typeof(T));
      
      if (!type.HasConstructorWithParameters())
      {
        var constraints = new List<IConstraint> { new ConstructorsMustBeNullProtected(type)};
        AssertionConstraintsEngine.TypeAdheresTo(constraints);
      }
    }

    public static void IsValue<T>(ValueTypeTraits traits)
    {
      if (!ValueObjectWhiteList.Contains<T>())
      {
        var activator = ValueObjectActivator.FreshInstance(typeof (T));
        var constraints = CreateConstraintsBasedOn(typeof (T), traits, activator);
        AssertionConstraintsEngine.TypeAdheresTo(constraints);
      }
    }

    private static IEnumerable<IConstraint> CreateConstraintsBasedOn(
      Type type, ValueTypeTraits traits, ValueObjectActivator activator)
    {

      var constraints = new List<IConstraint>();

      constraints.Add(new HasToBeAConcreteClass(type));

      if(traits.RequireAllFieldsReadOnly)
      {
        constraints.Add(new AllFieldsMustBeReadOnly(type));
      }

      constraints.Add(new ThereMustBeNoPublicPropertySetters(type));
      constraints.Add(new StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualsMethod(activator));
      constraints.Add(new StateBasedEqualityMustBeImplementedInTermsOfEqualsMethod(activator));

      constraints.Add(new StateBasedUnEqualityMustBeImplementedInTermsOfEqualsMethod(activator, 
        traits.IndexesOfConstructorArgumentsIndexesThatDoNotContituteAValueIdentify.ToArray()));
      
      constraints.Add(new HashCodeMustBeTheSameForSameObjectsAndDifferentForDifferentObjects(activator,
        traits.IndexesOfConstructorArgumentsIndexesThatDoNotContituteAValueIdentify.ToArray()));

      if(traits.RequireSafeUnequalityToNull)
      {
        constraints.Add(new UnEqualityWithNullMustBeImplementedInTermsOfEqualsMethod(activator));
      }


      if (traits.RequireEqualityAndUnequalityOperatorImplementation)
      {
        //equality operator
        constraints.Add(new StateBasedEqualityShouldBeAvailableInTermsOfEqualityOperator(type));
        constraints.Add(new StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator(activator));
        constraints.Add(new StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator(activator));
        constraints.Add(new StateBasedUnEqualityMustBeImplementedInTermsOfEqualityOperator(activator,
          traits.IndexesOfConstructorArgumentsIndexesThatDoNotContituteAValueIdentify.ToArray()));
        constraints.Add(new InequalityWithNullMustBeImplementedInTermsOfEqualityOperator(
          activator));

        //inequality operator
        constraints.Add(new StateBasedEqualityShouldBeAvailableInTermsOfInequalityOperator(
          type));
        constraints.Add(new StateBasedEqualityMustBeImplementedInTermsOfInequalityOperator(
          activator));
        constraints.Add(new StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator(
          activator));
        constraints.Add(new StateBasedUnEqualityMustBeImplementedInTermsOfInequalityOperator(
          activator,
          traits.IndexesOfConstructorArgumentsIndexesThatDoNotContituteAValueIdentify.ToArray()));
        constraints.Add(new InequalityWithNullMustBeImplementedInTermsOfInequalityOperator(
          activator));
      

      }
      return constraints;
    }

    public static void IsValue(Type type)
    {
      InvokeGenericVersionOfMethod(type, MethodBase.GetCurrentMethod().Name);
    }

    private static void InvokeGenericVersionOfMethod(Type type, string name)
    {
      var methodInfos = typeof(XAssert).GetMethods();
      var methodsByGivenName = methodInfos.Where(m => m.Name == name);
      var firstParameterlessVersion = methodsByGivenName.First(m => m.GetParameters().Length == 0);
      var genericMethod = firstParameterlessVersion.MakeGenericMethod(type);
      genericMethod.Invoke(null, null);
    }
  }
}
