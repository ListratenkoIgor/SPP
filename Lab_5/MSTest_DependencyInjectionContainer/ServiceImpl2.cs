using System;

namespace DependencyInjectionContainerTester
{
    public class ServiceImpl2 : IService, IContainerable
    {
        public string Service()
        {
            return "ServiceImpl2";
        }

        public string ShowType()
        {
            return typeof(ServiceImpl2).ToString();
        }
    }
}