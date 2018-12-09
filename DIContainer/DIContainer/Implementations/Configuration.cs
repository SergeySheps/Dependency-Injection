using DIContainer.Interfaces;

namespace DIContainer.Implementations
{
    public class Configuration : IConfiguration
    {
        public void Register<TImplementation>() where TImplementation : class
        {
            //
        }
        public void Register<TDependency, TImplementation>()
           where TDependency : class
           where TImplementation : TDependency
        {
            //
        }

    }
}
