using System;
using System.Web.Mvc;
using MvcMiniProfiler;
using System.Threading;
using WilliamsonFamily.Models.Blog;

namespace WilliamsonFamily.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}