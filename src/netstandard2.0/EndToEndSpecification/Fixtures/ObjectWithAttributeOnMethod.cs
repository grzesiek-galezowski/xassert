using System;
using System.ComponentModel;

namespace TddXt.XFluentAssert.EndToEndSpecification.Fixtures;

class AttributeFixture
{
  [Description("AnyCulture")]
  public object DecoratedMethod(int p1, int p2)
  {
    throw new NotImplementedException();
  }

  public object NonDecoratedMethod(int p1, int p2)
  {
    throw new NotImplementedException();
  }

}