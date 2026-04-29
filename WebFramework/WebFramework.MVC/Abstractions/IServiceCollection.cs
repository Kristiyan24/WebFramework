namespace WebFramework.MVC.Abstractions;

public interface IServiceCollection
{
    void Add<TSource, TDestination>() where TDestination : TSource;

    object CreateInstance(Type type);

    T CreateInstance<T>();
}
