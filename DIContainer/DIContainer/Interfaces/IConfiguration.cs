namespace DIContainer.Interfaces
{
    public interface IConfiguration
    {
        void Register<TImplementation>() where TImplementation : class;

        void Register<TDependency, TImplementation>()
           where TDependency : class
           where TImplementation : TDependency;
    }
}
