using StructureMap.Configuration.DSL;
using WilliamsonFamily.Library.Web;
using WilliamsonFamily.Models.Web;

namespace WilliamsonFamily.DependencyInjection.StructureMap.Registries
{
    public class MiscRegistry : Registry
    {
        public MiscRegistry()
        {
            For<ITitleCleaner>().Use<TitleCleaner>();

            SetAllProperties(p =>
            {
                p.OfType<ITitleCleaner>();
            });
        }
    }
}
