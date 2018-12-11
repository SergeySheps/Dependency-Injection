using DIContainer.Test.TestedDependencies;

namespace DIContainer.Test.TestedImplementations
{
    public class TestClass2 : ITest2
    {
        public ITest1 Test1 { get; }

        public TestClass2(ITest1 test)
        {
            Test1 = test;
        }
    }
}
