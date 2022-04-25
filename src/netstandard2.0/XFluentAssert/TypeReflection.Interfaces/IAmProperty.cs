namespace TddXt.XFluentAssert.TypeReflection.Interfaces;

internal interface IAmProperty
{
  bool HasPublicSetter();
  string ShouldNotBeMutableButIs();
}