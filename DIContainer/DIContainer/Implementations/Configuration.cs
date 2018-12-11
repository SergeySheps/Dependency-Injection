using DIContainer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIContainer.Implementations
{
    public class Configuration : IConfiguration
    {

        private readonly Dictionary<Type, ICollection<RegisteredType>> container =
            new Dictionary<Type, ICollection<RegisteredType>>();

        public RegisteredType GetRegisteredType(Type type)
        {
            return container.TryGetValue(type, out var registeredTypes)
                ? registeredTypes.FirstOrDefault()
                : null;
        }

        public IEnumerable<RegisteredType> GetRegisteredTypes(Type type)
        {
            return container.TryGetValue(type, out var registeredTypes) ? registeredTypes : null;
        }

        public RegisteredType Register<TImplementation>() where TImplementation : class
        {
            return RegisterType(typeof(TImplementation), typeof(TImplementation));
        }

        public RegisteredType Register<TDependency, TImplementation>()
           where TDependency : class
           where TImplementation : TDependency
        {
            return RegisterType(typeof(TDependency), typeof(TImplementation));
        }

        public RegisteredType Register(Type dependency, Type implementation)
        {
            return RegisterType(dependency, implementation);
        }

        private RegisteredType RegisterType(Type dependencyType, Type implementationType)
        {
            if (!IsValidImplementationType(implementationType))
            {
                throw new Exception($"{implementationType} is incorrect implementation type");
            }

            var registerType = new RegisteredType()
            {
                Dependency = dependencyType,
                Implementation = implementationType
            };

            if (container.TryGetValue(dependencyType, out var registeredTypes))
            {
                if (registeredTypes.All(x => x.Implementation != registerType.Implementation))
                {
                    registeredTypes.Add(registerType);
                }
            }
            else
            {
                container.Add(dependencyType, new List<RegisteredType>()
                {
                    registerType
                });
            }

            return registerType;
        }

        private bool IsValidImplementationType(Type implementationType)
        {
            return !(implementationType.IsAbstract || implementationType.IsInterface);
        }

    }
}
