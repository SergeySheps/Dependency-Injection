using System;

namespace DIContainer
{
    public class RegisteredType
    {

        public Type Dependency { get; set; }
        public Type Implementation { get; set; }
        public DependencyTTL DependencyTTL { get; set; }
        public object Instance { get; set; }

    }
}
