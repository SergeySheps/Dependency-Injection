using DIContainer;
using DIContainer.Implementations;
using DIContainer.Interfaces;
using DIContainer.Test.TestedDependencies;
using DIContainer.Test.TestedImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DIContainer.Test
{
    [TestClass]
    public class DIContainerTest
    {

        [TestMethod]
        public void TestDIInjection()
        {

            var conf = new Configuration();

            conf.Register<ITest1, TestClass1>();
            conf.Register<ITest2, TestClass2>();

            var container = new Container(conf);

            var test = container.Resolve<ITest2>();

            Assert.IsNotNull(test.Test1);

        }

        [TestMethod]
        public void TestSingletonTTL()
        {
            var conf = new Configuration();
            conf.Register<ITest1, TestClass1>().DependencyTTL = DependencyTTL.Singleton;

            var container = new Container(conf);

            var test1 = container.Resolve<ITest1>();
            var test2 = container.Resolve<ITest1>();

            Assert.AreEqual(test1, test2);
        }

        [TestMethod]
        public void TestInstancePerDependencyTTL()
        {
            var conf = new Configuration();
            conf.Register<ITest1, TestClass1>().DependencyTTL = DependencyTTL.InstancePerDependency;

            var container = new Container(conf);

            var test1 = container.Resolve<ITest1>();
            var test2 = container.Resolve<ITest1>();

            Assert.AreNotEqual(test1, test2);
        }

        [TestMethod]
        public void CyclicDependency()
        {
            var conf = new Configuration();
            conf.Register<ITest2, TestClass2>();
            conf.Register<ITest1, CycleTestClass>();

            var container = new Container(conf);

            try
            {
                var bar = container.Resolve<ITest1>();
                Assert.Fail("Cannot be cyclic dependencies");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Circular dependency", e.Message);
            }
        }
    }
}
