using System.Web.Mvc;
using StackExchange.Profiling;

namespace WilliamsonFamily.Library.Web.Filters
{
    public class EnableMiniProfilerActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity.IsAuthenticated && filterContext.HttpContext.User.Identity.Name == "sgwill")
            {
                if (MiniProfiler.Current == null)
                    MiniProfiler.Start();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}