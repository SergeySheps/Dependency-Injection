using DIContainer.Interfaces;

namespace DIContainer.Implementations
{
    public class Container : IContainer
    {
        private readonly IConfiguration configuration;

        public Container(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public TDependency Resolve<TDependency>() where TDependency : class
        {
            return null;
        }
    }
}
