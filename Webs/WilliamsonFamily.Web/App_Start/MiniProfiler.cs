using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Microsoft.Web.Infrastructure;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using StackExchange.Profiling.MVCHelpers;
using StackExchange.Profiling;

[assembly: WebActivator.PreApplicationStartMethod(
	typeof(WilliamsonFamily.Web.App_Start.MiniProfilerPackage), "PreStart")]

[assembly: WebActivator.PostApplicationStartMethod(
	typeof(WilliamsonFamily.Web.App_Start.MiniProfilerPackage), "PostStart")]


namespace WilliamsonFamily.Web.App_Start 
{
    public static class MiniProfilerPackage
    {
        public static void PreStart()
        {
            //TODO: Non SQL Server based installs can use other formatters like: new MvcMiniProfiler.SqlFormatters.InlineFormatter()
            MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

            //Make sure the MiniProfiler handles BeginRequest and EndRequest
            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));

            //Setup profiler for Controllers via a Global ActionFilter
            GlobalFilters.Filters.Add(new ProfilingActionFilter());
        }

        public static void PostStart()
        {
            // Intercept ViewEngines to profile all partial views and regular views.
            // If you prefer to insert your profiling blocks manually you can comment this out
            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }
    }

    public class MiniProfilerStartupModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
				MiniProfiler.Start();
            };

            context.AuthenticateRequest += (sender, e) =>
            {
				if (context.User == null || !context.User.Identity.IsAuthenticated || context.User.Identity.Name != "sgwill")
                    MiniProfiler.Stop(discardResults: true);
            };
            
            context.EndRequest += (sender, e) =>
            {
                MiniProfiler.Stop();
            };
        }

        public void Dispose() { }
    }
}