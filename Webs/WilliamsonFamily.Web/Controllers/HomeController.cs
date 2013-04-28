using System;
using System.Web.Mvc;
using WilliamsonFamily.Library.Web.Routing;
using WilliamsonFamily.Models.Content;

namespace WilliamsonFamily.Web.Controllers
{
    public class HomeController : BaseController
    {
		public IContentRepository ContentRepository { get; set; }
		[Route("", RoutePriority.High)]
		[Route("home")]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}