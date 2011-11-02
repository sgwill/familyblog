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
		[Route("admin")]
		public ActionResult Index()
		{
			return View();
		}

		[Route("admin/logs")]
		public ActionResult Logs()
		{
			var model = new AdminLogModel();
			model.LogCount = LogManager.LogsCount();

			return View(model);
		}

		[Route("admin/logs/clear")]
		public ActionResult ClearLogs()
		{
			LogManager.RemoveOldLogs(30);

			return RedirectToAction("Logs");
		}

		[Route("admin/logs/compact")]
		public ActionResult CompactLogs()
		{
			LogManager.Compact();

			return RedirectToAction("Logs");
		}
	}
}