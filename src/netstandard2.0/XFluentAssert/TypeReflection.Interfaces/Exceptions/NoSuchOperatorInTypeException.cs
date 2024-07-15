using System;

namespace TddXt.XFluentAssert.TypeReflection.Interfaces.Exceptions;

internal class NoSuchOperatorInTypeException(string s) : Exception(s);