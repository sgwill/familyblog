using System.Web.Routing;
using NUnit.Framework;
using MvcContrib.TestHelper;
using WilliamsonFamily.Web.Controllers;
using Rhino.Mocks;
using WilliamsonFamily.Web.Models.Blog;

namespace WilliamsonFamily.Tests.Web
{
    [TestFixture]
    public class RouteTests
    {
        #region Home Routes
        [Test]
        public void Routes_EmptyString_RoutesTo_HomeIndex()
        {
            "~/".Route().ShouldMapTo<HomeController>(a => a.Index());
        }
        
        [Test]
        public void Routes_Home_RoutesTo_HomeIndex()
        {
            "~/Home.aspx".ShouldMapTo<HomeController>(a => a.Index());
        }

        [Test]
        public void Routes_HomeLogin_RoutesTo_AccountLogin()
        {
            "~/Home/Login.aspx".ShouldMapTo<AccountController>(a => a.Login());
        }

        [Test]
        public void Routes_HomeLogout_RoutesTo_AccountLogout()
        {
            "~/Home/Logout.aspx".ShouldMapTo<AccountController>(a => a.Logout());
        }
        #endregion

        #region Admin routes
        [Test]
        public void Routes_AdminIndex_RoutesTo_AdminIndex()
        {
            "~/Admin/Index.aspx".ShouldMapTo<AdminController>(a => a.Index());
        }

        [Test]
        public void Routes_AdminUsers_RoutesTo_AdminUsers()
        {
            "~/Admin/Users.aspx".ShouldMapTo<AdminController>(a => a.Users());
        }

        [Test]
        public void Routes_AdminUserID_RoutesTo_AdminUserWithID()
        {
            "~/Admin/UserDetail.aspx/s".ShouldMapTo<AdminController>(a => a.UserDetail("s"));
        }
        #endregion

        //[Test]
        //public void Routes_Username_RoutesTo_UserIndex()
        //{
        //    "~/sam.mvc.aspx".Route().ShouldMapTo<UserController>(a => a.Index("sam"));
        //}

        #region Blog Routes
        [Test]
        public void Routes_UsernameBlogList_RoutesTo_BlogList()
        {
            "~/sam/Blog/List.aspx".ShouldMapTo<BlogController>(a => a.List("sam", ""));
        }

        [Test]
        public void Routes_UsernameBlogListFeed_RoutesTo_BlogFeed()
        {
            "~/sam/Blog/List.xml.aspx".ShouldMapTo<BlogController>(a => a.Feed("sam"));
        }

        //[Test]
        //public void Routes_UsernameBlogListYear_RoutesTo_BlogList()
        //{
        //    "~/sam/Blog/List.mvc.aspx/2009".ShouldMapTo<BlogController>(a => a.List("sam", "2009"));
        //}

        //[Test]
        //public void Routes_UsernameBlogListYearMonth_RoutesTo_BlogList()
        //{
        //    "~/sam/Blog/List.mvc.aspx/2009/08".ShouldMapTo<BlogController>(a => a.List("sam", "2009/08"));
        //}

        //[Test]
        //public void Routes_UsernameBlogListYearMonthDay_RoutesTo_BlogList()
        //{
        //    "~/sam/Blog/List.mvc.aspx/2009/08/07".ShouldMapTo<BlogController>(a => a.List("sam", "2009/08/07"));
        //}

        [Test]
        public void Routes_UsernameBlogCreate_RoutesTo_BlogCreate()
        {
            "~/sam/Blog/Create.aspx".ShouldMapTo<BlogController>(a => a.Create("sam"));
        }

        [Test]
        public void Routes_UsernameBlogEdit_RoutesTo_BlogEdit()
        {
            "~/sam/Blog/Edit/balgslk.aspx".ShouldMapTo<BlogController>(a => a.Edit("sam", "balgslk"));
        }

        [Test]
        public void Routes_UsernameBlogSubmitComment_RoutesTo_PostComment()
        {
            "~/sam/Blog/PostComment.aspx".ShouldMapTo<BlogController>(a => a.PostComment(Arg<BlogCommentModel>.Is.Anything));
        }

        [Test]
        public void Routes_UsernameBlogDetailTitle_RoutesTo_BlogDetail()
        {
            "~/sam/Blog/blah-as.aspx".ShouldMapTo<BlogController>(a => a.Detail("blah-as"));
        }
        #endregion

        #region Photo Routes
        [Test]
        public void Routes_PhotoUpload_RoutesTo_PhotoUpload()
        {
            "~/Photo/Upload.aspx".ShouldMapTo<PhotoController>(a => a.Upload());
        }
        #endregion

        // Account Profile
        //[Test]
        //public void Routes_UsernameAccount_RoutesTo_AccountProfile()
        //{
        //    "~/sam.mvc.aspx/Account".ShouldMapTo<AccountController>(a => a.Profile("sam"));
        //}

        #region Setup
        [TestFixtureSetUp]
        public void Setup()
        {
            WilliamsonFamily.Web.MvcApplication.RegisterRoutes();
        }
        #endregion
    }
}
