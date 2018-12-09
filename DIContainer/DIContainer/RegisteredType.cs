using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer
{
    public class RegisteredType
    {

        public Type Dependency { get; set; }
        public object Implementation { get; set; }
        public DependencyTTL DependencyTTL { get; set; }

    }
}
