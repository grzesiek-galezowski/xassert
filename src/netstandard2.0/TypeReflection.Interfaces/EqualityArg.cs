using System;

namespace TddXt.XFluentAssert.TypeReflection.Interfaces
{
    public class EqualityArg
    {
        private readonly Type _type;
        private readonly Func<object> _createExample1;
        private readonly Func<object> _createExample2;

        private EqualityArg(Type type, Func<object> createExample1, Func<object> createExample2)
        {
            _type = type;
            _createExample1 = createExample1;
            _createExample2 = createExample2;
        }

        public static EqualityArg For<T>(Func<T> createExample1, Func<T> createExample2)
        {
            return new EqualityArg(typeof(T), () => createExample1(), () => createExample2());
        }

        public bool IsFor(Type type)
        {
            return _type == type;
        }

        public object Example1()
        {
            return _createExample1();
        }

        public object Example2()
        {
          return _createExample2();
        }
    }
}