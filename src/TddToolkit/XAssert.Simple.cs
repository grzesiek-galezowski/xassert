﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using NSubstitute;

namespace TddEbook.TddToolkit
{
  public partial class XAssert
  {
    public static void Equal<T>(T expected, T actual)
    {
      actual.Should().Be(expected);
    }
    public static void Equal(string expected, string actual)
    {
      actual.Should().Be(expected);
    }
    public static void Equal(int expected, int actual)
    {
      actual.Should().Be(expected);
    }
    public static void Equal(float expected, float actual)
    {
      actual.Should().Be(expected);
    }
    public static void Equal(double expected, double actual)
    {
      actual.Should().Be(expected);
    }
    public static void Equal(short expected, short actual)
    {
      actual.Should().Be(expected);
    }
    public static void Equal(decimal expected, decimal actual)
    {
      actual.Should().Be(expected);
    }

    public static void NotEqual<T>(T expected, T actual)
    {
      actual.Should().NotBe(expected);
    }
    public static void NotEqual(string expected, string actual)
    {
      actual.Should().NotBe(expected);
    }

    public static void Same<T>(T expected, T actual)
    {
      actual.Should().BeSameAs(expected);
    }
    public static void Same(string expected, string actual)
    {
      actual.Should().BeSameAs(expected);
    }

    public static void NotSame<T>(T expected, T actual)
    {
      actual.Should().NotBeSameAs(expected);
    }
    public static void NotSame(string expected, string actual)
    {
      actual.Should().NotBeSameAs(expected);
    }

    public static void Equal<T>(T expected, T actual, string message)
    {
      actual.Should().Be(expected, "{0}", message);
    }

    public static void CollectionsEqual<T>
      (IEnumerable<T> expected, IEnumerable<T> actual, string message)
    {
      actual.Count().Should().Be(expected.Count(), "{0}", message);
      actual.Should().ContainInOrder(expected, "{0}", message);
    }
    public static void CollectionsEqual<T>
      (IEnumerable<T> expected, IEnumerable<T> actual)
    {
      actual.Count().Should().Be(expected.Count());
      actual.Should().ContainInOrder(expected);
    }

    public static void CollectionsEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
      actual.Should().BeEquivalentTo(expected);
    }
    public static void CollectionsNotEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
      actual.Should().BeEquivalentTo(expected);
    }

    public static void NotEqual<T>(T expected, T actual, string message)
    {
      actual.Should().NotBe(expected, "{0}", message);
    }

    public static void Same<T>(T expected, T actual, string message)
    {
      actual.Should().BeSameAs(expected, "{0}", message);
    }

    public static void NotSame<T>(T expected, T actual, string message)
    {
      actual.Should().NotBeSameAs(expected, "{0}", message);
    }
    public static void NotEqual(string expected, string actual, string message)
    {
      actual.Should().NotBe(expected, "{0}", message);
    }

    public static void Same(string expected, string actual, string message)
    {
      actual.Should().BeSameAs(expected, "{0}", message);
    }

    public static void NotSame(string expected, string actual, string message)
    {
      actual.Should().NotBeSameAs(expected, "{0}", message);
    }

    public static void NotNull<T>(T item)
    {
      item.Should().NotBeNull();
    }

    public static void NotNull<T>(T item, string message)
    {
      item.Should().NotBeNull(message, "{0}", message);
    }

    public static void IsUpperCase(string str)
    {
      str.Should().Be(str.ToUpperInvariant());
    }

    public static void IsLowerCase(string str)
    {
      str.Should().Be(str.ToLowerInvariant());
    }

    public static void IsLowerCase(char c)
    {
      c.Should().Be(char.ToLowerInvariant(c));
    }

    public static void IsUpperCase(char c)
    {
      c.Should().Be(char.ToUpperInvariant(c));
    }
  }
}
