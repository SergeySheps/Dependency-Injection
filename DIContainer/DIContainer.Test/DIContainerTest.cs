using DIContainer;
using DIContainer.Implementations;
using DIContainer.Interfaces;
using DIContainer.Test.TestedDependencies;
using DIContainer.Test.TestedImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DIContainer.Test
{
    [TestClass]
    public class DIContainerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var conf = new Configuration();
            conf.Register<ITest1, TestClass1>().DependencyTTL = DependencyTTL.Singleton;

            var container = new Container(conf);

            var test1 = container.Resolve<ITest1>();
            var test2 = container.Resolve<ITest1>();

            Assert.AreEqual(test1, test2);
        }
    }
}
