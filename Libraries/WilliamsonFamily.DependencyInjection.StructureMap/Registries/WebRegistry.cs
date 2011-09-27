using StructureMap.Configuration.DSL;
using WilliamsonFamily.Library.Communication;
using WilliamsonFamily.Library.Web.Caching;
using WilliamsonFamily.Models.Caching;
using WilliamsonFamily.Models.Communication;
using WilliamsonFamily.Models.Web;

namespace WilliamsonFamily.DependencyInjection.StructureMap.Registries
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<ICache>()
                .Singleton()
                .Use<HttpCache>();

            For<IEmailSender>().Use<EmailSender>();

            SetAllProperties(p =>
            {
                p.OfType<ICache>();
                p.OfType<IEmailSender>();
            });
        }
    }
}