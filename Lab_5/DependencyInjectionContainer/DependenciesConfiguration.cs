﻿using System;
using System.Collections.Generic;

namespace DependencyInjectionContainer
{
    public class DependenciesConfiguration
    {
        public Dictionary<Type, List<ImplementationInfo>> Dependencies { get; }

        public DependenciesConfiguration()
        {
            Dependencies = new Dictionary<Type, List<ImplementationInfo>>();
        }

        public void Register<TDependency, TImplementation>(Lifetime lifetime = Lifetime.Transient, ServiceImplementation variant = ServiceImplementation.None) 
            where TDependency: class 
            where TImplementation: TDependency
        {
            var implementationInfo = new ImplementationInfo(typeof(TImplementation), lifetime, variant);

            if (Dependencies.ContainsKey(typeof(TDependency)))
            {
                Dependencies[typeof(TDependency)].Add(implementationInfo);
            }
            else
            {
                Dependencies.Add(typeof(TDependency), new List<ImplementationInfo>());
                Dependencies[typeof(TDependency)].Add(implementationInfo);
            }
        }

        public void Register(Type dependencyType, Type implementationType)
        {
            var implementationInfo = new ImplementationInfo(implementationType, Lifetime.Transient, ServiceImplementation.None);

            if (Dependencies.ContainsKey(dependencyType))
            {
                Dependencies[dependencyType].Add(implementationInfo);
            }
            else
            {
                Dependencies.Add(dependencyType, new List<ImplementationInfo>());
                Dependencies[dependencyType].Add(implementationInfo);
            }
        }
    }
}