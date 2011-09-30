using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Models.Blog;
using Rhino.Mocks;
using WilliamsonFamily.Models.Web;
using System.Collections.Generic;
using WilliamsonFamily.Models.Data;
using WilliamsonFamily.Models;

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
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { PkID = id });
			persister.DataContext.Insert(new Blog { PkID = id + 1 });

			var blog = persister.Load(id);

			Assert.AreEqual(id, blog.UniqueKey);
		}

		[TestMethod]
		public void BlogRepository_LoadSingleByID_InvalidIDReturnsNull()
		{
			int id = 3;
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { PkID = id });

			var blog = persister.Load(2);

			Assert.IsNull(blog);
		}
		#endregion

		#region LoadList Tests
		[TestMethod]
		public void BlogRepository_LoadListByAuthorID_CanLoad()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId });
			persister.DataContext.Insert(new Blog { AuthorID = authorId + "2"});

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Number of Blogs");
			Assert.AreEqual(authorId, blogs.BlogEntries.FirstOrDefault().AuthorID, "Blog.AuthorID");
		}

		[TestMethod]
		public void BlogRepository_LoadListBySpecifiedIsPublished_CanLoad()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, IsPublished = true });
			persister.DataContext.Insert(new Blog { AuthorID = authorId + "2", IsPublished = false });

			var blogs = persister.LoadList(new BlogFilter { IsPublished = true });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Number of Blogs");
			Assert.AreEqual(authorId, blogs.BlogEntries.FirstOrDefault().AuthorID, "Blog.AuthorID");
		}

		[TestMethod]
		public void BlogRepository_LoadListWithoutSpecifyingIsPublished_CanLoad()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, IsPublished = true });
			persister.DataContext.Insert(new Blog { AuthorID = authorId + "2", IsPublished = false });

			var blogs = persister.LoadList(new BlogFilter { });

			Assert.AreEqual(2, blogs.BlogEntries.Count(), "Number of Blogs");
		}

	    [TestMethod]
		public void BlogRepository_LoadList_OrdersByDateDescending()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, DatePublished = DateTime.Now });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, DatePublished = DateTime.Now.AddDays(1) });

			var blogs = persister.LoadList(new BlogFilter { });

			Assert.AreEqual(DateTime.Now.AddDays(1).Date, blogs.BlogEntries.FirstOrDefault().DatePublished.Value.Date);
		}
	
		#endregion

		#region Factory Tests
		[TestMethod]
		public void BlogRepository_ModelFactory_CanCreateNew()
		{
			var persister = GetPersister();

			var blog = persister.New();

			Assert.IsInstanceOfType(blog, typeof(Blog));
		}
		#endregion

		#region Save Tests
		[TestMethod]
		public void BlogRepository_Save_CanSave()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = authorId, FirstName = "" });

			persister.Save(new Blog { AuthorID = authorId, Title = "" });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.IsNotNull(blog);
		}

		[TestMethod]
		public void BlogRepository_Save_UpdateNonExistingBlogThrowsException()
		{
			int pkId = 1;
			var persister = GetPersister();

			try
			{
				persister.Save(new OtherBlog { UniqueKey = pkId });
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
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = authorId, FirstName = "" });

			int beforeCount = persister.DataContext.Repository<Blog>().Count();
			persister.Save(new Blog { AuthorID = authorId, Title = "" });

			int afterCount = persister.DataContext.Repository<Blog>().Count();
			Assert.AreEqual(1, afterCount - beforeCount);
		}

		[TestMethod]
		public void BlogRepository_Save_WillNotInsertExistingBlog()
		{
			int id = 1;
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = "sam", FirstName = "" });
			persister.DataContext.Insert(new Blog { PkID = id, Title = "", AuthorID = "sam" });
			var blog = persister.Load(id);

			int beforeCount = persister.DataContext.Repository<Blog>().Count();
			persister.Save(blog);

			int afterCount = persister.DataContext.Repository<Blog>().Count();
			Assert.AreEqual(0, afterCount - beforeCount);
		}

		[TestMethod]
		public void BlogRepository_Save_WillUpdateExistingBlog()
		{
			int id = 1;
			string beforeAuthorId = "1";
			string afterAuthorId = "2";
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = afterAuthorId, FirstName = "" });
			persister.DataContext.Insert(new Blog { PkID = id, AuthorID = beforeAuthorId, Title = "" });
			var blog = persister.Load(id);

			blog.AuthorID = afterAuthorId;
			persister.Save(blog);

			blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual(blog.AuthorID, afterAuthorId);
		}

		[TestMethod]
		public void BlogRepository_Save_WillInsertDatePublished()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = authorId, FirstName = "" });

			persister.Save(new Blog { AuthorID = authorId, Title = "" });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.IsNotNull(blog.DatePublished);
		}

		[TestMethod] 
		public void BlogRepository_Save_WillLowercaseTags()
		{
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = "sam", FirstName = "" });

			persister.Save(new Blog { Tags = "Tag", Title = "", AuthorID = "sam" });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("tag", blog.Tags);
		}

		[TestMethod]
		public void BlogRepository_Save_WillAlphabatizeTags()
		{
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = "sam", FirstName = "" });

			persister.Save(new Blog { Tags = "tag apple", Title = "", AuthorID = "sam" });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("apple tag", blog.Tags);
		}

		[TestMethod]
		public void BlogRepository_Save_WillSaveSluggedTitle()
		{
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = "sam", FirstName = "" });
			persister.TitleCleaner
			   .Expect(s => s.CleanTitle("cooltitle"))
			   .Return("cooltitle");

			persister.Save(new Blog { Title = "cooltitle", AuthorID = "sam" });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("cooltitle", blog.Slug);
		}

		[TestMethod]
		public void BlogRepository_Save_Slug_WillReplaceSpacesWithDashes()
		{
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = "sam", FirstName = "" });
			persister.TitleCleaner
			   .Expect(s => s.CleanTitle("cool title"))
			   .Return("cool-title");

			persister.Save(new Blog { Title = "cool title", AuthorID = "sam" });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("cool-title", blog.Slug);
		}

		[TestMethod]
		public void BlogRepository_Save_Slug_WillLowerCase()
		{
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = "sam", FirstName = "" });
			persister.TitleCleaner
				.Expect(s => s.CleanTitle("Coolitle"))
				.Return("coolitle");

			persister.Save(new Blog { Title = "Coolitle", AuthorID = "sam" });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual("coolitle", blog.Slug);
		}

		[TestMethod]
		public void BlogRepository_Save_SetsAuthorName()
		{
			// Arrange
			string authorID = "author";
			string authorName = "name";
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = authorID, FirstName = authorName });

			persister.Save(new Blog { Title = "cooltitle", AuthorID = authorID });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.AreEqual(authorName, blog.AuthorName);
		}

		[TestMethod]
		public void BlogRepository_Save_SetsPublished()
		{
			// Arrange
			string authorID = "author";
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = authorID });

			persister.Save(new Blog { Title = "cooltitle", AuthorID = authorID, IsPublished = true });

			var blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			Assert.IsTrue(blog.IsPublished);
		}

		[TestMethod]
		public void BlogRepository_Save_OnEditAndPublish_SetDate()
		{
			int id = 1;
			string afterAuthorId = "2";
			var persister = GetPersister();
			persister.DataContext.Insert(new User { PkID = afterAuthorId, FirstName = "" });
			persister.DataContext.Insert(new Blog { PkID = id, AuthorID = afterAuthorId, Title = "", DatePublished = DateTime.Today.AddDays(-1), IsPublished = false });
			var blog = persister.Load(id);

			blog.IsPublished = true;
			persister.Save(blog);

			blog = persister.DataContext.Repository<Blog>().FirstOrDefault();
			//Assert.AreEqual(blog.DatePublished.Value.Day, DateTime.Today.Day);
			// ARG. InMemory has a bug; this doesn't get hit
		}
		#endregion

		#region LoadBySlug Tests
		[TestMethod]
		public void BlogRepository_LoadBySlug_LoadSingleBlogBySlug()
		{
			string title = "test title";
			string slug = "test-title";
			int id = 4;
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = id, Title = title, Slug = slug });

			// Act
			var blog = persister.LoadBySlug(slug);

			// Assert
			Assert.IsNotNull(blog, "Blog is not null");
			Assert.AreEqual(id, blog.UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadBySlug_UnmatchedSlugReturnsNull()
		{
			var persister = GetPersister();

			// Act
			var blog = persister.LoadBySlug("nah");

			//Assert
			Assert.IsNull(blog);
		}

		[TestMethod]
		public void BlogRepository_LoadBySlug_MultipleMatchesReturnsMostRecent()
		{
			int id1 = 1;
			int id2 = 2;
			string title = "the title";
			string slug = "the-title";
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = id1, DatePublished = DateTime.Now.AddDays(-1), Title = title, Slug = slug });
			persister.DataContext.Insert(new Blog { PkID = id2, DatePublished = DateTime.Now, Title = title, Slug = slug });

			// Act
			var blog = persister.LoadBySlug(slug);

			// Assert
			Assert.AreEqual(id2, blog.UniqueKey);
		}

		#endregion

		#region LoadListByDate
		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYear()
		{
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			persister.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			persister.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/12") });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Date = "2009" });

			// Assert
			Assert.AreEqual(3, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYearMonth()
		{
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			persister.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			persister.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/12") });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Date = "2009/10" });

			// Assert
			Assert.AreEqual(2, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYearZeroMonth()
		{
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			persister.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			persister.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/12") });
			persister.DataContext.Insert(new Blog { PkID = 4, Title = "fourth", DatePublished = DateTime.Parse("2010/08/02") });
			persister.DataContext.Insert(new Blog { PkID = 5, Title = "fifth", DatePublished = DateTime.Parse("2010/08/03") });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Date = "2010/08" });

			// Assert
			Assert.AreEqual(2, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYearMonthDay()
		{
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			persister.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			persister.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/12") });
			persister.DataContext.Insert(new Blog { PkID = 4, Title = "fourth", DatePublished = DateTime.Parse("2010/08/02") });
			persister.DataContext.Insert(new Blog { PkID = 5, Title = "fifth", DatePublished = DateTime.Parse("2010/08/03") });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Date = "2009/10/12" });

			// Assert
			Assert.AreEqual(1, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByDate_LoadByYearMonthZeroDay()
		{
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Title = "first", DatePublished = DateTime.Parse("2009/10/12") });
			persister.DataContext.Insert(new Blog { PkID = 2, Title = "second", DatePublished = DateTime.Parse("2009/10/13") });
			persister.DataContext.Insert(new Blog { PkID = 3, Title = "third", DatePublished = DateTime.Parse("2009/11/02") });
			persister.DataContext.Insert(new Blog { PkID = 4, Title = "fourth", DatePublished = DateTime.Parse("2010/08/02") });
			persister.DataContext.Insert(new Blog { PkID = 5, Title = "fifth", DatePublished = DateTime.Parse("2010/08/03") });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Date = "2010/08/02" });

			// Assert
			Assert.AreEqual(1, blogs.BlogEntries.Count());
		}
		#endregion

		#region LoadListByTags Tests
		[TestMethod]
		public void BlogRepository_LoadListByTags_LoadListOfBlogsWithSingleTagFromSingleTag()
		{
			string tag = "first";
			 var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Tags = tag });
			persister.DataContext.Insert(new Blog { PkID = 2, Tags = "second", });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Tags = tag });

			// Assert
			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Count");
			Assert.AreEqual(1, blogs.BlogEntries.FirstOrDefault().UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadListByTags_LoadListOfBlogsWithMultipleTagFromSingleTag()
		{
			string tag = "first";
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Tags = tag + " second" });
			persister.DataContext.Insert(new Blog { PkID = 2, Tags = "second", });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Tags = tag });

			// Assert
			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Count");
			Assert.AreEqual(1, blogs.BlogEntries.FirstOrDefault().UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadListByTags_CaseDoesNotMatter()
		{
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Tags = "first" });
			persister.DataContext.Insert(new Blog { PkID = 2, Tags = "second", });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Tags = "FIRST" });

			// Assert
			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Count");
			Assert.AreEqual(1, blogs.BlogEntries.FirstOrDefault().UniqueKey, "UniqueKey");
		}

		[TestMethod]
		public void BlogRepository_LoadListByTags_LoadListOfBlogsWithMultipleTagFromMultipsTag()
		{
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Tags = "first second" });
			persister.DataContext.Insert(new Blog { PkID = 2, Tags = "second third" });
			persister.DataContext.Insert(new Blog { PkID = 3, Tags = "first third" });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Tags = "second first" });

			// Assert
			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Count");
			Assert.AreEqual(1, blogs.BlogEntries.FirstOrDefault().UniqueKey, "UniqueKey");
		}
		
		[TestMethod]
		public void BlogRepository_LoadListByTags_OrderOfTagsDoesNotMatter()
		{
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { PkID = 1, Tags = "first second" });
			persister.DataContext.Insert(new Blog { PkID = 2, Tags = "second third", });
			persister.DataContext.Insert(new Blog { PkID = 3, Tags = "second first", });

			// Act
			var blogs = persister.LoadList(new BlogFilter { Tags = "second first" });

			// Assert
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
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Family { PkID = familiyID, FamilyName = familyName });
			persister.DataContext.Insert(new User { PkID = firstPerson });
			persister.DataContext.Insert(new User { PkID = secondPerson });
			persister.DataContext.Insert(new UserFamily { FamilyID = familiyID, UserID = firstPerson });
			persister.DataContext.Insert(new UserFamily { FamilyID = familiyID, UserID = secondPerson });
			persister.DataContext.Insert(new Blog { AuthorID = "none", PkID = 3 });
			persister.DataContext.Insert(new Blog { AuthorID = firstPerson, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = secondPerson, PkID = 2 });

			// Act
			var blogs = persister.LoadList(new BlogFilter { LoadBlogBy = LoadBlogBy.Family, AuthorName = familyName });

			// Assert
			Assert.AreEqual(2, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByFamilyName_IgnoreFamilyNameCase()
		{
			int familiyID = 1;
			string familyName = "family";
			string firstPerson = "2";
			string secondPerson = "3";
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Family { PkID = familiyID, FamilyName = familyName });
			persister.DataContext.Insert(new User { PkID = firstPerson });
			persister.DataContext.Insert(new User { PkID = secondPerson });
			persister.DataContext.Insert(new UserFamily { FamilyID = familiyID, UserID = firstPerson });
			persister.DataContext.Insert(new UserFamily { FamilyID = familiyID, UserID = secondPerson });
			persister.DataContext.Insert(new Blog { AuthorID = "none", PkID = 3 });
			persister.DataContext.Insert(new Blog { AuthorID = firstPerson, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = secondPerson, PkID = 2 });

			// Act
			var blogs = persister.LoadList(new BlogFilter { LoadBlogBy = LoadBlogBy.Family, AuthorName = familyName.ToUpper() });

			// Assert
			Assert.AreEqual(2, blogs.BlogEntries.Count());
		}

		[TestMethod]
		public void BlogRepository_LoadListByFamilyName_ReturnsEmptyListIfFamilyDoesNotExist()
		{
			string familyName = "family";
			string firstPerson = "2";
			string secondPerson = "3";
			var persister = GetPersister();

			// Arrange
			persister.DataContext.Insert(new Blog { AuthorID = firstPerson, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = secondPerson, PkID = 2 });

			// Act
			var blogs = persister.LoadList(new BlogFilter { LoadBlogBy = LoadBlogBy.Family, AuthorName = familyName });

			// Assert
			Assert.AreEqual(0, blogs.BlogEntries.Count());
		}
		#endregion

		#region LoadList - PagedList Tests
		// If pagesize null, ignore
		[TestMethod]
		public void BlogRespository_LoadPagedList_NullPageSize_ReturnsFullList()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User });

			Assert.AreEqual(3, blogs.BlogEntries.Count(), "Number of Blogs");
		}

		// If pagesize > total, return less
		[TestMethod]
		public void BlogRespository_LoadPagedList_ExplicitPageCount_ReturnsPagedList()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2 });

			Assert.AreEqual(2, blogs.BlogEntries.Count(), "Number of Blogs");
		}

		// if pageindex is set, return page
		[TestMethod]
		public void BlogRespository_LoadPagedList_ExplicitPageCountWithIndex_ReturnsPagedList()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2, PageIndex = 2 });

			Assert.AreEqual(1, blogs.BlogEntries.Count(), "Number of Blogs");
			Assert.AreEqual(3, blogs.BlogEntries.FirstOrDefault().UniqueKey, "Key");
		}

		// if paging, set pagecount
		[TestMethod]
		public void BlogRespository_LoadPagedList_ExplicitPageCountWithIndex_ReturnsPageCount()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2, PageIndex = 2 });

			Assert.AreEqual(2, blogs.PageCount);
		}

		// set totalcount
		[TestMethod]
		public void BlogRespository_LoadPagedList_ExplicitPageCountWithIndex_ReturnsPageIndex()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2, PageIndex = 2 });

			Assert.AreEqual(2, blogs.PageIndex);
		}
		
	// Default to page index 1
		[TestMethod]
		public void BlogRespository_LoadPagedList_NoPageIndexDefined_DefaultsTo1()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 2 });

			Assert.AreEqual(1, blogs.PageIndex);
		}
		
		// Large List page count
		[TestMethod]
		public void BlogRespository_LoadPagedList_LargeList_CorrectlyDefinesPageCount()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 4 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 5 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 6 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 7 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 9 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 9 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 10 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 11 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 12 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 13 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 14 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 15 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 16 });

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 3 });

			Assert.AreEqual(6, blogs.PageCount);
		}
		
		// Large List page count again
		[TestMethod]
		public void BlogRespository_LoadPagedList_AnotherLargeList_CorrectlyDefinesPageCount()
		{
			string authorId = "1";
			var persister = GetPersister();
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 1 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 2 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 3 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 4 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 5 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 6 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 7 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 9 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 9 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 10 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 11 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 12 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 13 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 14 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 15 });
			persister.DataContext.Insert(new Blog { AuthorID = authorId, PkID = 16 });

			var blogs = persister.LoadList(new BlogFilter { AuthorName = authorId, LoadBlogBy = LoadBlogBy.User, PageSize = 6 });

			Assert.AreEqual(3, blogs.PageCount);
		}
		#endregion

		#region Helpers
		#region Provider
		private BlogRepository GetPersister()
		{
			var dc = new InMemoryDataContext();
			var dcf = new InMemoryDataContextFactory { DataContext = dc };
			var titleCleaner = MockRepository.GenerateMock<ITitleCleaner>();
			return new BlogRepository { DataContext = dc, DataContextFactory = dcf, TitleCleaner = titleCleaner };
		}
		#endregion

		#region OtherBlog
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

		#endregion
		#endregion
	}
}