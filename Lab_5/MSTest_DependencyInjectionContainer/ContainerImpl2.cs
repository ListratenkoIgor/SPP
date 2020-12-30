using DependencyInjectionContainer;

namespace DependencyInjectionContainerTester
{
    public class ContainerImpl2<TContainerable> : IContainer<TContainerable> where TContainerable : IContainerable
    {
        private readonly TContainerable _object;

        public ContainerImpl2([DependencyKey(ServiceImplementation.Second)] TContainerable containerable)
        {
            _object = containerable;
        }

        public TContainerable GetObject()
        {
            return _object;
        }
    }
}