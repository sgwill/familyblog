using StructureMap;
using WilliamsonFamily.DependencyInjection.StructureMap.Registries;

namespace WilliamsonFamily.DependencyInjection.StructureMap
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
                {
                    x.AddRegistry<DataRegistry>();
                    x.AddRegistry<WebRegistry>();
                    x.AddRegistry<MiscRegistry>();
                });

            return ObjectFactory.Container;
        }
    }
}