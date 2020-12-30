using System;

namespace DependencyInjectionContainerTester
{
    public class ServiceImpl1 : IService, IContainerable
    {
        public string Service()
        {
            return "ServiceImpl1";
        }

        public string ShowType()
        {
            return typeof(ServiceImpl1).ToString();
        }
    }
}