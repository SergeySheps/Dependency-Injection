using System;
using System.Collections.Generic;

namespace DIContainer.Interfaces
{
    public interface IConfiguration
    {
        RegisteredType Register<TImplementation>() where TImplementation : class;

        RegisteredType Register<TDependency, TImplementation>()
           where TDependency : class
           where TImplementation : TDependency;

        RegisteredType GetRegisteredType(Type type);

        IEnumerable<RegisteredType> GetRegisteredTypes(Type type);
    }
}
