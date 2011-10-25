using System;
using System.Web.Mvc;
using WilliamsonFamily.Library.Web.Routing;
using WilliamsonFamily.Web.Models.Admin;
using WilliamsonFamily.Models.Log;
using MvcMiniProfiler;

namespace WilliamsonFamily.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
		public ILogManager LogManager { get; set; }

		[Route("admin/index")]
        public ActionResult Index()
        {
            return View();
        }

		[Route("admin/logs")]
		public ActionResult Logs()
		{
			using (MiniProfiler.Current.Step("AdminController.Logs"))
			{
				var model = new AdminLogModel();
				model.LogCount = LogManager.LogsCount();

				return View(model);
			}
		}

		[Route("admin/logs/clear")]
		public ActionResult ClearLogs()
		{
			using (MiniProfiler.Current.Step("AdminController.ClearLogs"))
			{
				LogManager.RemoveOldLogs(30);

				return RedirectToAction("Logs");
			}
		}

		[Route("admin/logs/compact")]
		public ActionResult CompactLogs()
		{
			using (MiniProfiler.Current.Step("AdminController.CompactLogs"))
			{
				LogManager.Compact();

				return RedirectToAction("Logs");
			}
		}

    }
}