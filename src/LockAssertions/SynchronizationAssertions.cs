using System;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Exceptions;
using static TddXt.AnyRoot.Root;

namespace LockAssertions
{
  public class SynchronizationAssertions
  {
    public static void LockShouldBeReleasedWhenCallThrowsException<T>(
      global::LockAssertions.LockAssertions lockAssertions,
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

    public static void LockShouldBeReleasedAfterACall<T>(
      T wrappingObject, 
      T wrappedObjectMock, 
      Action<T> callToCheck,
      LockAssertions lockAssertions) where T : class
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

    public static void LockShouldBeReleasedAfterACall<T, TReturn>(
      T wrappingObject, 
      T wrappedObjectMock,
      Func<T, TReturn> callToCheck,
      LockAssertions lockAssertions)
      where T : class
    {
      try
      {
        var cannedResult = Any.Instance<TReturn>();
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

    public static void Synchronizes<T>(T wrappingObject, Action<T> callToCheck, global::LockAssertions.LockAssertions lockAssertions,
      T wrappedObjectMock) where T : class
    {
      NSubstituteIsInCorrectVersion(wrappedObjectMock);
      SynchronizationAssertions.LockShouldBeReleasedAfterACall(wrappingObject, wrappedObjectMock, callToCheck, lockAssertions);
      SynchronizationAssertions.LockShouldBeReleasedWhenCallThrowsException(lockAssertions, wrappingObject, wrappedObjectMock, callToCheck);
    }

    public static void Synchronizes<T, TReturn>(T wrappingObject, Func<T, TReturn> callToCheck, global::LockAssertions.LockAssertions lockAssertions, T wrappedObjectMock) where T : class
    {
      NSubstituteIsInCorrectVersion(wrappedObjectMock);
      SynchronizationAssertions.LockShouldBeReleasedAfterACall(wrappingObject, wrappedObjectMock, callToCheck, lockAssertions);
      SynchronizationAssertions.LockShouldBeReleasedWhenCallThrowsException(lockAssertions, wrappingObject, wrappedObjectMock, t => callToCheck(t));
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

    public static void ReferencedAndLoadedAssemblyVersionsAreTheSame(string checkedAssemblyName)
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
                  "this is the version number of "+ checkedAssemblyName +" assembly referenced internally by Tdd-Toolkit " +
                  "and it should match the version of assembly loaded at runtime (currently, this is not the case, " +
                  "which means your tests are using an external "+ checkedAssemblyName + " dll with version different than the one needed by Tdd-Toolkit" +
                  " - please update your "+ checkedAssemblyName + " assembly to version "+ referencedAssemblyInfo.Version + ")");
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