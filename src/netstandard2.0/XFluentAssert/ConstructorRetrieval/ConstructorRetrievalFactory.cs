namespace TddXt.XFluentAssert.ConstructorRetrieval;

internal class ConstructorRetrievalFactory
{
  private readonly IConstructorRetrieval _constructorQuery;

  public ConstructorRetrievalFactory()
  {
    _constructorQuery =
      PublicNonRecursiveConstructors(
        PublicParameterlessConstructors(
          InternalNonRecursiveConstructors(
            InternalParameterlessConstructors(
              PublicStaticNonRecursiveFactoryMethod(
                PublicRecursiveConstructors(
                  InternalRecursiveConstructors(
                    PrimitiveConstructor()
                  )))))));
  }

  private IConstructorRetrieval PublicStaticNonRecursiveFactoryMethod(IConstructorRetrieval next)
  {
    return new PublicStaticFactoryMethodRetrieval(next);
  }

  private IConstructorRetrieval PrimitiveConstructor()
  {
    return new PrimitiveConstructorRetrieval();
  }

  public IConstructorRetrieval Create()
  {
    return _constructorQuery;
  }

  private static IConstructorRetrieval PublicRecursiveConstructors(IConstructorRetrieval next)
  {
    return new PublicRecursiveConstructorsRetrieval(next);
  }

  private static IConstructorRetrieval InternalRecursiveConstructors(IConstructorRetrieval next)
  {
    return new InternalRecursiveConstructorRetrieval(next);
  }

  private static IConstructorRetrieval PublicNonRecursiveConstructors(IConstructorRetrieval next)
  {
    return new PublicNonRecursiveConstructorRetrieval(next);
  }

  private static IConstructorRetrieval PublicParameterlessConstructors(IConstructorRetrieval next)
  {
    return new PublicParameterlessConstructorRetrieval(next);
  }

  private static IConstructorRetrieval InternalNonRecursiveConstructors(IConstructorRetrieval next)
  {
    return new InternalConstructorWithoutRecursionRetrieval(next);
  }

  private static IConstructorRetrieval InternalParameterlessConstructors(IConstructorRetrieval next)
  {
    return new NonPublicParameterlessConstructorRetrieval(next);
  }
}