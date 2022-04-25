namespace TddXt.XFluentAssert.Api.ValueAssertions;

public interface IKnowWhatValueTraitsToCheck
{
  bool RequireEqualityAndInequalityOperatorImplementation
  {
    get;
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedMemberInSuper.Global
    set;
  }

  bool RequireSafeInequalityToNull
  {
    get;
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable once UnusedMemberInSuper.Global
    set;
  }

  bool RequireAllFieldsReadOnly
  {
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    get;
    // ReSharper disable once UnusedMemberInSuper.Global
    set;
  }
}