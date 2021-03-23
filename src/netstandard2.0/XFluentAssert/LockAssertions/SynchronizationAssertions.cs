using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;

using NSubstitute;
using NSubstitute.Exceptions;

using TddXt.AnyRoot;

namespace TddXt.XFluentAssert.LockAssertions
{
  public static class SynchronizationAssertions
  {
    private static void LockShouldBeReleasedWhenCallThrowsException<T>(
      ILockAssertions lockAssertions,
      T wrappingObject,
      T wrappedObjectMock,
      Action<T> callToCheck) where T : class
    {
      try
      {
        wrappedObjectMock.When(callToCheck).Do(_ =>
        {
          lockAssertions.AssertLocked();
          throw new LockNotReleasedWhenExceptionOccurs();
        });

        callToCheck(wrappingObject);

        throw new Exception( // todo more specific exception
          "The specified method was probably not called by the proxy with exactly the same arguments it received");
      }
      catch
      {
        lockAssertions.AssertUnlocked();
      }
      finally
      {
        wrappedObjectMock.ClearReceivedCalls();
      }
    }
    
    private static void LockShouldBeReleasedAfterACall<T>(
      T wrappingObject,
      T wrappedObjectMock,
      Action<T> callToCheck,
      ILockAssertions lockAssertions) where T : class
    {
      try
      {

        wrappedObjectMock.When(callToCheck).Do(_ => lockAssertions.AssertLocked());
        lockAssertions.AssertUnlocked();
        callToCheck(wrappingObject);
        lockAssertions.AssertUnlocked();
        callToCheck(wrappedObjectMock.Received(1));
      }
      finally
      {
        wrappedObjectMock.ClearReceivedCalls();
      }
    }

    private static async Task LockShouldBeReleasedWhenCallThrowsException<T>(
      ILockAssertions lockAssertions,
      T wrappingObject,
      T wrappedObjectMock,
      Func<T, Task> callToCheck) where T : class
    {
      try
      {
        wrappedObjectMock.When(callToCheck).Throw(_ =>
        {
            lockAssertions.AssertLocked();
            return new LockNotReleasedWhenExceptionOccurs();
        });

        await callToCheck(wrappingObject);

        throw new Exception( // todo more specific exception
          "The specified method was probably not called by the proxy with exactly the same arguments it received");
      }
      catch
      {
        lockAssertions.AssertUnlocked();
      }
      finally
      {
        wrappedObjectMock.ClearReceivedCalls();
      }
    }

    private static async Task LockShouldBeReleasedAfterACall<T>(
      T wrappingObject,
      T wrappedObjectMock,
      Func<T, Task> callToCheck,
      ILockAssertions lockAssertions)
      where T : class
    {
      try
      {
        callToCheck(wrappedObjectMock).Returns(async ci =>
        {
          lockAssertions.AssertLocked();
        });

        lockAssertions.AssertUnlocked();
        await callToCheck(wrappingObject);
        lockAssertions.AssertUnlocked();
      }
      finally
      {
        wrappedObjectMock.ClearReceivedCalls();
      }
    }



    private static void LockShouldBeReleasedAfterACall<T, TReturn>(
      T wrappingObject,
      T wrappedObjectMock,
      Func<T, TReturn> callToCheck,
      ILockAssertions lockAssertions)
      where T : class
    {
      try
      {
        Func<TReturn> instance = Root.Any.Instance<TReturn>;
        var cannedResult = instance();
        callToCheck(wrappedObjectMock).Returns(ci =>
        {
          lockAssertions.AssertLocked();
          return cannedResult;
        });

        lockAssertions.AssertUnlocked();
        var actualResult = callToCheck(wrappingObject);
        lockAssertions.AssertUnlocked();
        actualResult.Should().Be(
          cannedResult,
          "{0}",
          "The specified method was probably not called by the proxy with exactly the same arguments it received or it did not return the value obtained from wrapped call");
      }
      finally
      {
        wrappedObjectMock.ClearReceivedCalls();
      }
    }

    public static void Synchronizes<T>(T wrappingObject, Action<T> callToCheck, ILockAssertions lockAssertions,
      T wrappedObjectMock) where T : class
    {
      NSubstituteIsInCorrectVersion(wrappedObjectMock);
      LockShouldBeReleasedAfterACall(wrappingObject, wrappedObjectMock, callToCheck, lockAssertions);
      LockShouldBeReleasedWhenCallThrowsException(lockAssertions, wrappingObject, wrappedObjectMock, callToCheck);
    }

    public static void Synchronizes<T, TReturn>(T wrappingObject, Func<T, TReturn> callToCheck, ILockAssertions lockAssertions, T wrappedObjectMock) where T : class
    {
      NSubstituteIsInCorrectVersion(wrappedObjectMock);
      LockShouldBeReleasedAfterACall(wrappingObject, wrappedObjectMock, callToCheck, lockAssertions);
      LockShouldBeReleasedWhenCallThrowsException(lockAssertions, wrappingObject, wrappedObjectMock, t => callToCheck(t));
    }

    public static async Task SynchronizesAsync<T>(T wrappingObject, Func<T, Task> callToCheck, ILockAssertions lockAssertions, T wrappedObjectMock) where T : class
    {
      NSubstituteIsInCorrectVersion(wrappedObjectMock);
      await LockShouldBeReleasedAfterACall(wrappingObject, wrappedObjectMock, callToCheck, lockAssertions);
      await LockShouldBeReleasedWhenCallThrowsException(lockAssertions, wrappingObject, wrappedObjectMock, t => callToCheck(t));
    }

    private static void NSubstituteIsInCorrectVersion<T>(T wrappedObjectMock) where T : class
    {
      try
      {
        wrappedObjectMock.ClearReceivedCalls();
      }
      catch (NotASubstituteException e)
      {
        AssertReferencedAndLoadedVersionsOfNSubstituteAreTheSame();
        throw e;
      }
    }

    private static void AssertReferencedAndLoadedVersionsOfNSubstituteAreTheSame()
    {
      var checkedAssemblyName = "NSubstitute";
      ReferencedAndLoadedAssemblyVersionsAreTheSame(checkedAssemblyName);
    }

    private static void ReferencedAndLoadedAssemblyVersionsAreTheSame(string checkedAssemblyName)
    {
      foreach (var assemblyName in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
      {
        if (assemblyName.Name.StartsWith(checkedAssemblyName, StringComparison.InvariantCulture))
        {
          var referencedAssemblyInfo = Assembly.Load(assemblyName).GetName();
          var referencedAssemblyShortName = referencedAssemblyInfo.Name;

          if (referencedAssemblyShortName.Equals(checkedAssemblyName))
          {
            foreach (var loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
              var loadedAssemblyInfo = loadedAssembly.GetName();
              if (referencedAssemblyShortName.Equals(loadedAssemblyInfo.Name))
              {
                loadedAssemblyInfo.Version.Should().Be(referencedAssemblyInfo.Version,
                  "this is the version number of " + checkedAssemblyName + " assembly referenced internally by Tdd-Toolkit " +
                  "and it should match the version of assembly loaded at runtime (currently, this is not the case, " +
                  "which means your tests are using an external " + checkedAssemblyName + " dll with version different than the one needed by Tdd-Toolkit" +
                  " - please update your " + checkedAssemblyName + " assembly to version " + referencedAssemblyInfo.Version + ")");
              }
            }
          }
        }
      }
    }
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