using System;
using TddXt.AnyRoot;

namespace TddEbook.TddToolkit
{
  public partial class XAssert
  {
  }

  internal class LockNotReleasedWhenExceptionOccurs : Exception
  {
    public LockNotReleasedWhenExceptionOccurs() 
      : base("Although the synchronized object threw an exception, the lock was not released. "
      + "There's probably a try-finally missing in your synchronizing proxy where the lock would be released in the `finally` block")
    {
    }
  }
}