﻿namespace AssertionConstraints
{
  public interface IConstraintsViolations
  {
    void AssertNone();
    void Add(string violationDetails);
  }
}