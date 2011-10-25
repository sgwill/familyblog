using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WilliamsonFamily.Library.Web.Routing;
using Elmah;

namespace WilliamsonFamily.Web.Controllers
{
	public class ErrorController : BaseController
    {
		[Route("404")]
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return new ContentResult { Content = "Not Found (this is a 404 page)" };
        }

		[Route("exception/log")]
		public void LogJavaScriptError(string message)
		{
			ErrorSignal
				.FromCurrentContext()
				.Raise(new JavaScriptException(message));
		}
    }

	public class JavaScriptException : Exception
	{
		public JavaScriptException(string message)
			: base(message)
		{
		}
	}
}
