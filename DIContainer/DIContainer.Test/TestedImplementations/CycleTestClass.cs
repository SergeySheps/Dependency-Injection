using DIContainer.Test.TestedDependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer.Test.TestedImplementations
{
    public class CycleTestClass : ITest1
    {
        private ITest2 Test2 { get; }

        public CycleTestClass(ITest2 test)
        {
            Test2 = test;
        }
    }
}
