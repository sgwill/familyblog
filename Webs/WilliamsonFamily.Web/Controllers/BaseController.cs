using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Profiling;
using WilliamsonFamily.Library.Web.Filters;

namespace WilliamsonFamily.Web.Controllers
{
//    [EnableMiniProfilerActionFilter]
    public abstract class BaseController : Controller
    {
        protected MiniProfiler profiler = MiniProfiler.Current;

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            _resultExecutingToExecuted = MiniProfiler.Current.Step("OnResultExecuting");

            base.OnResultExecuting(filterContext);
        }

        private IDisposable _resultExecutingToExecuted;

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (_resultExecutingToExecuted != null)
                _resultExecutingToExecuted.Dispose();

            base.OnResultExecuted(filterContext);
        }

        public string QueryValue(string key)
        {
            if (!string.IsNullOrEmpty(Request.QueryString[key]))
                return Request.QueryString[key];
            return "";
        }

        public bool IsAjaxRequest
        {
            get
            {
                return (Request.ServerVariables["HTTP_X_REQUESTED_WITH"] ?? "") == "XMLHttpRequest"
                    || (Request.QueryString["__ajax"] ?? "") == "true";
            }
        }
    }
}