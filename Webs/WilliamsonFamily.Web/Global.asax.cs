using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WilliamsonFamily.Library.Web;
using MvcMiniProfiler;
using Elmah;

namespace WilliamsonFamily.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
               "Home",
               "home",
                new { controller = "Home", action = "Index", id = "" }
            );

            RouteTable.Routes.MapRoute(
               "Login",
               "home/login",
               new { controller = "Account", action = "Login" }
            );

            RouteTable.Routes.MapRoute(
                "Logout",
                "home/logout",
                new { controller = "Account", action = "Logout" }
            );

            ///////////////////////////////////////////////////////
            // Blog Routes
            ///////////////////////////////////////////////////////

            RouteTable.Routes.MapRoute(
                "BlogList",
                "{user}/blog/list.aspx/{*date}",
                new { controller = "Blog", action = "List", user = "", date = "" }
            );

            RouteTable.Routes.MapRoute(
                "BlogUserList",
                "{user}/blog/userlist.aspx",
                new { controller = "Blog", action = "UserList", user = "" }
            );

            RouteTable.Routes.MapRoute(
               "BlogListFeed",
               "{user}/blog/list.xml.aspx/{*date}",
               new { controller = "Blog", action = "Feed", user = "", date = "" }
            );

            RouteTable.Routes.MapRoute(
                "BlogCreate",
                "{user}/blog/create.aspx/",
                new { controller = "Blog", action = "Create", user = "" }
            );

            RouteTable.Routes.MapRoute(
                "BlogEdit",
                "{user}/blog/edit/{slug}.aspx",
                new { controller = "Blog", action = "Edit", user = "", slug = "" }
            );

            RouteTable.Routes.MapRoute(
                "BlogDetail",
                "{user}/blog/{title}.aspx",
                new { controller = "Blog", action = "Detail", user = "", title = "" }
            );

            ///////////////////////////////////////////////////////
            // Photo Routes
            ///////////////////////////////////////////////////////

            RouteTable.Routes.MapRoute(
               "Photo",
               "{user}/photo/upload.aspx",
               new { controller = "Photo", action = "Upload", user = "" }
            );

            ///////////////////////////////////////////////////////
            // Default Routes
            ///////////////////////////////////////////////////////

            RouteTable.Routes.MapRoute(
              "Default",                                              // Route name
              "{controller}/{action}/{id}",                           // URL with parameters
              new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );


            RouteTable.Routes.MapRoute(
                "DefaultWithUser",
                "{user}/{controller}/{action}/{id}",
                new { controller = "", action = "", user = "", id = "" }
            );
            
          

            // 404
            RouteTable.Routes.MapRoute(
                "404-Catch",
                "{*url}",
                new { controller = "Base", action = "MissingPage" }
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Clear();
            //ViewEngines.Engines.AddGenericMobile<RazorViewEngine>();
            ViewEngines.Engines.Add(new RazorViewEngine());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes();
        }

        protected void Application_BeginRequest()
        {
            MiniProfiler profiler = null;

            // might want to decide here (or maybe inside the action) whetddher you want
            // to profile this request - for example, using an "IsSystemAdmin" flag against
            // the user, or similar; this could also all be done in action filters, but this
            // is simple and practical; just return null for most users. For our test, we'll
            // profile only for local requests (seems reasonable)
            //if (User != null && User.IsInRole("admin"))
            //{
            //    profiler = MvcMiniProfiler.MiniProfiler.Start();
            //}

            profiler.Step("Application_BeginRequest");
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Current.Step("Application_EndRequest");
            MvcMiniProfiler.MiniProfiler.Stop();
        }

        protected void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            FilterError404(e);
        }

        //Dimiss 404 errors for ELMAH
        private void FilterError404(ExceptionFilterEventArgs e)
        {
            if (e.Exception.GetBaseException() is HttpException)
            {
                HttpException ex = (HttpException)e.Exception.GetBaseException();

                if (ex.GetHttpCode() == 404)
                {
                    e.Dismiss();
                }
            }
        }
    }
}