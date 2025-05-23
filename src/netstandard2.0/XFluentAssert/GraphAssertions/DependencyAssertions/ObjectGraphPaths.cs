﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using static System.Environment;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions;

internal class ObjectGraphPaths
{
  private readonly List<ObjectGraphPath> _paths = new();

  public void Add(ObjectGraphPath objectGraphPath)
  {
    _paths.Add(objectGraphPath);
  }

  public void AssertContainNonRootObjectOf(Type type)
  {
    if (!FindAllPathsContainingNonRootInstanceOf(type).Any())
    {
      AssertionChain.GetOrCreate().FailWith("Could not find " + type + " anywhere in dependency graph");
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

    AssertionChain.GetOrCreate().FailWith(WithMessageThatCouldNotFind(value,
      FindAllPathsContainingNonRootInstanceOf(typeof(T)))); //TODO

  }
  public void AssertContainNonRootSubPath(object[] values)
  {
    foreach (var path in _paths)
    {
      if (path.ContainsNonRootSubpath(values))
      {
        return;
      }
    }

    AssertionChain.GetOrCreate().FailWith(WithMessageThatCouldNotFindMultiple(values, _paths));
  }

  public void AssertContainNonRootTypeSubPath(Type[] types)
  {
    foreach (var path in _paths)
    {
      if (path.ContainsNonRootTypeSubpath(types))
      {
        return;
      }
    }

    AssertionChain.GetOrCreate().FailWith(WithMessageThatCouldNotFindMultiple(types, _paths)); //bug

  }

  private string WithMessageThatCouldNotFindMultiple(object[] values, List<ObjectGraphPath> paths)
  {
    var message = "Could not find the particular sequence of objects: [" + string.Join(", ", values) +
                  $"] anywhere in dependency graph. Paths searched:{NewLine} {AsString(paths)}";
    return message;
  }

  private string WithMessageThatCouldNotFind<T>(T value, IReadOnlyCollection<ObjectGraphPath> pathsWithInstanceOfType)
  {
    var message = "Could not find the particular instance: " + value +
                  " anywhere in dependency graph";
    if (pathsWithInstanceOfType.Any())
    {
      message += " however, another instance of this type was found within the following paths:" +
                 NewLine + AsString(pathsWithInstanceOfType);
    }
    else
    {
      message += ". Paths created when searching: " + NewLine + AsString(_paths);
    }

    return message;
  }

  private static string AsString(IEnumerable<ObjectGraphPath> pathsWithInstanceOfType)
  {
    return string.Join("\n", pathsWithInstanceOfType.Select(s => s.ToString()));
  }


  //todo report error - make generic method non-static sometimes doesn't work

}