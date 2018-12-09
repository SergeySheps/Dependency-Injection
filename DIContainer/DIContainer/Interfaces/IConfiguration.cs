namespace DIContainer.Interfaces
{
    public interface IConfiguration
    {
        RegisteredType Register<TImplementation>() where TImplementation : class;

        RegisteredType Register<TDependency, TImplementation>()
           where TDependency : class
           where TImplementation : TDependency;
    }
}
