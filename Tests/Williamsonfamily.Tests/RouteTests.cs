﻿using System;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using WilliamsonFamily.Web.Controllers;
using Rhino.Mocks;

namespace WilliamsonFamily.Web.Tests
{
    [TestClass]
    public class RouteTests
    {
        #region Home Routes
        [TestMethod]
        public void Routes_EmptyString_RoutesTo_HomeIndex()
        {
            "~/".Route().ShouldMapTo<HomeController>(a => a.Index());
        }
        
        [TestMethod]
        public void Routes_Home_RoutesTo_HomeIndex()
        {
            "~/home".ShouldMapTo<HomeController>(a => a.Index());
        }

        [TestMethod]
        public void Routes_HomeLogin_RoutesTo_AccountLogin()
        {
            "~/home/login".ShouldMapTo<AccountController>(a => a.Login());
        }

        [TestMethod]
        public void Routes_HomeLogout_RoutesTo_AccountLogout()
        {
            "~/home/logout".ShouldMapTo<AccountController>(a => a.Logout());
        }

        #endregion

        #region Admin routes
		[TestMethod]
		public void Routes_Admin_RoutesTo_Admin()
		{
			"~/admin".ShouldMapTo<AdminController>(a => a.Index());
		}
		
		[TestMethod]
		public void Routes_AdminIndex_RoutesTo_AdminIndex()
		{
			"~/admin/index".ShouldMapTo<AdminController>(a => a.Index());
		}

		[TestMethod]
		public void Routes_AdminLogs_RoutesTo_AdminLogs()
		{
			"~/admin/logs".ShouldMapTo<AdminController>(a => a.Logs());
		}

		[TestMethod]
		public void Routes_AdminLogsClear_RoutesTo_AdminLogsClear()
		{
			"~/admin/logs/clear".ShouldMapTo<AdminController>(a => a.ClearLogs());
		}

		[TestMethod]
		public void Routes_AdminLogsCompact_RoutesTo_AdminLogsCompact()
		{
			"~/admin/logs/compact".ShouldMapTo<AdminController>(a => a.CompactLogs());
		}
        #endregion

//        //[TestMethod]
//        //public void Routes_Username_RoutesTo_UserIndex()
//        //{
//        //    "~/sam.mvc.aspx".Route().ShouldMapTo<UserController>(a => a.Index("sam"));
//        //}

        #region Blog Routes
        [TestMethod]
        public void Routes_UsernameBlogList_RoutesTo_BlogList()
        {
            "~/sam/blog".ShouldMapTo<BlogController>(a => a.List("sam"));
        }

        [TestMethod]
        public void Routes_UsernameBlogUserList_RoutesTo_BlogList()
        {
            "~/sam/blog/userlist".ShouldMapTo<BlogController>(a => a.UserList("sam"));
        }

        [TestMethod]
        public void Routes_UsernameBlogListFeed_RoutesTo_BlogFeed()
        {
            "~/sam/blog/list.xml.aspx".ShouldMapTo<BlogController>(a => a.Feed("sam"));
        }

        //[TestMethod]
        //public void Routes_UsernameBlogListYear_RoutesTo_BlogList()
        //{
        //    "~/sam/Blog/List.mvc.aspx/2009".ShouldMapTo<BlogController>(a => a.List("sam", "2009"));
        //}

        //[TestMethod]
        //public void Routes_UsernameBlogListYearMonth_RoutesTo_BlogList()
        //{
        //    "~/sam/Blog/List.mvc.aspx/2009/08".ShouldMapTo<BlogController>(a => a.List("sam", "2009/08"));
        //}

        //[TestMethod]
        //public void Routes_UsernameBlogListYearMonthDay_RoutesTo_BlogList()
        //{
        //    "~/sam/Blog/List.mvc.aspx/2009/08/07".ShouldMapTo<BlogController>(a => a.List("sam", "2009/08/07"));
        //}

        [TestMethod]
        public void Routes_UsernameBlogCreate_RoutesTo_BlogCreate()
        {
            "~/sam/blog/create.aspx".ShouldMapTo<BlogController>(a => a.Create("sam"));
        }

        [TestMethod]
        public void Routes_UsernameBlogEdit_RoutesTo_BlogEdit()
        {
            "~/sam/blog/edit/balgslk.aspx".ShouldMapTo<BlogController>(a => a.Edit("sam", "balgslk"));
        }             

        [TestMethod]
        public void Routes_UsernameBlogDetailTitle_RoutesTo_BlogDetail()
        {
            "~/sam/blog/blah-as.aspx".ShouldMapTo<BlogController>(a => a.Detail("blah-as"));
        }
        #endregion

        #region Photo Routes
        [TestMethod]
        public void Routes_PhotoUpload_RoutesTo_Upload()
        {
            "~/sam/photo/upload".ShouldMapTo<PhotoController>(a => a.Upload());
        }

        [TestMethod]
        public void Routes_PhotoUploadPhoto_RoutesTo_UploadPhoto()
        {
            "~/sam/photo/uploadphoto".ShouldMapTo<PhotoController>(a => a.UploadPhoto(null));
        }
        #endregion

//        // Account Profile
//        //[TestMethod]
//        //public void Routes_UsernameAccount_RoutesTo_AccountProfile()
//        //{
//        //    "~/sam.mvc.aspx/Account".ShouldMapTo<AccountController>(a => a.Profile("sam"));
		//        //}

		#region Content Routes
		[TestMethod]
		public void Routes_UserContent_RoutesTo_UserContent()
		{
			"~/sam/content".ShouldMapTo<ContentController>(c => c.Index("sam"));
		}
		#endregion

		#region Setup
		[TestInitialize]
        public void Setup()
        {
            WilliamsonFamily.Web.MvcApplication.RegisterRoutes();
        }
        #endregion
    }
}