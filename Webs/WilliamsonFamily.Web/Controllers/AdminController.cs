using System;
using System.Web.Mvc;
using WilliamsonFamily.Library.Web.Routing;

namespace WilliamsonFamily.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
		[Route("admin/index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}