using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer.Test.TestedDependencies
{
    public interface ITest2
    {
        ITest1 Test1 { get; }
    }
}
