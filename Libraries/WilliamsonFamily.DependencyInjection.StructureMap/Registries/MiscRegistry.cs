using StructureMap.Configuration.DSL;
using WilliamsonFamily.Library.Web;
using WilliamsonFamily.Models.Web;
using WilliamsonFamily.Models.Log;
using WilliamsonFamily.Library.Log;
using System.Configuration;

namespace WilliamsonFamily.DependencyInjection.StructureMap.Registries
{
    public class MiscRegistry : Registry
    {
        public MiscRegistry()
        {
            For<ITitleCleaner>().Use<TitleCleaner>();
			For<ILogManager>().Use<ElmahLogManager>()
				.WithProperty("ConnectionString").EqualTo(ConfigurationManager.ConnectionStrings["Elmah.SQLite"].ConnectionString);

            SetAllProperties(p =>
            {
                p.OfType<ITitleCleaner>();
				p.OfType<ILogManager>();
            });
        }
    }
}