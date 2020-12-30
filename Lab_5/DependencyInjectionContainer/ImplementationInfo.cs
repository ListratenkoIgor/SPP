using System;

namespace DependencyInjectionContainer
{
    public class ImplementationInfo
    {
        public Lifetime Lifetime { get; }
        public Type ImplementationType { get; }
        public ServiceImplementation Variant { get; }   
        public ImplementationInfo(Type type, Lifetime lifetime, ServiceImplementation variant)
        {
            Lifetime = lifetime;
            ImplementationType = type;
            Variant = variant;
        }
    }
}