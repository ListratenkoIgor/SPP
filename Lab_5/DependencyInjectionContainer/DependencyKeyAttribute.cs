using System;

namespace DependencyInjectionContainer
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class DependencyKeyAttribute : Attribute
    {
        public ServiceImplementation ImplementationVariant { get; }

        public DependencyKeyAttribute(ServiceImplementation variant)
        {
            ImplementationVariant = variant;
        }
    }
}