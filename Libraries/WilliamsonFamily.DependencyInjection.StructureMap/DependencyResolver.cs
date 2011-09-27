using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace WilliamsonFamily.DependencyInjection.StructureMap
{
    public class WilliamfonFamilyDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public WilliamfonFamilyDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null) return null;
            try
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                         ? _container.TryGetInstance(serviceType)
                         : _container.GetInstance(serviceType);
            }
            catch
            {

                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}