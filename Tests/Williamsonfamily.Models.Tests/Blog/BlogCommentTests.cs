using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Models.Blog;

namespace WilliamsonFamily.Models.Data.Tests
{
    [TestClass]
    public class BlogCommentTests
    {
        #region Load Tests
        [TestMethod]
        public void BlogComment_LoadListByAuthorID_CanLoad()
        {
            int blogId = 1;
            var persister = GetPersister();
            persister.DataContext.Insert(new BlogComment { BlogID = blogId });
            persister.DataContext.Insert(new BlogComment { BlogID = blogId + 1 });

            var blogComments = persister.LoadList(blogId);

            Assert.AreEqual(1, blogComments.Count(), "Number of BlogComments");
            Assert.AreEqual(blogId, blogComments.FirstOrDefault().BlogID, "BlogComment.BlogID");
        }

        [TestMethod]
        public void BlogComment_LoadSingleByID_InvalidIDReturnsEmptyList()
        {
            int blogId = 3;
            var persister = GetPersister();
            persister.DataContext.Insert(new BlogComment { BlogID = blogId });

            var blogComment = persister.LoadList(blogId + 1);

            Assert.IsNotNull(blogComment, "List is not null");
            Assert.AreEqual(0, blogComment.Count(), "Number of BlogComments");
        }
        #endregion

        #region Factory Tests
        [TestMethod]
        public void BlogComment_ModelFactory_CanCreateNew()
        {
            var persister = GetPersister();

            var blogComment = persister.New();

            Assert.IsInstanceOfType(blogComment, typeof(BlogComment));
        }
        #endregion

        #region Save Tests
        [TestMethod]
        public void BlogComment_Save_CanSave()
        {
            string authorId = "1";
            var persister = GetPersister();

            persister.Save(new BlogComment { AuthorID = authorId });

            var blogComment = persister.DataContext.Repository<BlogComment>().FirstOrDefault();
            Assert.IsNotNull(blogComment);
        }

        [TestMethod]
        public void BlogComment_Save_UpdateInvalidBlogCommentThrowsException()
        {
            int pkID = 1;
            var persister = GetPersister();

            try
            {
                persister.Save(new OtherBlogComment { UniqueKey = pkID });
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            }
        }

        [TestMethod]
        public void BlogComment_Save_WillInsertNewBlog()
        {
            string authorId = "1";
            var persister = GetPersister();

            int beforeCount = persister.DataContext.Repository<BlogComment>().Count();
            persister.Save(new BlogComment { AuthorID = authorId });

            int afterCount = persister.DataContext.Repository<BlogComment>().Count();
            Assert.AreEqual(1, afterCount - beforeCount);
        }

        [TestMethod]
        public void BlogComment_Save_WillNotInsertExistingBlog()
        {
            int id = 1;
            var persister = GetPersister();
            persister.DataContext.Insert(new BlogComment { PkID = 1, BlogID = id });
            var blogComment = persister.LoadList(id).FirstOrDefault();

            int beforeCount = persister.DataContext.Repository<BlogComment>().Count();
            persister.Save(blogComment);

            int afterCount = persister.DataContext.Repository<BlogComment>().Count();
            Assert.AreEqual(0, afterCount - beforeCount);
        }

        [TestMethod]
        public void BlogComment_Save_WillUpdateExistingBlog()
        {
            int id = 1;
            string beforeAuthorId = "1";
            string afterAuthorId = "2";
            var persister = GetPersister();
            persister.DataContext.Insert(new BlogComment { BlogID = id, AuthorID = beforeAuthorId });
            var blogComment = persister.LoadList(id).FirstOrDefault();

            blogComment.AuthorID = afterAuthorId;
            persister.Save(blogComment);

            blogComment = persister.DataContext.Repository<BlogComment>().FirstOrDefault();
            Assert.AreEqual(blogComment.AuthorID, afterAuthorId);
        }

        [TestMethod]
        public void BlogComment_Save_WillInsertDatePublished()
        {
            string authorId = "1";
            var persister = GetPersister();

            persister.Save(new BlogComment { AuthorID = authorId });

            var blogComment = persister.DataContext.Repository<BlogComment>().FirstOrDefault();
            Assert.IsNotNull(blogComment.DatePublished);
        }
        #endregion

        #region Provider
        private BlogCommentRepository GetPersister()
        {
            var dc = new InMemoryDataContext();
            var dcf = new InMemoryDataContextFactory { DataContext = dc };
            return new BlogCommentRepository { DataContext = dc, DataContextFactory = dcf };
        }
        #endregion

        #region OtherBlog
        public class OtherBlogComment : IBlogComment
        {
            public int BlogID { get; set; }
            public string AuthorID { get; set; }
            public string Comment { get; set; }
            public DateTime? DatePublished { get; set; }
            public int UniqueKey { get; set; }
        }

        #endregion
    }
}
