﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TddXt.XFluentAssert.GraphAssertions.DependencyAssertions
{
  internal class ArrayNode : IObjectGraphNode
  {
    private readonly object _target;
    private readonly string _fieldName;
    private readonly IReadOnlyList<IObjectGraphNode> _path;
    private readonly Action<string> _log;
    private readonly IEnumerable<ITerminalNodeCondition> _terminalNodeConditions;

    public ArrayNode(
      object target,
      string fieldName,
      IEnumerable<IObjectGraphNode> path, 
      Action<string> log, 
      IEnumerable<ITerminalNodeCondition> terminalNodeConditions)
    {
      _target = target;
      _fieldName = fieldName;
      _log = log;
      _path = path.Concat(new [] {this}).ToList();
      _terminalNodeConditions = terminalNodeConditions;
    }

    public void CollectPathsInto(ObjectGraphPaths objectGraphPaths)
    {
      //todo consider extracting the items before passing them to this class instance
      var list = ToObjectsList();
      var items = list.Select((o, i) => ObjectGraphNode.From(
        _path, 
        o, 
        "array element[" + i + "]", 
        _log, 
        _terminalNodeConditions));

      if (items.Any())
      {
        foreach (var item in items)
        {
          item.CollectPathsInto(objectGraphPaths);
        }
      }
      else
      {
        objectGraphPaths.Add(new ObjectGraphPath(_path));
      }
    }

    private List<object> ToObjectsList()
    {
      var list = new List<object>();
      foreach (var o in (IEnumerable)_target)
      {
        list.Add(o);
      }

      return list;
    }

    public bool IsOf(Type type)
    {
      return _target.GetType() == type;
    }

    public bool ValueIsEqualTo<T>(T value)
    {
      //todo what if both are null?
      if ((object)value is object[] expectedArray && _target is object[] targetArray)
      {
        return expectedArray.SequenceEqual(targetArray);
      }
      else
      {
        return Equals(value, _target);
      }
    }

    public bool ValueIsSameAs(object value)
    {
      return ReferenceEquals(_target, value);
    }

    public override string ToString()
    {
      return ObjectGraphNode.FormatFieldAndTypeName(_fieldName, _target);
    }
  }
}