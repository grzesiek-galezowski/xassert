using System;

namespace TddXt.XFluentAssert.TypeReflection.ImplementationDetails;

internal class DuplicateConstantException(string message) : Exception(message);