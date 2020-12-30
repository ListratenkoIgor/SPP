using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependencyInjectionContainer
{
    public class DependencyProvider
    {
        private readonly Dictionary<Type, List<ImplementationInfo>> _dependencies;
        public DependencyProvider(DependenciesConfiguration configuration)
        {
            _dependencies = configuration.Dependencies;
        }
        public T Resolve<T>(ImplementationVariant implementationVariant = ImplementationVariant.None) where T: class
        {
            Type dependencyType = typeof(T);

            return (T)Resolve(dependencyType, implementationVariant);
        }
        private object Resolve(Type dependencyType, ImplementationVariant implementationVariant = ImplementationVariant.None)
        {
            if (dependencyType.IsGenericType && (dependencyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                return GetAllImplementations(dependencyType.GetGenericArguments()[0]);
            }

            if (dependencyType.IsGenericType && _dependencies.ContainsKey(dependencyType.GetGenericTypeDefinition()))
            {
                var implementationType = _dependencies[dependencyType.GetGenericTypeDefinition()][0].ImplementationType;
                implementationType = implementationType.MakeGenericType(dependencyType.GetGenericArguments());

                if (dependencyType.IsAssignableFrom(implementationType))
                {
                    return CreateInstance(implementationType);
                }
            }

            if (!_dependencies.ContainsKey(dependencyType))
            {
                return null;
            }

            var implementationInfo = GetImplementationInfo(dependencyType, implementationVariant);

            if (!dependencyType.IsAssignableFrom(implementationInfo.ImplementationType))
            {
                return null;
            }

            return implementationInfo.Lifetime switch
            {
                Lifetime.Transient => CreateInstance(implementationInfo.ImplementationType),
                Lifetime.Singleton => Singleton.GetInstance(implementationInfo.ImplementationType, CreateInstance),
                _ => null
            };
        }
        private ImplementationInfo GetImplementationInfo(Type dependencyType, ImplementationVariant variant)
        {
            var implementations = _dependencies[dependencyType];
            return implementations?.First(info => info.Variant == variant);
        }
        private IEnumerable GetAllImplementations(Type dependencyType)
        {
            if (_dependencies.ContainsKey(dependencyType))
            {
                List<ImplementationInfo> implementations = _dependencies[dependencyType];
                Type collectionType = typeof(List<>).MakeGenericType(dependencyType);
                IList instances = (IList)Activator.CreateInstance(collectionType);

                foreach (var implementation in implementations)
                {
                    instances.Add(CreateInstance(implementation.ImplementationType));
                }

                return instances;
            }
            return null;
        }
        private object CreateInstance(Type type)
        {
            ConstructorInfo constructor = type.GetConstructors()[0];
            var parameters = new List<object>();

            foreach (var parameter in constructor.GetParameters())
            {
                var attribute = (DependencyKeyAttribute) parameter.GetCustomAttribute(typeof(DependencyKeyAttribute));

                parameters.Add(attribute == null
                    ? Resolve(parameter.ParameterType)
                    : Resolve(parameter.ParameterType, attribute.ImplementationVariant));
            }

            return Activator.CreateInstance(type, parameters.ToArray());
        }
    }
}