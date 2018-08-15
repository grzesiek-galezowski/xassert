using System;

namespace TddEbook.TddToolkitSpecification.Fixtures
{
  using System.ComponentModel;

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
}
