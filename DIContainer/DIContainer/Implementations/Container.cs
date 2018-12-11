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

            return ResolveType<TDependency>();

        }

        private TDependency ResolveType<TDependency>() where TDependency : class
        {
            var type = typeof(TDependency);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return ResolveCollection<TDependency>();
            }

            var registeredType = configuration.GetRegisteredType(type);
            if (registeredType == null && type.IsGenericType)
            {
                registeredType = configuration.GetRegisteredType(type.GetGenericTypeDefinition());
            }

            if (registeredType != null)
            {
                return (TDependency)GetInstance(type, registeredType);
            }

            throw new Exception($"Not registered type {type.FullName}");
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

        private object GetInstance(Type type, RegisteredType registeredType)
        {

            if (registeredType.DependencyTTL == DependencyTTL.Singleton && registeredType.Instance != null)
            {
                return registeredType.Instance;
            }

            lock (Sync)
            {
                if (registeredType.DependencyTTL == DependencyTTL.Singleton &&
                registeredType.Instance != null)
                {
                    return registeredType.Instance;
                }

                return CreateInstance(type, registeredType);
            }
        }

        private object CreateInstance(Type type, RegisteredType registeredType)
        {
            if (stack.Contains(type))
            {
                throw new Exception("Circular dependency");
            }

            stack.Push(type);

            var instanceType = registeredType.Implementation;

            var constructor = instanceType.GetConstructors()
                                .OrderByDescending(x => x.GetParameters().Length)
                                .FirstOrDefault();

            var parameters = GetConstructorParameters(constructor);
            registeredType.Instance = Activator.CreateInstance(instanceType, parameters);

            stack.Pop();

            return registeredType.Instance;
        }

        private object[] GetConstructorParameters(ConstructorInfo constructor)
        {
            var parameters = new List<object>();

            foreach (var parameter in constructor.GetParameters())
            {
                var registeredType = configuration.GetRegisteredType(parameter.ParameterType);

                if (registeredType == null)
                {
                    throw new Exception($"Not registered type {parameter.ParameterType.FullName}");
                }

                parameters.Add(GetInstance(parameter.ParameterType, registeredType));
            }

            return parameters.ToArray();
        }
    }
}
