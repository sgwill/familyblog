using System;
using System.IO.Compression;
using System.Web.Mvc;
using StackExchange.Profiling;

namespace WilliamsonFamily.Library.Web.Filters
{
    public class CompressActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (MiniProfiler.Current.Step("CompressActionFilter.OnActionExecuting"))
            {
                var acceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
                if (string.IsNullOrEmpty(acceptEncoding))
                    return;

                var response = filterContext.HttpContext.Response;
                acceptEncoding = acceptEncoding.ToLowerInvariant();
                if (acceptEncoding.Contains("gzip"))
                {
                    response.AppendHeader("Content-encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                }
                else if (acceptEncoding.Contains("deflate"))
                {
                    response.AppendHeader("Content-encoding", "deflate");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                }
            }
        }
    }
}