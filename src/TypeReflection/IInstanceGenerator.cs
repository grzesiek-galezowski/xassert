﻿using System;

namespace TddEbook.TypeReflection
{
  public interface IInstanceGenerator
  {
    T InstanceOf<T>();
    T Instance<T>();
    T OtherThan<T>(params T[] omittedValues);
    object Instance(Type type);
    T Dummy<T>();
  }
}