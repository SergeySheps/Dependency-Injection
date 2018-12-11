using DIContainer.Test.TestedDependencies;

namespace DIContainer.Test.TestedImplementations
{
    public class ServiceImpl : IService
    {
        public ITest1 Test1 { get; }
        public ITest2 Test2 { get; }

        public ServiceImpl(ITest1 test1, ITest2 test2)
        {
            Test1 = test1;
            Test2 = test2;
        }
    }
}
