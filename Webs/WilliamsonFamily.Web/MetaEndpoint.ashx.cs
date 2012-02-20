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

namespace WilliamsonFamily.Web
{
	/// <summary>
	/// Summary description for MetaEndpoint
	/// </summary>
	public class MetaEndpoint : XmlRpcService, IMetaWeblog
    {
		public IUserRepository UserRepository { get; set; }
		public IBlogRepository BlogRepository { get; set; }
		public IContainer Container { get; set; }

		private void EnsureInjectables()
		{
			if (BlogRepository == null) BlogRepository = Container.GetInstance<IBlogRepository>();// throw new InjectablePropertyNullException("BlogRepository");
			if (UserRepository == null) UserRepository = Container.GetInstance<IUserRepository>(); // throw new InjectablePropertyNullException("UserRepository");
		}

		public MetaEndpoint()
        {
			Container = (IContainer)IoC.Initialize();
			DependencyResolver.SetResolver(new WilliamfonFamilyDependencyResolver(Container));
        }

		public string AddPost(string blogid, string username, string password, Post post, bool publish)
		{
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public bool UpdatePost(string postid, string username, string password, Post post, bool publish)
		{
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public Post GetPost(string postid, string username, string password)
		{
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public CategoryInfo[] GetCategories(string blogid, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				return new List<CategoryInfo>().ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
		{
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
		{
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public bool DeletePost(string key, string postid, string username, string password, bool publish)
		{
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public BlogInfo[] GetUsersBlogs(string key, string username, string password)
		{
			if (ValidateUser(username, password))
			{
				List<BlogInfo> blogs = new List<BlogInfo>();
				blogs.Add(new BlogInfo { blogid = "sgwill", blogName = "sgwill", url = "http://wwww.williamsonfamily.com/sgwill/blog/" });
				return blogs.ToArray();
			}
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		public UserInfo GetUserInfo(string key, string username, string password)
		{
			throw new XmlRpcFaultException(0, "User is not valid!");
		}

		private bool ValidateUser(string username, string password)
		{
			EnsureInjectables();

			var user = UserRepository.Load(username);
			if (user == null)
				return false;

			return true;
		}
	}
}