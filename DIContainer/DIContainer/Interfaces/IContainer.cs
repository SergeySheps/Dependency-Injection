namespace DIContainer.Interfaces
{
    public interface IContainer
    {
        TDependency Resolve<TDependency>() where TDependency : class;
    }
}
