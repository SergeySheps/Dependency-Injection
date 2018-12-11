namespace DIContainer.Test.TestedDependencies
{
    public interface IServiceGeneric<T> where T : IService
    {
        IService Service { get; }
    }
}
