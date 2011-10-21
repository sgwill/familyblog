using StructureMap.Configuration.DSL;
using WilliamsonFamily.Library.Web;
using WilliamsonFamily.Models.Web;
using WilliamsonFamily.Models.Log;
using WilliamsonFamily.Library.Log;

namespace WilliamsonFamily.DependencyInjection.StructureMap.Registries
{
    public class MiscRegistry : Registry
    {
        public MiscRegistry()
        {
            For<ITitleCleaner>().Use<TitleCleaner>();
			For<ILogCleaner>().Use<ElmahLogCleaner>();

            SetAllProperties(p =>
            {
                p.OfType<ITitleCleaner>();
				p.OfType<ILogCleaner>();
            });
        }
    }
}