using TddXt.XFluentAssert.AssertionConstraints;
using TddXt.XFluentAssert.EqualityAssertions;
using TddXt.XFluentAssert.EqualityAssertions.EqualityOperator;
using TddXt.XFluentAssert.EqualityAssertions.InequalityOperator;
using System;
using System.Collections.Generic;

namespace TddXt.XFluentAssert.ValueObjectConstraints
{
  public class AssertionConstraints
  {
    //todo move elsewhere (but has troublesome dependency on ValueTraits which is public...)
    public static IEnumerable<IConstraint> ForValueSemantics<T>(Type type, Func<T>[] equalInstances,
      Func<T>[] otherInstances,
      IKnowWhatValueTraitsToCheck traits)
    {
      var constraints = new List<IConstraint>();
      constraints.Add(new HasToBeAConcreteClass(type));

      if (traits.RequireAllFieldsReadOnly)
      {
        constraints.Add(new AllFieldsMustBeReadOnly(type));
      }

      //bug add sealed constraint
      constraints.Add(new ThereMustBeNoPublicPropertySetters(type));
      constraints.Add(new MustBeSealed(type));

      constraints.Add(new StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualsMethod<T>(equalInstances, otherInstances));
      constraints.Add(new StateBasedEqualityMustBeImplementedInTermsOfEqualsMethod<T>(equalInstances));

      constraints.Add(new StateBasedInequalityMustBeImplementedInTermsOfEqualsMethod<T>(equalInstances, otherInstances));

      constraints.Add(new HashCodeMustBeTheSameForSameObjectsAndDifferentForDifferentObjects<T>(equalInstances, otherInstances));

      if (traits.RequireSafeInequalityToNull)
      {
        constraints.Add(new InequalityWithNullMustBeImplementedInTermsOfEqualsMethod<T>(equalInstances, otherInstances));
      }

      if (traits.RequireEqualityAndInequalityOperatorImplementation)
      {
        //equality operator
        constraints.Add(new StateBasedEqualityShouldBeAvailableInTermsOfEqualityOperator(type));
        constraints.Add(new StateBasedEqualityMustBeImplementedInTermsOfEqualityOperator<T>(equalInstances));
        constraints.Add(new StateBasedEqualityWithItselfMustBeImplementedInTermsOfEqualityOperator<T>(equalInstances, otherInstances));
        constraints.Add(new StateBasedInequalityMustBeImplementedInTermsOfEqualityOperator<T>(equalInstances, otherInstances));
        constraints.Add(new InequalityWithNullMustBeImplementedInTermsOfEqualityOperator<T>(equalInstances, otherInstances));

        //inequality operator
        constraints.Add(new StateBasedEqualityShouldBeAvailableInTermsOfInequalityOperator(type));
        constraints.Add(new StateBasedEqualityMustBeImplementedInTermsOfInequalityOperator<T>(equalInstances));
        constraints.Add(new StateBasedEqualityWithItselfMustBeImplementedInTermsOfInequalityOperator<T>(equalInstances, otherInstances));
        constraints.Add(new StateBasedInequalityMustBeImplementedInTermsOfInequalityOperator<T>(equalInstances, otherInstances));
        constraints.Add(new InequalityWithNullMustBeImplementedInTermsOfInequalityOperator<T>(equalInstances, otherInstances));
      }
      return constraints;
    }
  }
}
