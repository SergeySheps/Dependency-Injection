using DIContainer.Test.TestedDependencies;

namespace DIContainer.Test.TestedImplementations
{
    public class ServiceGeneric<T> : IServiceGeneric<T> where T : IService
    {
        public IService Service { get; }

        public ServiceGeneric(IService service)
        {
            Service = service;
        }
    }
}
