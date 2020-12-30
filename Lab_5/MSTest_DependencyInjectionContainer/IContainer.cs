namespace DependencyInjectionContainerTester
{
    public interface IContainer<out TContainerable> where TContainerable : IContainerable
    {
        TContainerable GetObject();
    }
}