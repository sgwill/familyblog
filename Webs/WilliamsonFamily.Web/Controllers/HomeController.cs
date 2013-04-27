using System;
using System.Web.Mvc;
using WilliamsonFamily.Library.Web.Routing;

namespace WilliamsonFamily.Web.Controllers
{
    public class HomeController : BaseController
    {
		[Route("", RoutePriority.High)]
		[Route("home")]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}