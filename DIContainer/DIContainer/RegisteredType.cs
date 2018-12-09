using System;

namespace DIContainer
{
    public class RegisteredType
    {

        public Type Dependency { get; set; }
        public object Implementation { get; set; }
        public DependencyTTL DependencyTTL { get; set; }

    }
}
