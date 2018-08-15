namespace TddXt.XAssert.ConstructorRetrieval
{
  public class ConstructorRetrievalFactory
  {
    private readonly ConstructorRetrieval _constructorQuery;

    public ConstructorRetrievalFactory()
    {
      this._constructorQuery = 
        PublicNonRecursiveConstructors(
          PublicParameterlessConstructors(
            InternalNonRecursiveConstructors(
              InternalParameterlessConstructors(
                this.PublicStaticNonRecursiveFactoryMethod(
                  PublicRecursiveConstructors(
                    InternalRecursiveConstructors(
                      this.PrimitiveConstructor()
                    )))))));
    }

    private ConstructorRetrieval PublicStaticNonRecursiveFactoryMethod(ConstructorRetrieval next)
    {
      return new PublicStaticFactoryMethodRetrieval(next);
    }

    private ConstructorRetrieval PrimitiveConstructor()
    {
      return new PrimitiveConstructorRetrieval();
    }

    public ConstructorRetrieval Create()
    {
      return this._constructorQuery;
    }

    private static ConstructorRetrieval PublicRecursiveConstructors(ConstructorRetrieval next)
    {
      return new PublicRecursiveConstructorsRetrieval(next);
    }

    private static ConstructorRetrieval InternalRecursiveConstructors(ConstructorRetrieval next)
    {
      return new InternalRecursiveConstructorRetrieval(next);
    }

    private static ConstructorRetrieval PublicNonRecursiveConstructors(ConstructorRetrieval next)
    {
      return new PublicNonRecursiveConstructorRetrieval(next);
    }

    private static ConstructorRetrieval PublicParameterlessConstructors(ConstructorRetrieval next)
    {
      return new PublicParameterlessConstructorRetrieval(next);
    }

    private static ConstructorRetrieval InternalNonRecursiveConstructors(ConstructorRetrieval next)
    {
      return new InternalConstructorWithoutRecursionRetrieval(next);
    }

    private static ConstructorRetrieval InternalParameterlessConstructors(ConstructorRetrieval next)
    {
      return new NonPublicParameterlessConstructorRetrieval(next);
    }
  }
}