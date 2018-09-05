using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  public class ObjectGraphPaths
  {
    private readonly List<ObjectGraphPath> _paths = new List<ObjectGraphPath>();

    public void Add(ObjectGraphPath objectGraphPath)
    {
      _paths.Add(objectGraphPath);
    }

    public void AssertContainNonRootObjectOf(Type type)
    {
      if (!FindAllPathsContainingNonRootInstanceOf(type).Any())
      {
        Services.ThrowException("Could not find " + type + " anywhere in dependency graph"); //TODO
      }
    }

    private IReadOnlyCollection<ObjectGraphPath> FindAllPathsContainingNonRootInstanceOf(Type type)
    {
      return _paths.Where(p => p.ContainsNonRootInstanceOf(type)).ToList();
    }

    public void AssertContainNonRoot<T>(T value)
    {
      foreach (var path in _paths)
      {
        if (path.ContainsNonRootValueEqualTo(value))
        {
          return;
        }
      }

      Services.ThrowException(WithMessageThatCouldNotFind(value, 
        FindAllPathsContainingNonRootInstanceOf(typeof(T)))); //TODO

    }

    private string WithMessageThatCouldNotFind<T>(T value, IReadOnlyCollection<ObjectGraphPath> pathsWithInstanceOfType)
    {
      var message = "Could not find the particular instance: " + value +
                    " anywhere in dependency graph";
      if (pathsWithInstanceOfType.Any())
      {
        message += " however, another instance of this type was found within the following paths: " +
                   Environment.NewLine + AsString(pathsWithInstanceOfType);
      }
      else
      {
        message += ". Paths created when searching: " + Environment.NewLine + AsString(_paths);
      }

      return message;
    }

    private static string AsString(IEnumerable<ObjectGraphPath> pathsWithInstanceOfType)
    {
      return string.Join("\n", pathsWithInstanceOfType.Select(s => s.ToString()));
    }


    //todo report error - make generic method non-static sometimes doesn't work
  }
}