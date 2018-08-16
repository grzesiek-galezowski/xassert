﻿namespace TddXt.XFluentAssert.Root.ConstraintsAssertions
{
  using System;
  using System.Collections.Generic;

  using TddXt.XFluentAssert.AssertionConstraints;
  using TddXt.XFluentAssert.EqualityAssertions;
  using TddXt.XFluentAssert.EqualityAssertions.EqualityOperator;
  using TddXt.XFluentAssert.EqualityAssertions.InequalityOperator;
  using TddXt.XFluentAssert.Root.ValueAssertions;
  using TddXt.XFluentAssert.ValueActivation;
  using TddXt.XFluentAssert.ValueObjectConstraints;

  public partial class AssertionConstraints
  {
    //bug move elsewhere
    public static IEnumerable<IConstraint> ForValueSemantics(
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
        traits.IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify.ToArray()));
      
      constraints.Add(new HashCodeMustBeTheSameForSameObjectsAndDifferentForDifferentObjects(activator,
        traits.IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify.ToArray()));

      if(traits.RequireSafeInequalityToNull)
      {
        constraints.Add(new UnEqualityWithNullMustBeImplementedInTermsOfEqualsMethod(activator));
      }


      if (traits.RequireEqualityAndInequalityOperatorImplementation)
      {
        //equality operator
        constraints.Add(new StateBasedEqualityShouldBeAvailableInTermsOfEqualityOperator(type));
        constraints.Add(new StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator(activator));
        constraints.Add(new StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator(activator));
        constraints.Add(new StateBasedUnEqualityMustBeImplementedInTermsOfEqualityOperator(activator,
          traits.IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify.ToArray()));
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
          traits.IndexesOfConstructorArgumentsIndexesThatDoNotConstituteAValueIdentify.ToArray()));
        constraints.Add(new InequalityWithNullMustBeImplementedInTermsOfInequalityOperator(
          activator));
      

      }
      return constraints;
    }
  }
}
