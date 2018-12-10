using DIContainer.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DIContainer.Implementations
{
    public class Container : IContainer
    {
        private readonly IConfiguration configuration;
        private Stack<Type> stack = new Stack<Type>();
        private static object Sync = new object();

        public Container(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public TDependency Resolve<TDependency>() where TDependency : class
        {
            lock (Sync)
            {
                return ResolveType<TDependency>();
            }
        }

        private TDependency ResolveType<TDependency>() where TDependency : class
        {
            var type = typeof(TDependency);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return ResolveCollection<TDependency>();
            }

            var registeredType = configuration.GetRegisteredType(type);

            if (registeredType != null)
            {
                return GetInstance<TDependency>(registeredType);
            }

            throw new Exception($"Not registered type {type.FullName}"); ;
        }

        private TDependency ResolveCollection<TDependency>()
        {
            var nestedType = typeof(TDependency).GenericTypeArguments.FirstOrDefault();
            var registeredType = configuration.GetRegisteredType(nestedType);

            if (registeredType == null)
            {
                throw new Exception($"Not registered type: {nestedType?.FullName}");
            }

            var collection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(nestedType));
            var registeredTypes = configuration.GetRegisteredTypes(nestedType);

            foreach (var type in registeredTypes)
            {
                collection.Add(GetInstance(type.Dependency, type));
            }

            return (TDependency)collection;
        }

        private TDependency GetInstance<TDependency>(RegisteredType registeredType)
        {
            var type = typeof(TDependency);

            if (type.IsValueType)
            {
                return Activator.CreateInstance<TDependency>();
            }

            if (registeredType.DependencyTTL == DependencyTTL.Singleton)
            {
                if (registeredType.Implementation != null)
                {
                    return (TDependency)registeredType.Implementation;
                }
            }

            return (TDependency)GetInstance(type, registeredType);
        }

        private object GetInstance(Type type, RegisteredType registeredType)
        {
            if (stack.Contains(type))
            {
                throw new Exception("circular dependency");
            }

            stack.Push(type);

            var constructor = type.GetConstructors()
                               .OrderByDescending(x => x.GetParameters().Length)
                               .FirstOrDefault();

            var parameters = GetConstructorParameters(constructor);

            registeredType.Implementation = Activator.CreateInstance(type, parameters);
            stack.Pop();

            return registeredType.Implementation;
        }

        private object[] GetConstructorParameters(ConstructorInfo constructor)
        {
            var parameters = new List<object>();

            foreach (var parameter in constructor.GetParameters())
            {
                var registeredType = configuration.GetRegisteredType(parameter.ParameterType);
                if (registeredType == null)
                {
                    throw new Exception($"Not registered type {parameter.Name}");
                }
                parameters.Add(GetInstance(parameter.ParameterType, registeredType));
            }

            return parameters.ToArray();
        }
    }
}
