using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Models.Blog;
using Rhino.Mocks;
using WilliamsonFamily.Models.Web;
using System.Collections.Generic;
using WilliamsonFamily.Models.Data;
using WilliamsonFamily.Models;
using WilliamsonFamily.Models.Data.Tests.TestHelpers;
using WilliamsonFamily.Library.Web.Caching;

namespace WilliamsonFamily.Models.Data.Tests
{
	[TestClass]
	public class BlogRepositoryTests
	{
		#region LoadByID Tests
		[TestMethod]
		public void BlogRepository_LoadSingleByID_CanLoad()
		{
			int id = 3;
			repository.DataContext.Insert(new Blog { PkID = id });
			repository.DataContext.Insert(new Blog { PkID = id + 1 });

			var blog = repository.Load(id);

			Assert.AreEqual(id, blog.UniqueKey);
		}

		[TestMethod]
		public void BlogRepository_LoadSingleByID_InvalidIDReturnsNull()
		{
			int id = 3;
			repository.DataContext.Insert(new Blog { PkID = id });

			var blog = repository.Load(2);

			Assert.IsNull(blog);
		}
		#endregion

		#region LoadList Tests
		[TestMethod]
		public void BlogRepository_LoadListByAuthorID_CanLoad()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId });
			repository.DataContext.Insert(new Blog { AuthorID = authorId + "2" });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Number of Blogs");
			Assert.AreEqual(authorId, blogs.BlogEntries.FirstOrDefault().AuthorID, "Blog.AuthorID");
		}

		[TestMethod]
		public void BlogRepository_LoadListByIsPublished_CanLoad()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, IsPublished = true, DatePublished = DateTime.Now.AddDays(-1) });
			repository.DataContext.Insert(new Blog { AuthorID = authorId + "2", IsPublished = false, DatePublished = DateTime.Now.AddDays(-1) });

			var blogs = repository.LoadList(new BlogFilter { IsPublished = true });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Number of Blogs");
			Assert.AreEqual(authorId, blogs.BlogEntries.FirstOrDefault().AuthorID, "Blog.AuthorID");
		}

		[TestMethod]
		public void BlogRepository_LoadListByIsPublished_WillNotLoadFutureDates()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, IsPublished = true, DatePublished = DateTime.Now.AddDays(-1) });
			repository.DataContext.Insert(new Blog { AuthorID = authorId + "2", IsPublished = true, DatePublished = DateTime.Now.AddDays(1) });

			var blogs = repository.LoadList(new BlogFilter { IsPublished = true });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Number of Blogs");
			Assert.AreEqual(authorId, blogs.BlogEntries.FirstOrDefault().AuthorID, "Blog.AuthorID");
		}

		[TestMethod]
		public void BlogRepository_LoadListWithoutSpecifyingIsPublished_CanLoad()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, IsPublished = true });
			repository.DataContext.Insert(new Blog { AuthorID = authorId + "2", IsPublished = false });

			var blogs = repository.LoadList(new BlogFilter { });

			Assert.AreEqual(2, blogs.BlogEntries.Count(), "Number of Blogs");
		}

		[TestMethod]
		public void BlogRepository_LoadList_OrdersByDateDescending()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, DatePublished = DateTime.Now });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, DatePublished = DateTime.Now.AddDays(1) });

			var blogs = repository.LoadList(new BlogFilter { });

			Assert.AreEqual(DateTime.Now.AddDays(1).Date, blogs.BlogEntries.FirstOrDefault().DatePublished.Value.Date);
		}

		#endregion

		#region Factory Tests
		[TestMethod]
		public void BlogRepository_ModelFactory_CanCreateNew()
		{
			var blog = repository.New();

			Assert.IsInstanceOfType(blog, typeof(Blog));
		}
		#endregion

		#region Save Tests
		[TestMethod]
		public void BlogRepository_Save_CanSave()
		{
			string authorId = "1";
			repository.DataContext.Insert(new User { PkID = authorId, FirstName = "" });

			repository.Save(new Blog { AuthorID = authorId, Title = "" });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.IsNotNull(blog);
		}

		[TestMethod]
		public void BlogRepository_Save_UpdateNonExistingBlogThrowsException()
		{
			int pkId = 1;

			try
			{
				repository.Save(new OtherBlog { UniqueKey = pkId });
				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.IsInstanceOfType(ex, typeof(ArgumentException));
			}
		}

		[TestMethod]
		public void BlogRepository_Save_WillInsertNewBlog()
		{
			string authorId = "1";
			repository.DataContext.Insert(new User { PkID = authorId, FirstName = "" });
			int beforeCount = repository.DataContext.Repository<Blog>().Count();

			repository.Save(new Blog { AuthorID = authorId, Title = "" });

			int afterCount = repository.DataContext.Repository<Blog>().Count();
			Assert.AreEqual(1, afterCount - beforeCount);
		}

		[TestMethod]
		public void BlogRepository_Save_WillNotInsertExistingBlog()
		{
			int id = 1;
			repository.DataContext.Insert(new User { PkID = "sam", FirstName = "" });
			repository.DataContext.Insert(new Blog { PkID = id, Title = "", AuthorID = "sam" });
			var blog = repository.Load(id);

			int beforeCount = repository.DataContext.Repository<Blog>().Count();
			repository.Save(blog);

			int afterCount = repository.DataContext.Repository<Blog>().Count();
			Assert.AreEqual(0, afterCount - beforeCount);
		}

		[TestMethod]
		public void BlogRepository_Save_WillUpdateExistingBlog()
		{
			int id = 1;
			string beforeAuthorId = "1";
			string afterAuthorId = "2";
			repository.DataContext.Insert(new User { PkID = afterAuthorId, FirstName = "" });
			repository.DataContext.Insert(new Blog { PkID = id, AuthorID = beforeAuthorId, Title = "" });
			var blog = repository.Load(id);

			blog.AuthorID = afterAuthorId;
			repository.Save(blog);

			blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual(blog.AuthorID, afterAuthorId);
		}

		[TestMethod]
		public void BlogRepository_Save_WillInsertDatePublished()
		{
			string authorId = "1";
			repository.DataContext.Insert(new User { PkID = authorId, FirstName = "" });

			repository.Save(new Blog { AuthorID = authorId, Title = "" });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.IsNotNull(blog.DatePublished);
		}

		[TestMethod]
		public void BlogRepository_Save_WillLowercaseTags()
		{
			repository.DataContext.Insert(new User { PkID = "sam", FirstName = "" });

			repository.Save(new Blog { Tags = "Tag", Title = "", AuthorID = "sam" });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("tag", blog.Tags);
		}

		[TestMethod]
		public void BlogRepository_Save_WillAlphabatizeTags()
		{
			repository.DataContext.Insert(new User { PkID = "sam", FirstName = "" });

			repository.Save(new Blog { Tags = "tag apple", Title = "", AuthorID = "sam" });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("apple tag", blog.Tags);
		}

		[TestMethod]
		public void BlogRepository_Save_WillSaveSluggedTitle()
		{
			repository.DataContext.Insert(new User { PkID = "sam", FirstName = "" });
			repository.TitleCleaner
			   .Expect(s => s.CleanTitle("cooltitle"))
			   .Return("cooltitle");

			repository.Save(new Blog { Title = "cooltitle", AuthorID = "sam" });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("cooltitle", blog.Slug);
		}

		[TestMethod]
		public void BlogRepository_Save_Slug_WillReplaceSpacesWithDashes()
		{
			repository.DataContext.Insert(new User { PkID = "sam", FirstName = "" });
			repository.TitleCleaner
			   .Expect(s => s.CleanTitle("cool title"))
			   .Return("cool-title");

			repository.Save(new Blog { Title = "cool title", AuthorID = "sam" });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("cool-title", blog.Slug);
		}

		[TestMethod]
		public void BlogRepository_Save_Slug_WillLowerCase()
		{
			repository.DataContext.Insert(new User { PkID = "sam", FirstName = "" });
			repository.TitleCleaner
				.Expect(s => s.CleanTitle("Coolitle"))
				.Return("coolitle");

			repository.Save(new Blog { Title = "Coolitle", AuthorID = "sam" });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("coolitle", blog.Slug);
		}

		[TestMethod]
		public void BlogRepository_Save_SetsAuthorName()
		{
			string authorID = "author";
			string authorName = "name";

			repository.DataContext.Insert(new User { PkID = authorID, FirstName = authorName });

			repository.Save(new Blog { Title = "cooltitle", AuthorID = authorID });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual(authorName, blog.AuthorName);
		}

		[TestMethod]
		public void BlogRepository_Save_SetsPublished()
		{
			string authorID = "author";

			repository.DataContext.Insert(new User { PkID = authorID });

			repository.Save(new Blog { Title = "cooltitle", AuthorID = authorID, IsPublished = true });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.IsTrue(blog.IsPublished);
		}

		[TestMethod]
		public void BlogRepository_Save_ManualFuturePublishDateGetsSaved()
		{
			string authorID = "author";

			repository.DataContext.Insert(new User { PkID = authorID });
			DateTime published = DateTime.Now.AddDays(1);

			repository.Save(new Blog { Title = "cooltitle", AuthorID = authorID, IsPublished = true, DatePublished = published });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual(published, blog.DatePublished);
		}

		[TestMethod]
		public void BlogRepository_Save_OnEditAndPublish_SetDate()
		{
			int id = 1;
			string afterAuthorId = "2";

			repository.DataContext.Insert(new User { PkID = afterAuthorId, FirstName = "" });
			repository.DataContext.Insert(new Blog { PkID = id, AuthorID = afterAuthorId, Title = "", DatePublished = DateTime.Today.AddDays(-1), IsPublished = false });

			// NOTE: because it's an in memory repo, we're doing this madness.
			repository.Save(new Blog { PkID = id, AuthorID = afterAuthorId, Title = "", DatePublished = DateTime.Today.AddDays(-1), IsPublished = true });

			var blog = repository.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual(blog.DatePublished.Value.Day, DateTime.Today.Day);
		}
		#endregion

		#region LoadBySlug Tests
		[TestMethod]
		public void BlogRepository_LoadBySlug_LoadSingleBlogBySlug()
		{
			string title = "test title";
			string slug = "test-title";
			int id = 4;
			repository.DataContext.Insert(new Blog { PkID = id, Title = title, Slug = slug });

			var blog = repository.LoadBySlug(slug);

			Assert.IsNotNull(blog, "Blog is not null");
			Assert.AreEqual(id, blog.UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadBySlug_UnmatchedSlugReturnsNull()
		{
			var blog = repository.LoadBySlug("nah");

			Assert.IsNull(blog);
		}

		[TestMethod]
		public void BlogRepository_LoadBySlug_MultipleMatchesReturnsMostRecent()
		{
			int id1 = 1;
			int id2 = 2;
			string title = "the title";
			string slug = "the-title";
			repository.DataContext.Insert(new Blog { PkID = id1, DatePublished = DateTime.Now.AddDays(-1), Title = title, Slug = slug });
			repository.DataContext.Insert(new Blog { PkID = id2, DatePublished = DateTime.Now, Title = title, Slug = slug });

			var blog = repository.LoadBySlug(slug);

			Assert.AreEqual(id2, blog.UniqueKey);
		}

		#endregion

		#region LoadListByDate
		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYear()
		{
			repository.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			repository.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			repository.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/12") });

			var blogs = repository.LoadList(new BlogFilter { Date = "2009" });

			Assert.AreEqual(3, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYearMonth()
		{
			repository.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			repository.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			repository.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/12") });

			var blogs = repository.LoadList(new BlogFilter { Date = "2009/10" });

			Assert.AreEqual(2, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYearZeroMonth()
		{
			repository.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			repository.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			repository.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/12") });
			repository.DataContext.Insert(new Blog { PkID = 4, Title = "fourth", DatePublished = DateTime.Parse("2010/08/02") });
			repository.DataContext.Insert(new Blog { PkID = 5, Title = "fifth", DatePublished = DateTime.Parse("2010/08/03") });

			var blogs = repository.LoadList(new BlogFilter { Date = "2010/08" });

			Assert.AreEqual(2, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYearMonthDay()
		{
			repository.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			repository.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			repository.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/12") });
			repository.DataContext.Insert(new Blog { PkID = 4, Title = "fourth", DatePublished = DateTime.Parse("2010/08/02") });
			repository.DataContext.Insert(new Blog { PkID = 5, Title = "fifth", DatePublished = DateTime.Parse("2010/08/03") });

			var blogs = repository.LoadList(new BlogFilter { Date = "2009/10/12" });

			Assert.AreEqual(1, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYearMonthZeroDay()
		{
			repository.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			repository.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			repository.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/02") });
			repository.DataContext.Insert(new Blog { PkID = 4, Title = "fourth", DatePublished = DateTime.Parse("2010/08/02") });
			repository.DataContext.Insert(new Blog { PkID = 5, Title = "fifth", DatePublished = DateTime.Parse("2010/08/03") });

			var blogs = repository.LoadList(new BlogFilter { Date = "2010/08/02" });

			Assert.AreEqual(1, blogs.BlogEntries.Count());
		}
		#endregion

		#region LoadListByTags Tests
		[TestMethod]
		public void BlogRepository_LoadListByTags_LoadListOfBlogsWithSingleTagFromSingleTag()
		{
			string tag = "first";
			repository.DataContext.Insert(new Blog { PkID = 1, Tags = tag });
			repository.DataContext.Insert(new Blog { PkID = 2, Tags = "second", });

			var blogs = repository.LoadList(new BlogFilter { Tags = tag });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Count");
			Assert.AreEqual(1, blogs.BlogEntries.FirstOrDefault().UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadListByTags_LoadListOfBlogsWithMultipleTagFromSingleTag()
		{
			string tag = "first";
			repository.DataContext.Insert(new Blog { PkID = 1, Tags = tag + " second" });
			repository.DataContext.Insert(new Blog { PkID = 2, Tags = "second", });

			var blogs = repository.LoadList(new BlogFilter { Tags = tag });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Count");
			Assert.AreEqual(1, blogs.BlogEntries.FirstOrDefault().UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadListByTags_CaseDoesNotMatter()
		{
			repository.DataContext.Insert(new Blog { PkID = 1, Tags = "first" });
			repository.DataContext.Insert(new Blog { PkID = 2, Tags = "second", });

			var blogs = repository.LoadList(new BlogFilter { Tags = "FIRST" });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Count");
			Assert.AreEqual(1, blogs.BlogEntries.FirstOrDefault().UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadListByTags_LoadListOfBlogsWithMultipleTagFromMultipsTag()
		{
			repository.DataContext.Insert(new Blog { PkID = 1, Tags = "first second" });
			repository.DataContext.Insert(new Blog { PkID = 2, Tags = "second third" });
			repository.DataContext.Insert(new Blog { PkID = 3, Tags = "first third" });

			var blogs = repository.LoadList(new BlogFilter { Tags = "second first" });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Count");
			Assert.AreEqual(1, blogs.BlogEntries.FirstOrDefault().UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadListByTags_OrderOfTagsDoesNotMatter()
		{
			repository.DataContext.Insert(new Blog { PkID = 1, Tags = "first second" });
			repository.DataContext.Insert(new Blog { PkID = 2, Tags = "second third", });
			repository.DataContext.Insert(new Blog { PkID = 3, Tags = "second first", });

			var blogs = repository.LoadList(new BlogFilter { Tags = "second first" });

			Assert.AreEqual(2, blogs.BlogEntries.Count(), "Count");
		}
		#endregion

		#region LoadListByFamilyName Tests
		[TestMethod]
		public void BlogRepository_LoadListByFamilyName_CanLoadEntriesFromFamily()
		{
			int familiyID = 1;
			string familyName = "family";
			string firstPerson = "2";
			string secondPerson = "3";
			repository.DataContext.Insert(new Family { PkID = familiyID, FamilyName = familyName });
			repository.DataContext.Insert(new User { PkID = firstPerson });
			repository.DataContext.Insert(new User { PkID = secondPerson });
			repository.DataContext.Insert(new UserFamily { FamilyID = familiyID, UserID = firstPerson });
			repository.DataContext.Insert(new UserFamily { FamilyID = familiyID, UserID = secondPerson });
			repository.DataContext.Insert(new Blog { AuthorID = "none", PkID = 3 });
			repository.DataContext.Insert(new Blog { AuthorID = firstPerson, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = secondPerson, PkID = 2 });

			var blogs = repository.LoadList(new BlogFilter { LoadBlogBy = LoadBlogBy.Family, AuthorName = familyName });

			Assert.AreEqual(2, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByFamilyName_IgnoreFamilyNameCase()
		{
			int familiyID = 1;
			string familyName = "family";
			string firstPerson = "2";
			string secondPerson = "3";
			repository.DataContext.Insert(new Family { PkID = familiyID, FamilyName = familyName });
			repository.DataContext.Insert(new User { PkID = firstPerson });
			repository.DataContext.Insert(new User { PkID = secondPerson });
			repository.DataContext.Insert(new UserFamily { FamilyID = familiyID, UserID = firstPerson });
			repository.DataContext.Insert(new UserFamily { FamilyID = familiyID, UserID = secondPerson });
			repository.DataContext.Insert(new Blog { AuthorID = "none", PkID = 3 });
			repository.DataContext.Insert(new Blog { AuthorID = firstPerson, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = secondPerson, PkID = 2 });

			var blogs = repository.LoadList(new BlogFilter { LoadBlogBy = LoadBlogBy.Family, AuthorName = familyName.ToUpper() });

			Assert.AreEqual(2, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByFamilyName_ReturnsEmptyListIfFamilyDoesNotExist()
		{
			string familyName = "family";
			string firstPerson = "2";
			string secondPerson = "3";
			repository.DataContext.Insert(new Blog { AuthorID = firstPerson, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = secondPerson, PkID = 2 });

			var blogs = repository.LoadList(new BlogFilter { LoadBlogBy = LoadBlogBy.Family, AuthorName = familyName });

			Assert.AreEqual(0, blogs.BlogEntries.Count());
		}
		#endregion

		#region LoadList - PagedList Tests
		// If pagesize null, ignore
		[TestMethod]
		public void BlogRespository_LoadPagedList_NullPageSize_ReturnsFullList()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User });

			Assert.AreEqual(3, blogs.BlogEntries.Count(), "Number of Blogs");
		}

		// If pagesize > total, return less
		[TestMethod]
		public void BlogRespository_LoadPagedList_ExplicitPageCount_ReturnsPagedList()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2 });

			Assert.AreEqual(2, blogs.BlogEntries.Count(), "Number of Blogs");
		}

		// if pageindex is set, return page
		[TestMethod]
		public void BlogRespository_LoadPagedList_ExplicitPageCountWithIndex_ReturnsPagedList()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2, PageIndex = 2 });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Number of Blogs");
			Assert.AreEqual(3, blogs.BlogEntries.FirstOrDefault().UniqueKey, "Key");
		}

		// if paging, set pagecount
		[TestMethod]
		public void BlogRespository_LoadPagedList_ExplicitPageCountWithIndex_ReturnsPageCount()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2, PageIndex = 2 });

			Assert.AreEqual(2, blogs.PageCount);
		}

		// set totalcount
		[TestMethod]
		public void BlogRespository_LoadPagedList_ExplicitPageCountWithIndex_ReturnsPageIndex()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2, PageIndex = 2 });

			Assert.AreEqual(2, blogs.PageIndex);
		}

		// Total Count from cache 
		[TestMethod]
		public void BlogRespository_LoadPagedList_PreviousCacheBlogCount_ReturnsPageCount()
		{
			string authorId = "1";
			repository.Cache.Insert(new BlogListCountCacheKey().GenerateKey(authorId), 2);
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2, PageIndex = 1 });

			Assert.AreEqual(1, blogs.PageCount);
		}

		// Default to page index 1
		[TestMethod]
		public void BlogRespository_LoadPagedList_NoPageIndexDefined_DefaultsTo1()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2 });

			Assert.AreEqual(1, blogs.PageIndex);
		}

		// Large List page count
		[TestMethod]
		public void BlogRespository_LoadPagedList_LargeList_CorrectlyDefinesPageCount()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 4 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 5 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 6 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 7 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 9 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 9 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 10 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 11 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 12 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 13 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 14 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 15 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 16 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 3 });

			Assert.AreEqual(6, blogs.PageCount);
		}

		// Large List page count again
		[TestMethod]
		public void BlogRespository_LoadPagedList_AnotherLargeList_CorrectlyDefinesPageCount()
		{
			string authorId = "1";
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 4 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 5 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 6 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 7 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 9 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 9 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 10 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 11 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 12 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 13 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 14 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 15 });
			repository.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 16 });

			var blogs = repository.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 6 });

			Assert.AreEqual(3, blogs.PageCount);
		}
		#endregion

		#region DeleteUnpublished
		[TestMethod]
		public void DeleteUnpublished_Unpublished_DeletesEntry()
		{
			repository.DataContext.Insert(new Blog { AuthorID = "none", PkID = 3, Entry = "entry", IsPublished = false });

			repository.DeleteUnpublished(3);

			Assert.AreEqual(0, repository.DataContext.Repository<Blog>().Count());
		}

		[TestMethod]
		public void DeleteUnpublished_Published_DoesNotDeleteEntry()
		{
			repository.DataContext.Insert(new Blog { AuthorID = "none", PkID = 3, Entry = "entry", IsPublished = true });

			repository.DeleteUnpublished(3);

			Assert.AreEqual(1, repository.DataContext.Repository<Blog>().Count());
		}

		#endregion

		BlogRepository repository;
		[TestInitialize]
		public void Init()
		{
			repository = GetPersister();
		}

		private BlogRepository GetPersister()
		{
			var dc = new InMemoryDataContext();
			var dcf = new InMemoryDataContextFactory { DataContext = dc };
			var titleCleaner = MockRepository.GenerateMock<ITitleCleaner>();
			return new BlogRepository { DataContext = dc, DataContextFactory = dcf, TitleCleaner = titleCleaner, Cache = new InMemoryCache() };
		}

		public class OtherBlog : IBlog
		{
			public string AuthorID { get; set; }
			public string Title { get; set; }
			public string Slug { get; set; }
			public string Entry { get; set; }
			public string Tags { get; set; }
			public DateTime? DatePublished { get; set; }
			public int UniqueKey { get; set; }
			public string AuthorName { get; set; }
			public bool IsPublished { get; set; }
		}
	}
}