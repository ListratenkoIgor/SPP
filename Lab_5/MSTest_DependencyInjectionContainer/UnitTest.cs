using System.Collections.Generic;
using System.Linq;
using DependencyInjectionContainer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionContainerTester
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod] 
        public void TestResolve()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IService, ServiceImpl1>();
            var provider = new DependencyProvider(configuration);

            var service1 = provider.Resolve<IService>();
            
            Assert.AreEqual("ServiceImpl1", service1.Service());
        }

        [TestMethod]
        public void TestResolveTransient()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IService, ServiceImpl1>();
            var provider = new DependencyProvider(configuration);

            var service11 = provider.Resolve<IService>();
            var service12 = provider.Resolve<IService>();
            
            Assert.AreNotSame(service11, service12);
        }
        
        [TestMethod]
        public void TestResolveSingleton()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IService, ServiceImpl1>(Lifetime.Singleton);
            var provider = new DependencyProvider(configuration);

            var service11 = provider.Resolve<IService>();
            var service12 = provider.Resolve<IService>();
            
            Assert.AreSame(service11, service12);
        }
        
        [TestMethod]
        public void TestResolveWithMultipleImplementationsReturnsList()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IService, ServiceImpl1>();
            configuration.Register<IService, ServiceImpl2>();
            var provider = new DependencyProvider(configuration);

            var services = provider.Resolve<IEnumerable<IService>>();
            
            Assert.AreEqual(2, services.Count());
        }
        
        [TestMethod]
        public void TestResolveWithMultipleImplementationsSeparateCreation()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IService, ServiceImpl1>(variant: ServiceImplementation.First);
            configuration.Register<IService, ServiceImpl2>(variant: ServiceImplementation.Second);
            var provider = new DependencyProvider(configuration);

            var service1 = provider.Resolve<IService>(ServiceImplementation.First);
            var service2 = provider.Resolve<IService>(ServiceImplementation.Second);

            Assert.AreEqual("ServiceImpl1", service1.Service());
            Assert.AreEqual("ServiceImpl2", service2.Service());
        }

        [TestMethod]
        public void TestResolveWithEmbeddedType()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IService, ServiceImpl1>();
            configuration.Register<IController, Controller>();
            var provider = new DependencyProvider(configuration);

            var controller = provider.Resolve<IController>();
            
            Assert.AreEqual("Controller: ServiceImpl1", controller.Dispatch());
        }

        [TestMethod]
        public void TestResolveParameterizedType()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IContainerable, ServiceImpl1>();
            configuration.Register<IContainer<IContainerable>, ContainerImpl1<IContainerable>>();
            var provider = new DependencyProvider(configuration);

            var container = provider.Resolve<IContainer<IContainerable>>();
            var obj = container.GetObject();
            
            Assert.AreEqual("DependencyInjectionContainerTester.ServiceImpl1", obj.ShowType());
        }
        
        [TestMethod]
        public void TestResolveOpenGenerics()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IContainerable, ServiceImpl1>();
            configuration.Register(typeof(IContainer<>), typeof(ContainerImpl1<>));
            var provider = new DependencyProvider(configuration);
        
            var container = provider.Resolve<IContainer<IContainerable>>();
            var obj = container.GetObject();
            
            Assert.AreEqual("DependencyInjectionContainerTester.ServiceImpl1", obj.ShowType());
        }
        
        [TestMethod]
        public void TestResolveWithMultipleImplementationsThroughAttribute()
        {
            var configuration = new DependenciesConfiguration();
            configuration.Register<IContainerable, ServiceImpl1>(variant: ServiceImplementation.First);
            configuration.Register<IContainerable, ServiceImpl2>(variant: ServiceImplementation.Second);
            configuration.Register<IContainer<IContainerable>, ContainerImpl2<IContainerable>>();
            var provider = new DependencyProvider(configuration);
        
            var container = provider.Resolve<IContainer<IContainerable>>();
            var obj = container.GetObject();
            
            Assert.AreEqual("DependencyInjectionContainerTester.ServiceImpl2", obj.ShowType());
        }
    }
}