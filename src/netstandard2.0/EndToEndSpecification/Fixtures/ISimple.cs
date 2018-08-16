namespace TddXt.XFluentAssert.EndToEndSpecification.Fixtures
{
  using System;
  using System.Collections.Generic;

  public interface ISimple
  {
    int GetInt();
    string GetString();
    ISimple GetInterface();

    string GetStringProperty { get; }
    Type GetTypeProperty { get; }
    IEnumerable<ISimple> Simples { get; }
  }
}