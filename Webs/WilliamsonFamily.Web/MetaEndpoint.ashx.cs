using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CookComputing.XmlRpc;
using WilliamsonFamily.Models.Web.MetaWeblog;
using WilliamsonFamily.Models.User;
using WilliamsonFamily.Models.Blog;
using WilliamsonFamily.Library.Exceptions;
using WilliamsonFamily.DependencyInjection.StructureMap;
using System.Web.Mvc;
using StructureMap;
using WilliamsonFamily.Models.Web;

namespace WilliamsonFamily.Web
{
	/// <summary>
	/// Summary description for MetaEndpoint
	/// </summary>
	public class MetaEndpoint : XmlRpcService, IMetaWeblog
    {
		public IBlogRepository BlogRepository { get; set; }
		public IUserRepository UserRepository { get; set; }
		public IMembershipService MembershipService { get; set; }
		public IContainer Container { get; set; }

		private void EnsureInjectables()
		{
			if (BlogRepository == null) BlogRepository = Container.GetInstance<IBlogRepository>();
			if (UserRepository == null) UserRepository = Container.GetInstance<IUserRepository>();
			if (MembershipService == null) MembershipService = new AccountMembershipService();
		}

		public MetaEndpoint()
        {
			Container = (IContainer)IoC.Initialize();
			DependencyResolver.SetResolver(new WilliamfonFamilyDependencyResolver(Container));

			EnsureInjectables();
        }

		public string AddPost(string blogid, string username, string password, Post post, bool publish)
		{
			if (MembershipService.ValidateUser(username, password))
			{
				var user = UserRepository.Load(username);

				var blog = BlogRepository.New();
				blog.Entry = post.description;
				blog.Title = post.title;
				blog.IsPublished = publish;
				blog.AuthorID = user.UniqueKey;

				var savedEntry = BlogRepository.Save(blog);

				return savedEntry.UniqueKey.ToString();;
			}
			throw new XmlRpcFaultException(0, "User is not valid");
		}

		public bool UpdatePost(string postid, string username, string password, Post post, bool publish)
		{
			if (MembershipService.ValidateUser(username, password))
			{
				var blog = BlogRepository.Load(Convert.ToInt32(postid));
				if (blog == null)
					throw new XmlRpcFaultException(0, "Not an existing post");

				blog.Entry = post.description;
				blog.Title = post.title;
				blog.IsPublished = publish;

				BlogRepository.Save(blog);

				return true;
			}
			throw new XmlRpcFaultException(0, "User is not valid");
		}

		public Post GetPost(string postid, string username, string password)
		{
			if (MembershipService.ValidateUser(username, password))
			{
				var blog = BlogRepository.Load(Convert.ToInt32(postid));
				if (blog == null)
					throw new XmlRpcFaultException(0, "Not an existing post");

				Post post = new Post();
				post.title = blog.Title;
				post.description = blog.Entry;
				post.postid = postid;
				if (blog.IsPublished)
					post.dateCreated = blog.DatePublished.Value;

				return post;
			}
			throw new XmlRpcFaultException(0, "User is not valid");
		}

		public CategoryInfo[] GetCategories(string blogid, string username, string password)
		{
			if (MembershipService.ValidateUser(username, password))
			{
				return Enumerable.Empty<CategoryInfo>().ToArray(); 
			}
			throw new XmlRpcFaultException(0, "User is not valid");
		}

		public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
		{
			if (MembershipService.ValidateUser(username, password))
			{
				var user = UserRepository.Load(username);
				var blogs = BlogRepository.LoadList(new BlogFilter { AuthorName = user.UniqueKey, PageSize = numberOfPosts, LoadBlogBy = LoadBlogBy.User });
				return blogs.BlogEntries.Select(b => new Post
					{
						title = b.Title,
						description = b.Entry,
						postid = b.UniqueKey,
						dateCreated = b.IsPublished ? b.DatePublished.Value : DateTime.Now.AddYears(-10)
					}).ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid");
		}

		public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
		{
			throw new XmlRpcFaultException(0, "Upload media not yet supported");
			throw new XmlRpcFaultException(0, "User is not valid");
		}

		public bool DeletePost(string key, string postid, string username, string password, bool publish)
		{
			throw new XmlRpcFaultException(0, "Deleting not supported");
		}

		public BlogInfo[] GetUsersBlogs(string key, string username, string password)
		{
			if (MembershipService.ValidateUser(username, password))
			{
				List<BlogInfo> blogs = new List<BlogInfo>();
				blogs.Add(new BlogInfo { blogid = "sgwill", blogName = "Sam", url = "http://wwww.williamsonfamily.com/sgwill/blog/list.mvc.aspx" });
				blogs.Add(new BlogInfo { blogid = "michele", blogName = "Michele", url = "http://wwww.williamsonfamily.com/sammichele/blog/list.mvc.aspx" });
				return blogs.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid");
		}

		public UserInfo GetUserInfo(string key, string username, string password)
		{
			if (MembershipService.ValidateUser(username, password))
			{
				var user = UserRepository.Load(username);

				UserInfo userInfo = new UserInfo();
				userInfo.email = user.Email;
				userInfo.firstname = user.FirstName;
				userInfo.lastname = user.LastName;
				userInfo.nickname = user.Username;
				userInfo.userid = user.Username;

				return userInfo;
			}
			throw new XmlRpcFaultException(0, "User is not valid");
		}
	}
}