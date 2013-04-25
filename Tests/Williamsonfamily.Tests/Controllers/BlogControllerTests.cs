using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Rhino.Mocks;
using WilliamsonFamily.Library.Web;
using WilliamsonFamily.Library.Web.Caching;
using WilliamsonFamily.Models;
using WilliamsonFamily.Models.Blog;
using WilliamsonFamily.Models.Caching;
using WilliamsonFamily.Models.Communication;
using WilliamsonFamily.Models.Family;
using WilliamsonFamily.Models.User;
using WilliamsonFamily.Models.Web;
using WilliamsonFamily.Web.Controllers;
using WilliamsonFamily.Web.Models.Blog;
using WilliamsonFamily.Web.Tests.Helpers;

namespace WilliamsonFamily.Web.Tests.Controllers
{
    [TestClass]
    public class BlogControllerTests
    {
        #region List Tests
        [TestMethod]
        public void List_InvalidUserAndFamily_RedirectsToHomeIndex()
        {
            // Assert
            controller.List("")
                .AssertActionRedirect()
                .ToController("Home")
                .ToAction("Index");
                
            controller
                .TempData["Message"]
                .ShouldBe("Invalid User or Family");
        }

        //[TestMethod]
        //public void BlogController_List_ViewData_IsBlogListModel()
        //{
        //    string username = "sgwill";

        //    // Arrange
        //    controller.UserRepository
        //        .Expect(u => u.Load(Arg<string>.Is.Anything))
        //        .Return(MockRepository.GenerateStub<IUser>());
        //    controller.BlogRepository
        //        .Expect(b => b.LoadList(Arg<BlogFilter>.Is.Anything))
        //        .Return(MockRepository.GenerateStub<IBlogList>());

        //    // Act
        //    controller.List(username, "");
            
        //    // Assert
        //    Assert.IsInstanceOfType(controller.ViewData.Model, typeof(BlogListModel));
        //}

//        [TestMethod]
//        public void BlogController_List_RendersList()
//        {
//            string username = "sgwill";

//            // Arrange
//            controller.UserRepository
//                .Expect(u => u.Load(Arg<string>.Is.Anything))
//                .Return(MockRepository.GenerateStub<IUser>());
//            controller.BlogRepository
//                .Expect(b => b.LoadList(Arg<BlogFilter>.Is.Anything))
//                .Return(MockRepository.GenerateStub<IEnumerable<IBlog>>());

//            // Assert
//            controller.List(username, "")
//                .AssertViewRendered()
//                .ForView("List");
//        }

//        [TestMethod]
//        public void BlogController_List_ValidUserWithEmptyBlogEntries_ReturnsEmptyList()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.User
//                        && b.AuthorName == uniqueKey)))
//                .Return(new List<IBlog>());

//            // Act
//            var result = controller.List(username, "");

//            // Assert
//            result
//                .AssertViewRendered()
//                .ForView("List");
//            Assert.AreEqual(0, ((BlogListModel)controller.ViewData.Model).BlogEntries.Count());
//        }

//        [TestMethod]
//        public void BlogController_List_ValidUserWithBlogEntries_ReturnsBlogEntries()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";
            
//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);
            
//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.User
//                        && b.AuthorName == uniqueKey)))
//                .Return(new List<IBlog> { blogentry });

//            // Act
//            var result = controller.List(username, "");

//            // Assert
//            result
//                .AssertViewRendered()
//                .ForView("List");
//            Assert.AreEqual(1, ((BlogListModel)controller.ViewData.Model).BlogEntries.Count());
//            Assert.AreEqual(blogentry, ((BlogListModel)controller.ViewData.Model).BlogEntries.FirstOrDefault());
//        }

//        [TestMethod]
//        public void BlogController_List_InvalidUserButValidFamilyWithoutBlogEntries_ReturnsEmptyList()
//        {
//            string familyName = "wf";

//            // Arrange
//            var family = MockRepository.GenerateStub<IFamily>();
//            controller.FamilyRepository
//                .Expect(u => u.Load(familyName))
//                .Return(family);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.Family
//                        && b.AuthorName == familyName)))
//                .Return(new List<IBlog>());

//            // Act
//            var result = controller.List(familyName, "");

//            // Assert
//            result
//                .AssertViewRendered()
//                .ForView("List");
//            Assert.AreEqual(0, ((BlogListModel)controller.ViewData.Model).BlogEntries.Count());
//        }

//        [TestMethod]
//        public void BlogController_List_ValidFamilyWithBlogEntries_ReturnsEntries()
//        {
//            string uniqueKey = "1234";
//            string familyName = "wf";

//            // Arrange
//            var family = MockRepository.GenerateStub<IFamily>();
//            controller.FamilyRepository
//                .Expect(u => u.Load(familyName))
//                .Return(family);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            DateTime time = DateTime.Now;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(time);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.Family
//                        && b.AuthorName == familyName)))
//                .Return(new List<IBlog> { blogentry });

//            // Act
//            var result = controller.List(familyName, "");

//            // Assert
//            result
//                .AssertViewRendered()
//                .ForView("List");
//            Assert.AreEqual(blogentry, ((BlogListModel)controller.ViewData.Model).BlogEntries.FirstOrDefault());
//        }

//        [TestMethod]
//        public void BlogController_List_UserBlogs_SortedByNewest()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now.AddDays(-1));
//            blogentry.Title = "title1";
//            var blogentry2 = MockRepository.GenerateStub<IBlog>();
//            blogentry2.AuthorID = uniqueKey;
//            blogentry2
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            blogentry2.Title = "title2";
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.User
//                        && b.AuthorName == uniqueKey)))
//                .Return(new List<IBlog> { blogentry, blogentry2 });

//            // Act
//            var result = controller.List(username, "");

//            // Assert
//            var viewData = controller.ViewData.Model as BlogListModel;
//            Assert.AreEqual("title2", viewData.BlogEntries.FirstOrDefault().Title);
//        }

//        [TestMethod]
//        public void BlogController_List_UserBlogs_LoadsOnlyPublished()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now.AddDays(-1));
//            blogentry.Title = "title1";
//            blogentry.IsPublished = true;
//            var blogentry2 = MockRepository.GenerateStub<IBlog>();
//            blogentry2.AuthorID = uniqueKey;
//            blogentry2
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            blogentry2.Title = "title2";
//            blogentry.IsPublished = false;
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.User
//                        && b.AuthorName == uniqueKey
//                        && b.IsPublished == true)))
//                .Return(new List<IBlog> { blogentry });

//            // Act
//            var result = controller.List(username, "");

//            // Assert
//            var viewData = controller.ViewData.Model as BlogListModel;
//            Assert.AreEqual(1, viewData.BlogEntries.Count());
//        }

//        [TestMethod]
//        public void BlogController_List_FamilyBlogs_SortedByNewest()
//        {
//            string uniqueKey = "1234";
//            string familyName = "wf";

//            // Arrange
//            var family = MockRepository.GenerateStub<IFamily>();
//            controller.FamilyRepository
//                .Expect(u => u.Load(familyName))
//                .Return(family);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now.AddDays(-1));
//            blogentry.Title = "title1";
//            var blogentry2 = MockRepository.GenerateStub<IBlog>();
//            blogentry2.AuthorID = uniqueKey;
//            blogentry2
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            blogentry2.Title = "title2";
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.Family
//                        && b.AuthorName == familyName)))
//                .Return(new List<IBlog> { blogentry, blogentry2 });

//            // Act
//            var result = controller.List(familyName, "");

//            // Assert
//            var viewData = controller.ViewData.Model as BlogListModel;
//            Assert.AreEqual("title2", viewData.BlogEntries.FirstOrDefault().Title);
//        }

//        [TestMethod]
//        public void BlogController_List_FamilyBlog_LoadsFromRepositoryIfCacheIsNull()
//        {
//            string uniqueKey = "1234";
//            string familyName = "wf";

//            // Arrange
//            var family = MockRepository.GenerateStub<IFamily>();
//            controller.FamilyRepository
//                .Expect(u => u.Load(familyName))
//                .Return(family);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.Family
//                        && b.AuthorName == familyName)))
//                .Return(new List<IBlog> { MockRepository.GenerateStub<IBlog>() });

//            // Act
//            var result = controller.List(familyName, "");

//            // Assert
//            controller.BlogRepository
//                .AssertWasCalled(b => b.LoadList(
//                        Arg<BlogFilter>.Matches(c =>
//                        c.LoadBlogBy == LoadBlogBy.Family
//                        && c.AuthorName == familyName)));
//        }

//        [TestMethod]
//        public void BlogController_List_FamilyBlog_LoadsFromCacheIfCacheIsNotNull()
//        {
//            string familyName = "wf";

//            // Arrange
//            var family = MockRepository.GenerateStub<IFamily>();
//            controller.FamilyRepository
//                .Expect(u => u.Load(familyName))
//                .Return(family);
//            ICacheKey cacheKey = new BlogListCacheKey();
//            controller.Cache.Insert(cacheKey.GenerateKey(""), new List<IBlog> { MockRepository.GenerateStub<IBlog>() });

//            // Act
//            var result = controller.List(familyName, "");

//            // Assert
//            controller.BlogRepository
//                .AssertWasNotCalled(b => b.LoadList(Arg<BlogFilter>.Is.Anything));
//        }

//        [TestMethod]
//        public void BlogController_List_FamilyWithBlogEntries_SetsAuthorInfo()
//        {
//            string uniqueKey = "1234";
//            string familyName = "wf";
//            string familyDesc = "Sam and Michele";

//            // Arrange
//            var family = MockRepository.GenerateStub<IFamily>();
//            family.Description = familyDesc;
//            controller.FamilyRepository
//                .Expect(u => u.Load(familyName))
//                .Return(family);
//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            DateTime time = DateTime.Now;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(time);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.Family
//                        && b.AuthorName == familyName)))
//                .Return(new List<IBlog> { blogentry });

//            // Act
//            var result = controller.List(familyName, "");

//            // Assert
//            Assert.AreEqual(familyDesc, ((BlogListModel)controller.ViewData.Model).Author.FirstName);
//            Assert.AreEqual(familyName, ((BlogListModel)controller.ViewData.Model).Author.UrlName);
//        }

//        [TestMethod]
//        public void BlogController_List_BlogWithBlogEntries_SetsAuthorInfo()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";
//            string firstname = "Sam";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            user.FirstName = firstname;
//            user.Username = username;
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.User
//                        && b.AuthorName == uniqueKey)))
//                .Return(new List<IBlog> { blogentry });

//            // Act
//            var result = controller.List(username, "");
//            // Assert
//            Assert.AreEqual(firstname, ((BlogListModel)controller.ViewData.Model).Author.FirstName);
//            Assert.AreEqual(username, ((BlogListModel)controller.ViewData.Model).Author.UrlName);
//        }
        #endregion

        #region Feed Tests
//        // TODO: Yeah yeah, I didn't test feeds. Damn.
        #endregion

//        // Todo: Tags
//        // Todo: Dates

        #region List - Date Range
//        [TestMethod]
//        public void BlogController_List_DateRange_ReturnsBlogEntries()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";
//            string dateRange = "2009";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user.Expect(u => u.UniqueKey).Return(uniqueKey);
//            controller.UserRepository.Expect(u => u.Load(username)).Return(user);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry.Expect(b => b.DatePublished).Return(DateTime.Now);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.User
//                        && b.AuthorName == uniqueKey
//                        && b.Date == dateRange)))
//                .Return(new List<IBlog> { blogentry });

//            // Act
//            var result = controller.List(username, dateRange);

//            // Assert
//            result.AssertViewRendered().ForView("List");
//            Assert.AreEqual(blogentry, ((BlogListModel)controller.ViewData.Model).BlogEntries.FirstOrDefault());
//        }

        #endregion

        #region List - Admin
//        [TestMethod]
//        public void BlogController_UserList_InvalidUser_RedirectsToAccountIndex()
//        {
//            // Assert
//            controller.UserList("")
//                .AssertActionRedirect()
//                .ToAction("Index")
//                .ToController("Account");
//            controller
//                .TempData["Message"]
//                .ShouldBe("Invalid User or Family");
//        }

//        [TestMethod]
//        public void BlogController_UserList_ViewData_IsBlogListModel()
//        {
//            string username = "sgwill";

//            // Arrange
//            controller.UserRepository
//                .Expect(u => u.Load(Arg<string>.Is.Anything))
//                .Return(MockRepository.GenerateStub<IUser>());
//            controller.BlogRepository
//                .Expect(b => b.LoadList(Arg<BlogFilter>.Is.Anything))
//                .Return(MockRepository.GenerateStub<IEnumerable<IBlog>>());

//            // Act
//            controller.UserList(username);

//            // Assert
//            Assert.IsInstanceOfType(controller.ViewData.Model, typeof(BlogListModel));
//        }

//        [TestMethod]
//        public void BlogController_UserList_RendersUserList()
//        {
//            string username = "sgwill";

//            // Arrange
//            controller.UserRepository
//                .Expect(u => u.Load(Arg<string>.Is.Anything))
//                .Return(MockRepository.GenerateStub<IUser>());
//            controller.BlogRepository
//                .Expect(b => b.LoadList(Arg<BlogFilter>.Is.Anything))
//                .Return(MockRepository.GenerateStub<IEnumerable<IBlog>>());

//            // Assert
//            controller.UserList(username)
//                .AssertViewRendered()
//                .ForView("UserList");
//        }

//        [TestMethod]
//        public void BlogController_UserList_ValidUserWithBlogEntries_ReturnsBlogEntries()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.User
//                        && b.AuthorName == uniqueKey)))
//                .Return(new List<IBlog> { blogentry });

//            // Act
//            var result = controller.UserList(username);

//            // Assert
//            Assert.AreEqual(1, ((BlogListModel)controller.ViewData.Model).BlogEntries.Count());
//            Assert.AreEqual(blogentry, ((BlogListModel)controller.ViewData.Model).BlogEntries.FirstOrDefault());
//        }

//        [TestMethod]
//        public void BlogController_UserList_UserBlogs_SortedByIsPublished_ThenByNewest()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry.IsPublished = true;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now.AddDays(-1));
//            blogentry.Title = "title1";
//            var blogentry2 = MockRepository.GenerateStub<IBlog>();
//            blogentry2.AuthorID = uniqueKey;
//            blogentry2
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            blogentry2.Title = "title2";
//            blogentry2.IsPublished = true;
//            var blogentry3 = MockRepository.GenerateStub<IBlog>();
//            blogentry3.AuthorID = uniqueKey;
//            blogentry3
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now.AddDays(-4));
//            blogentry3.Title = "title3";
//            blogentry3.IsPublished = false;
//            controller.BlogRepository
//            .Expect(u => u.LoadList(
//                Arg<BlogFilter>.Matches(b =>
//                    b.LoadBlogBy == LoadBlogBy.User
//                    && b.AuthorName == uniqueKey)))
//                    .Return(new List<IBlog> { blogentry, blogentry2, blogentry3 });

//            // Act
//            var result = controller.UserList(username);

//            // Assert
//            var viewData = controller.ViewData.Model as BlogListModel;
//            Assert.AreEqual("title3", viewData.BlogEntries.FirstOrDefault().Title);
//            //
//            // NOTE: This will fail because of rhino mocks; eh
//            //Assert.AreEqual("title2", viewData.BlogEntries.FirstOrDefault(b => b.Title != "title3").Title);
//        }

//        [TestMethod]
//        public void BlogController_UserList_UserBlogs_LoadsPublishedANDUnPublished()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry.IsPublished = true;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now.AddDays(-1));
//            blogentry.Title = "title1";
//            var blogentry2 = MockRepository.GenerateStub<IBlog>();
//            blogentry2.AuthorID = uniqueKey;
//            blogentry2
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            blogentry2.Title = "title2";
//            blogentry2.IsPublished = true;
//            var blogentry3 = MockRepository.GenerateStub<IBlog>();
//            blogentry3.AuthorID = uniqueKey;
//            blogentry3
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now.AddDays(-4));
//            blogentry3.Title = "title3";
//            blogentry3.IsPublished = false;
//            controller.BlogRepository
//            .Expect(u => u.LoadList(
//                Arg<BlogFilter>.Matches(b =>
//                    b.LoadBlogBy == LoadBlogBy.User
//                    && b.AuthorName == uniqueKey)))
//                    .Return(new List<IBlog> { blogentry, blogentry2, blogentry3 });

//            // Act
//            var result = controller.UserList(username);

//            // Assert
//            var viewData = controller.ViewData.Model as BlogListModel;
//            Assert.AreEqual(3, viewData.BlogEntries.Count());
//        }

//        [TestMethod]
//        public void BlogController_UserList_BlogWithBlogEntries_SetsAuthorInfo()
//        {
//            string uniqueKey = "1234";
//            string username = "sgwill";
//            string firstname = "Sam";

//            // Arrange
//            var user = MockRepository.GenerateStub<IUser>();
//            user
//                .Expect(u => u.UniqueKey)
//                .Return(uniqueKey);
//            user.FirstName = firstname;
//            user.Username = username;
//            controller.UserRepository
//                .Expect(u => u.Load(username))
//                .Return(user);

//            var blogentry = MockRepository.GenerateStub<IBlog>();
//            blogentry.AuthorID = uniqueKey;
//            blogentry
//                .Expect(b => b.DatePublished)
//                .Return(DateTime.Now);
//            controller.BlogRepository
//                .Expect(u => u.LoadList(
//                    Arg<BlogFilter>.Matches(b =>
//                        b.LoadBlogBy == LoadBlogBy.User
//                        && b.AuthorName == uniqueKey)))
//                .Return(new List<IBlog> { blogentry });

//            // Act
//            var result = controller.UserList(username);
//            // Assert
//            Assert.AreEqual(firstname, ((BlogListModel)controller.ViewData.Model).Author.FirstName);
//            Assert.AreEqual(username, ((BlogListModel)controller.ViewData.Model).Author.UrlName);
//        }

        #endregion

        #region Detail
		private IBlog Setup_Detail_Blog(string title)
		{
			return Setup_Detail_Blog(title, "blah");
		}

		private IBlog Setup_Detail_Blog(string title, string author)
		{
			var entry = MockRepository.GenerateStub<IBlog>();
			entry
				.Expect(e => e.Slug)
				.Return(title);
            entry.AuthorName = author;
			entry.AuthorID = author;
			controller.BlogRepository
				.Expect(b => b.LoadBySlug(title))
				.Return(entry);

			return entry;
		}

		[TestMethod]
		public void Detail_ViewData_IsBlogModel()
		{
			// Arrange
			Setup_Detail_Blog("blog");

			// Act
			controller.Detail("blog");

			// Assert
			Assert.IsInstanceOfType(controller.ViewData.Model, typeof(BlogModel));
		}

		[TestMethod]
		public void Detail_RendersDetailsView()
		{
			// Arrange
			Setup_Detail_Blog("blog");

			// Act
			var result = controller.Detail("blog");

			// Assert
			result
				.AssertViewRendered()
				.ForView("Details");
		}

        [TestMethod]
        public void Detail_EmptyTitle_RedirectsToList()
        {
            // Assert
            controller.Detail("")
                .AssertActionRedirect()
                .ToAction("List");
        }

		[TestMethod]
		public void Detail_Initializes_Author()
		{
			// Arrange
			Setup_Detail_Blog("blog");

			// Act
			var result = controller.Detail("blog");

			// Assert
			Assert.IsNotNull(((BlogModel)controller.ViewData.Model).Author);
		}

		[TestMethod]
		public void Detail_Sets_Author()
		{
			// Arrange
			Setup_Detail_Blog("blog", "user");
			IUser user = MockRepository.GenerateStub<IUser>();
			user.Username = "user";
			user.FirstName = "sam";
			controller.UserRepository
				.Expect(u => u.Load("user"))
				.Return(user);

			// Act
			var result = controller.Detail("blog");

			// Assert
			Assert.AreEqual("sam", ((BlogModel)controller.ViewData.Model).Author.FirstName);
			Assert.AreEqual("user", ((BlogModel)controller.ViewData.Model).Author.UrlName);
		}

		[TestMethod]
		public void Detail_Title_ReturnsBlogEntry()
		{
			string title = "title";

			// Arrange
			var entry = Setup_Detail_Blog(title, "user");

			// Act
			controller.Detail(title);

			// Assert
			Assert.AreEqual(entry, ((BlogModel)controller.ViewData.Model).BlogEntry);
		}

//        [TestMethod]
//        public void BlogController_Detail_LoadsFamilyEntriesFromCacheIfCacheIsNotNull()
//        {
//            string title = "title";

//            // Arrange
//            var entry = MockRepository.GenerateStub<IBlog>();
//            entry
//                .Expect(e => e.Slug)
//                .Return(title);
//            entry.AuthorID = "blah";
//            controller.BlogRepository
//                .Expect(b => b.LoadBySlug(title))
//                .Return(entry);
//            ICacheKey cacheKey = new BlogListCacheKey();
//            controller.Cache
//                .Expect(c => c.Get<IEnumerable<IBlog>>(cacheKey.GenerateKey(""), null))
//                .Return(new List<IBlog> { MockRepository.GenerateStub<IBlog>() });

//            // Act
//            controller.Detail(title);

//            // Assert
//            Assert.IsNotNull(((BlogModel)controller.ViewData.Model).FamilyEntries);
            
//        }

        [TestMethod]
        public void Detail_InvalidTitle_RedirectsToList()
        {
            controller.Detail("title")
                .AssertActionRedirect()
                .ToAction("List");
            controller
                .TempData["Message"]
                .ShouldBe("No Blog");
        }
        #endregion

        #region Create - Get
//        [TestMethod]
//        public void Blogcontroller_CreateGet_RendersCreate()
//        {
//            string userName = "sam";
//            string id = "12345";

//            // Arrange
//            var identity = MockRepository.GenerateStub<IIdentity>();
//            identity
//                .Stub(u => u.Name)
//                .Return(userName);
//            var user = MockRepository.GenerateStub<IPrincipal>();
//            user
//                .Stub(u => u.Identity)
//                .Return(identity);
//            controller.HttpContext.User = user;
//            var dbUser = MockRepository.GenerateStub<IUser>();
//            dbUser.Username = userName;
//            dbUser
//                .Stub(u => u.UniqueKey)
//                .Return(id);
//            controller.UserRepository
//                .Expect(u => u.Load(userName))
//                .Return(dbUser);

//            // Act
//            var result = controller.Create(userName);

//            // Assert
//            result.AssertViewRendered()
//                .ForView("Create");
//        }

//        [TestMethod]
//        public void BlogController_CreateGet_ViewData_IsBlogCreateModel()
//        {
//            string userName = "sam";
//            string id = "12345";

//            // Arrange
//            var identity = MockRepository.GenerateStub<IIdentity>();
//            identity
//                .Stub(u => u.Name)
//                .Return(userName);
//            var user = MockRepository.GenerateStub<IPrincipal>();
//            user
//                .Stub(u => u.Identity)
//                .Return(identity);
//            controller.HttpContext.User = user;
//            var dbUser = MockRepository.GenerateStub<IUser>();
//            dbUser.Username = userName;
//            dbUser
//                .Stub(u => u.UniqueKey)
//                .Return(id);
//            controller.UserRepository
//                .Expect(u => u.Load(userName))
//                .Return(dbUser);

//            // Act
//            controller.Create(userName);

//            // Assert
//            Assert.IsInstanceOfType(controller.ViewData.Model, typeof(BlogCreateModel));
//        }

//        [TestMethod]
//        public void BlogController_CreateGet_ViewData_UserGetsSet()
//        {
//            string userName = "sam";
//            string id = "12345";

//            // Arrange
//            var identity = MockRepository.GenerateStub<IIdentity>();
//            identity
//                .Stub(u => u.Name)
//                .Return(userName);
//            var user = MockRepository.GenerateStub<IPrincipal>();
//            user
//                .Stub(u => u.Identity)
//                .Return(identity);
//            controller.HttpContext.User = user;
//            var dbUser = MockRepository.GenerateStub<IUser>();
//            dbUser.Username = userName;
//            dbUser
//                .Stub(u => u.UniqueKey)
//                .Return(id);
//            controller.UserRepository
//                .Expect(u => u.Load(userName))
//                .Return(dbUser);

//            // Act
//            controller.Create(userName);

//            // Assert
//            Assert.AreEqual(dbUser.UniqueKey, ((BlogCreateModel)controller.ViewData.Model).AuthorID);
//        }

//        [TestMethod]
//        public void BlogController_CreateGet_RequestingUserDifferentThanAuthenticatedUserRedirectsToAuthenticatedUser()
//        {
//            string firstUser = "sam";
//            string secondUser = "david";

//            // Arrange
//            var identity = MockRepository.GenerateStub<IIdentity>();
//            identity
//                .Stub(u => u.Name)
//                .Return(firstUser);
//            var user = MockRepository.GenerateStub<IPrincipal>();
//            user
//                .Stub(u => u.Identity)
//                .Return(identity);
//            controller.HttpContext.User = user;
            
//            // Act
//            var result = controller.Create(secondUser);

//            // Assert
//            result.AssertActionRedirect().ToAction("Create").WithParameter("user", firstUser);
//        }

//        [TestMethod]
//        public void BlogController_CreateGet_InvalidDBUserRedirectsHome()
//        {
//            string userName = "sam";

//            // Arrange
//            var identity = MockRepository.GenerateStub<IIdentity>();
//            identity
//                .Stub(u => u.Name)
//                .Return(userName);
//            var user = MockRepository.GenerateStub<IPrincipal>();
//            user
//                .Stub(u => u.Identity)
//                .Return(identity);
//            controller.HttpContext.User = user;

//            // Act
//            var result = controller.Create(userName);

//            // Assert
//            result.AssertActionRedirect().ToAction("Index").ToController("Home");
//        }

//        [TestMethod]
//        public void BlogController_CreateGet_FamilyNameRedirectsHomewithMessage()
//        {
//            string userName = "sam";
//            string familyName = "wf";

//            // Arrange
//            var identity = MockRepository.GenerateStub<IIdentity>();
//            identity
//                .Stub(u => u.Name)
//                .Return(userName);
//            var user = MockRepository.GenerateStub<IPrincipal>();
//            user
//                .Stub(u => u.Identity)
//                .Return(identity);
//            controller.HttpContext.User = user;
//            var family = MockRepository.GenerateStub<IFamily>();
//            controller.FamilyRepository
//                .Expect(u => u.Load(userName))
//                .Return(family);

//            // Act
//            var result = controller.Create(userName);

//            // Assert
//            result.AssertActionRedirect().ToAction("Index").ToController("Home");
//            controller.TempData["Message"].ShouldBe("Cannot create blog entries as Family");
//        }

//        [TestMethod]
//        public void BlogController_CreateGet_SetsIsPublishedToFalse()
//        {
//            string userName = "sam";
//            string id = "12345";

//            // Arrange
//            var identity = MockRepository.GenerateStub<IIdentity>();
//            identity
//                .Stub(u => u.Name)
//                .Return(userName);
//            var user = MockRepository.GenerateStub<IPrincipal>();
//            user
//                .Stub(u => u.Identity)
//                .Return(identity);
//            controller.HttpContext.User = user;
//            var dbUser = MockRepository.GenerateStub<IUser>();
//            dbUser.Username = userName;
//            dbUser
//                .Stub(u => u.UniqueKey)
//                .Return(id);
//            controller.UserRepository
//                .Expect(u => u.Load(userName))
//                .Return(dbUser);

//            // Act
//            var result = controller.Create(userName);

//            // Assert
//            Assert.IsFalse(((BlogCreateModel)controller.ViewData.Model).IsPublished);
//        }
        #endregion

        #region Create - Post
//        [TestMethod]
//        public void BlogController_CreatePost_InsertsNewBlogEntry()
//        {
//            // Arrange
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Title = "title";
//            model.Entry = "entry";
//            model.AuthorID = "author";
//            controller.BlogRepository
//                .Expect(b => b.New())
//                .Return(MockRepository.GenerateStub<IBlog>());

//            // Act
//            controller.Create(model);
            
//            // Assert
//            controller.BlogRepository
//                .AssertWasCalled(c => c.Save(Arg<IBlog>.Is.Anything));
//        }

//        [TestMethod]
//        public void BlogController_CreatePost_ReturnToUserListAfterSuccess()
//        {
//            // Arrange
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Title = "title";
//            model.Entry = "entry";
//            model.AuthorID = "author";
//            controller.BlogRepository
//                .Expect(b => b.New())
//                .Return(MockRepository.GenerateStub<IBlog>());
            
//            // Act
//            var result = controller.Create(model);

//            // Assert
//            result.AssertActionRedirect().ToAction("UserList");
//        }

//        [TestMethod]
//        public void BlogController_CreatePost_EmptyTitle_ReturnsWithError()
//        {
//            // Arrange
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Entry = "entry";
//            model.Title = "";

//            // Assert
//            controller.Create(model)
//                .AssertViewRendered()
//                .ForView("Create");
//            Assert.IsFalse(controller.ModelState.IsValid);
//            Assert.IsTrue(controller.ModelState.ContainsKey("Title"));
//        }

//        [TestMethod]
//        public void BlogController_CreatePost_EmptyEntry_ReturnsWithError()
//        {
//            // Arrange
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Title = "title";

//            // Assert
//            controller.Create(model)
//                .AssertViewRendered()
//                .ForView("Create");
//            Assert.IsFalse(controller.ModelState.IsValid);
//            Assert.IsTrue(controller.ModelState.ContainsKey("Entry"));
//        }

//        [TestMethod]
//        public void BlogController_CreatePost_EmptyEntryWithFormValue_SetsEntry()
//        {
//            // Arrange
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Title = "title";
//            controller.Request.Form["textEntry"] = "value";
//            model.AuthorID = "author";
//            controller.BlogRepository
//                .Expect(b => b.New())
//                .Return(MockRepository.GenerateStub<IBlog>());

//            // Act
//            var result = controller.Create(model);

//            // Assert
//            result.AssertActionRedirect().ToAction("UserList");
//        }

//        [TestMethod]
//        public void BlogController_CreatePost_EntryWithDuplicateTitle_ReturnsError()
//        {
//            // Arrange
//            string title = "testtitle";
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Title = title;
//            model.Entry = "entry";
//            controller.BlogRepository
//                .Expect(b => b.LoadBySlug(model.Title))
//                .Return(MockRepository.GenerateStub<IBlog>());

//            // Act
//            var result = controller.Create(model);

//            // Assert
//            controller.Create(model)
//                .AssertViewRendered()
//                .ForView("Create");
//            Assert.IsFalse(controller.ModelState.IsValid);
//            Assert.IsTrue(controller.ModelState.ContainsKey("DuplicateTitle"));
//        }
        #endregion

        #region Edit - Get
//        [TestMethod]
//        public void BlogController_EditGet_EmptySlug_RedirectsHome()
//        {
//            controller.Edit("sam", "")
//                .AssertActionRedirect()
//                .ToAction("Index")
//                .ToController("Home");
//        }

//        [TestMethod]
//        public void BlogController_EditGet_RendersEdit()
//        {
//            // Arrange
//            var blog = MockRepository.GenerateStub<IBlog>();
//            blog.AuthorID = "sam";
//            controller.BlogRepository
//               .Expect(b => b.LoadBySlug(Arg<string>.Is.Anything))
//               .Return(blog);

//            controller.Edit("sam", "blah")
//                .AssertViewRendered()
//                .ForView("Edit");
//        }

//        [TestMethod]
//        public void BlogController_EditGet_ViewData_IsBlogCreateModel()
//        {
//            // Arrange
//            var blog = MockRepository.GenerateStub<IBlog>();
//            blog.AuthorID = "sam";
//            controller.BlogRepository
//               .Expect(b => b.LoadBySlug(Arg<string>.Is.Anything))
//               .Return(blog);

//            // Act
//            controller.Edit("sam", "blah");

//            // Assert
//            Assert.IsInstanceOfType(controller.ViewData.Model, typeof(BlogCreateModel));
//        }

//        [TestMethod]
//        public void BlogController_EditGet_InvalidBlogEntry_RedirectsHome()
//        {
//            controller.Edit("sam", "blah")
//                .AssertActionRedirect()
//                .ToAction("Index")
//                .ToController("Home");
//        }

//        [TestMethod]
//        public void BlogController_EditGet_ValidBlog_SetsViewData()
//        {
//            // Arrange
//            var blog = MockRepository.GenerateStub<IBlog>();
//            blog.Entry = "entry";
//            blog.Title = "title";
//            blog.Tags = "tags";
//            blog.AuthorID = "sam";
//            controller.BlogRepository
//               .Expect(b => b.LoadBySlug(Arg<string>.Is.Anything))
//               .Return(blog);

//            // Act
//            controller.Edit("sam", "blog");

//            // Assert
//            Assert.AreEqual(blog.Entry, ((BlogCreateModel)controller.ViewData.Model).Entry);
//            Assert.AreEqual(blog.Title, ((BlogCreateModel)controller.ViewData.Model).Title);
//            Assert.AreEqual(blog.Tags, ((BlogCreateModel)controller.ViewData.Model).Tags);
//            Assert.AreEqual(blog.AuthorID, ((BlogCreateModel)controller.ViewData.Model).AuthorID);
//        }

//        [TestMethod]
//        public void BlogController_EditGet_ValidBlog_SetsIsEdit()
//        {
//            // Arrange
//            var blog = MockRepository.GenerateStub<IBlog>();
//            blog.Entry = "entry";
//            blog.Title = "title";
//            blog.Tags = "tags";
//            blog.AuthorID = "sam";
//            controller.BlogRepository
//               .Expect(b => b.LoadBySlug(Arg<string>.Is.Anything))
//               .Return(blog);

//            // Act
//            controller.Edit("sam", "blog");

//            // Assert
//            Assert.IsTrue(((BlogCreateModel)controller.ViewData.Model).IsEdit);
//        }

//        [TestMethod]
//        public void BlogController_EditGet_ValidBlog_SetsIsPublished()
//        {
//            // Arrange
//            var blog = MockRepository.GenerateStub<IBlog>();
//            blog.Entry = "entry";
//            blog.Title = "title";
//            blog.Tags = "tags";
//            blog.AuthorID = "sam";
//            blog.IsPublished = true;
//            controller.BlogRepository
//               .Expect(b => b.LoadBySlug(Arg<string>.Is.Anything))
//               .Return(blog);

//            // Act
//            controller.Edit("sam", "blog");

//            // Assert
//            Assert.IsTrue(((BlogCreateModel)controller.ViewData.Model).IsPublished);
//        }

//        //[TestMethod]
//        //public void BlogController_EditGet_DifferentUser_RedirectsHome()
//        //{
//        //    // Arrange
//        //    var blog = MockRepository.GenerateStub<IBlog>();
//        //    blog.Entry = "entry";
//        //    blog.Title = "title";
//        //    blog.Tags = "tags";
//        //    blog.AuthorID = "sam";
//        //    controller.BlogRepository
//        //       .Expect(b => b.LoadBySlug(Arg<string>.Is.Anything))
//        //       .Return(blog);

//        //    // Assert
//        //    controller.Edit("d", "blog")
//        //        .AssertActionRedirect()
//        //        .ToAction("Index")
//        //        .ToController("Home");
//        //}
        #endregion

        #region Edit - Post
//        [TestMethod]
//        public void BlogController_EditPost_RendersUserList()
//        {
//            // Arrange
//            controller.BlogRepository
//                .Expect(b => b.LoadBySlug(Arg<string>.Is.Anything))
//                .Return(MockRepository.GenerateStub<IBlog>());
//            var viewData = MockRepository.GenerateStub<BlogCreateModel>();
//            viewData.Entry = "entry";
//            viewData.Title = "title";
            
//            // Assert
//            controller.Edit(viewData)
//                .AssertActionRedirect()
//                .ToAction("UserList");
//        }

//        [TestMethod]
//        public void BlogController_EditPost_EmptyEntry_ReturnsWithError()
//        {
//            // Arrange
//            var model = MockRepository.GenerateStub<BlogCreateModel>();

//            // Assert
//            controller.Edit(model)
//                .AssertViewRendered()
//                .ForView("Edit");
//            Assert.IsFalse(controller.ModelState.IsValid);
//            Assert.IsTrue(controller.ModelState.ContainsKey("Entry"));
//        }

//        [TestMethod]
//        public void BlogController_EditPost_EmptyEntryWithFormValue_SetsEntry()
//        {
//            // Arrange
//            controller.Request.Form["textEntry"] = "value";
//            controller.BlogRepository
//               .Expect(b => b.LoadBySlug(Arg<string>.Is.Anything))
//               .Return(MockRepository.GenerateStub<IBlog>());
//            var viewData = MockRepository.GenerateStub<BlogCreateModel>();
//            viewData.Entry = "entry";
//            viewData.Title = "title";

//            // Act
//            var result = controller.Edit(viewData);

//            // Assert
//            result
//                .AssertActionRedirect()
//                .ToAction("UserList");
//        }

//        [TestMethod]
//        public void BlogController_EditPost_UpdatesAndSavesEntry()
//        {
//            // Arrange
//            var originalBlog = MockRepository.GenerateStub<IBlog>();
//            originalBlog.Entry = "entry";
//            originalBlog.Title = "the title";
//            controller.BlogRepository
//                .Expect(l => l.LoadBySlug("the-title"))
//                .Return(originalBlog);
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Entry = "new entry";
//            model.Title = "the title";

//            // Act
//            var result = controller.Edit(model);

//            // Assert
//            controller.BlogRepository
//                .AssertWasCalled(b => b.Save(Arg<IBlog>
//                    .Matches(a => a.Entry == model.Entry)));
//        }

//        [TestMethod]
//        public void BlogController_EditPost_UpdatesAndSavesIsPublishedIfNotAlreadyPublished()
//        {
//            // Arrange
//            var originalBlog = MockRepository.GenerateStub<IBlog>();
//            originalBlog.Entry = "entry";
//            originalBlog.Title = "the title";
//            originalBlog.IsPublished = false;
//            controller.BlogRepository
//                .Expect(l => l.LoadBySlug("the-title"))
//                .Return(originalBlog);
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Entry = "new entry";
//            model.Title = "the title";
//            model.IsPublished = true;

//            // Act
//            var result = controller.Edit(model);

//            // Assert
//            controller.BlogRepository
//                .AssertWasCalled(b => b.Save(Arg<IBlog>
//                    .Matches(a => a.IsPublished == true)));
//        }

//        [TestMethod]
//        public void BlogController_EditPost_UpdatesAndSavesIsPublished_WillNotUnPublish()
//        {
//            // Arrange
//            var originalBlog = MockRepository.GenerateStub<IBlog>();
//            originalBlog.Entry = "entry";
//            originalBlog.Title = "the title";
//            originalBlog.IsPublished = true;
//            controller.BlogRepository
//                .Expect(l => l.LoadBySlug("the-title"))
//                .Return(originalBlog);
//            var model = MockRepository.GenerateStub<BlogCreateModel>();
//            model.Entry = "new entry";
//            model.Title = "the title";
//            model.IsPublished = false;

//            // Act
//            var result = controller.Edit(model);

//            // Assert
//            controller.BlogRepository
//                .AssertWasCalled(b => b.Save(Arg<IBlog>
//                    .Matches(a => a.IsPublished == true)));
//        }
        #endregion

        private BlogController controller;
        private TestControllerBuilder builder;

        [TestInitialize]
        public void Setup()
        {
            controller = new BlogController();

            controller.BlogRepository = MockRepository.GenerateStub<IBlogRepository>();
            controller.BlogRepository.TitleCleaner = MockRepository.GenerateStub<ITitleCleaner>();
            controller.UserRepository = MockRepository.GenerateStub<IUserRepository>();
            controller.FamilyRepository = MockRepository.GenerateStub<IFamilyRepository>();
            controller.Cache = new InMemoryCache();

            builder = new TestControllerBuilder();
            builder.InitializeController(controller);

            controller.Url = new UrlHelper(new RequestContext(controller.HttpContext, new RouteData()));
        }
    }
}
