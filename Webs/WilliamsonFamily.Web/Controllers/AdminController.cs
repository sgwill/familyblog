using System;
using System.Web.Mvc;

namespace WilliamsonFamily.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

    }
}