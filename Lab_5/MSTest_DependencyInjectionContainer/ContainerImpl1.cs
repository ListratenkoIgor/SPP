using DependencyInjectionContainer;

namespace DependencyInjectionContainerTester
{
    public class ContainerImpl1<TContainerable> : IContainer<TContainerable> where TContainerable : IContainerable
    {
        private readonly TContainerable _object;

        public ContainerImpl1(TContainerable containerable)
        {
            _object = containerable;
        }

        public TContainerable GetObject()
        {
            return _object;
        }
    }
}