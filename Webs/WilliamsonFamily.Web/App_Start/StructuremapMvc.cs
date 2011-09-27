using System.Web.Mvc;
using StructureMap;
using WilliamsonFamily.DependencyInjection.StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(WilliamsonFamily.Web.App_Start.StructuremapMvc), "Start")]

namespace WilliamsonFamily.Web.App_Start
{
    public static class StructuremapMvc
    {
        public static void Start()
        {
            var container = (IContainer)IoC.Initialize();
            DependencyResolver.SetResolver(new WilliamfonFamilyDependencyResolver(container));
        }
    }
}