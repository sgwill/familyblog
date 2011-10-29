using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using MvcMiniProfiler;
using WilliamsonFamily.Library.Exceptions;
using WilliamsonFamily.Library.Web;
using WilliamsonFamily.Library.Web.Caching;
using WilliamsonFamily.Library.Web.Filters;
using WilliamsonFamily.Models.Blog;
using WilliamsonFamily.Models.Caching;
using WilliamsonFamily.Models.Family;
using WilliamsonFamily.Models.User;
using WilliamsonFamily.Web.Models.Blog;
using WilliamsonFamily.Web.Web;
using WilliamsonFamily.Library.Web.Routing;

namespace WilliamsonFamily.Web.Controllers
{
	public class BlogController : BaseController
	{
		#region Injectables
		public IBlogRepository BlogRepository { get; set; }
		public IUserRepository UserRepository { get; set; }
		public IFamilyRepository FamilyRepository { get; set; }
		public ICache Cache { get; set; }

		private void EnsureInjectables()
		{
			if (BlogRepository == null) throw new InjectablePropertyNullException("BlogRepository");
			if (UserRepository == null) throw new InjectablePropertyNullException("UserRepository");
			if (FamilyRepository == null) throw new InjectablePropertyNullException("FamilyRepository");
			if (Cache == null) throw new InjectablePropertyNullException("Cache");
		}
		#endregion

		#region List
		[CompressActionFilter]
		[Route("{user}/blog/list.aspx", RoutePriority.High)]
		public ActionResult List(string user)
		{
			EnsureInjectables();
			string date = "";
			var viewData = GetBlogEntries(user, date, true);

			if (viewData == null)
			{
				TempData["Message"] = "Invalid User or Family";
				return RedirectToAction("Index", "Home");
			}

			return View("List", viewData);
		}

		private BlogListModel GetBlogEntries(string user, string date, bool getIsPublishedOnly)
		{
			return GetBlogEntries(user, date, getIsPublishedOnly, 10);
		}

		private BlogListModel GetBlogEntries(string user, string date, bool getIsPublishedOnly, int defaultPageSize)
		{
			using (profiler.Step("BlogController.GetBlogEntries"))
			{
				var viewData = new BlogListModel();
				var blogFilter = new BlogFilter();

				var theUser = UserRepository.Load(user);
				if (theUser == null)
				{
					var family = FamilyRepository.Load(user);
					if (family == null)
					{
						return null;
					}
					else
					{
						blogFilter.AuthorName = user;
						blogFilter.LoadBlogBy = LoadBlogBy.Family;
						viewData.Author.FirstName = family.Description;
					}
				}
				else
				{
					blogFilter.AuthorName = theUser.UniqueKey;
					blogFilter.LoadBlogBy = LoadBlogBy.User;
					viewData.Author.FirstName = theUser.FirstName;
				}

				viewData.Author.UrlName = user;

				if (!string.IsNullOrEmpty(date))
					blogFilter.Date = date;
				if (getIsPublishedOnly)
					blogFilter.IsPublished = true;

                ICacheKey cacheKey = new BlogListCacheKey();
                viewData.BlogTitles = Cache.Get<IDictionary<string, IDictionary<string, IEnumerable<IBlog>>>>(cacheKey.GenerateKey(string.Format("SidebarList:{0}", blogFilter.AuthorName)), () =>
                                GenerateBlogTitles(BlogRepository
                                    .LoadList(blogFilter)
                                    .BlogEntries
                                    .OrderByDescending(b => b.DatePublished)));

				int pageSize = 10;
				if (Int32.TryParse(QueryValue("pageSize"), out pageSize))
					blogFilter.PageSize = pageSize;
				else
					blogFilter.PageSize = defaultPageSize;

				int pageIndex = 1;
				if (Int32.TryParse(QueryValue("page"), out pageIndex))
					blogFilter.PageIndex = pageIndex;
				else
					blogFilter.PageIndex = 1;


				var model = BlogRepository.LoadList(blogFilter);
				viewData.BlogEntries = model.BlogEntries.OrderByDescending(b => b.DatePublished);
				viewData.PageCount = model.PageCount;
				viewData.PageIndex = model.PageIndex;

				return viewData;
			}
		}

		private IDictionary<string, IDictionary<string, IEnumerable<IBlog>>> GenerateBlogTitles(IEnumerable<IBlog> blogEntries)
		{
			// Year, blogs
			IDictionary<string, IEnumerable<IBlog>> e = blogEntries.GroupBy(b => b.DatePublished.Value.Year.ToString(), b => b).ToDictionary(a => a.Key, a => a.AsEnumerable());
			// Year, Month, Blogs
			Dictionary<string, IDictionary<string, IEnumerable<IBlog>>> all = new Dictionary<string, IDictionary<string, IEnumerable<IBlog>>>();

			foreach (var year in e)
			{
				IDictionary<string, IEnumerable<IBlog>> month = year.Value.GroupBy(a => a.DatePublished.Value.ToString("MMM"), a => a).ToDictionary(a => a.Key, a => a.AsEnumerable());
				all.Add(year.Key, month);
			}

			return all;
		}
		#endregion

		#region UserList
		[Authorize]
		[Route("{user}/blog/userlist.aspx")]
		public ActionResult UserList(string user)
		{
			EnsureInjectables();
			var viewData = GetBlogEntries(user, "", false, 500);

			if (viewData == null)
			{
				TempData["Message"] = "Invalid User or Family";
				return RedirectToAction("Index", "Account");
			}

			viewData.BlogEntries = viewData.BlogEntries
								  .OrderBy(b => b.IsPublished)
								  .ThenByDescending(b => b.DatePublished);
			// TODO: Instead, load based on preference
			if (viewData.Author.FirstName == "Sam" || viewData.Author.FirstName == "Michele")
				viewData.Family = "sammichele";

			return View("UserList", viewData);
		}
		#endregion

		#region Feed
		[Route("{user}/blog/list.xml.aspx")]
		public ActionResult Feed(string user)
		{
			EnsureInjectables();
			var viewData = GetBlogEntries(user, null, true);

			List<SyndicationItem> items = new List<SyndicationItem>();
			foreach (var b in viewData.BlogEntries)
			{
				string url = Url.Action("Detail", new { title = b.Slug });

				var item = new SyndicationItem(b.Title, null, new Uri(string.Format("http://www.williamsonfamily.com{0}", url)))
				{
					Summary = new TextSyndicationContent(b.Entry, TextSyndicationContentKind.Html),
					LastUpdatedTime = new DateTimeOffset(b.DatePublished.Value),
					PublishDate = new DateTimeOffset(b.DatePublished.Value),

				};

				item.Authors.Add(new SyndicationPerson { Name = viewData.Author.FirstName });
				items.Add(item);
			}

			var feed = new SyndicationFeed(viewData.Author.FirstName + "'s feed", viewData.Author.Bio, new Uri(string.Format("http://www.williamsonfamily.com/{0}/Blog/List.xml.aspx/", viewData.Author.UrlName)), items);
			feed.Links.Add(new SyndicationLink { Title = "WilliamsonFamily.com", Uri = new Uri("http://www.williamsonfamily.com") });

			return new RssResult(feed);
		}
		#endregion

		#region Detail
		[CompressActionFilter]
		[Route("{user}/blog/{title}.aspx", RoutePriority.Low)]
		public ActionResult Detail(string title)
		{
			EnsureInjectables();

			if (String.IsNullOrEmpty(title))
				return RedirectToAction("List");

			var entry = BlogRepository.LoadBySlug(title);
			if (entry == null)
			{
				TempData["Message"] = "No Blog";
				return RedirectToAction("List");
			}

			var viewData = new BlogModel();
			viewData.BlogEntry = entry;

			var user = UserRepository.Load(entry.AuthorName);
			if (user != null)
			{
				viewData.Author.UrlName = user.Username;
				viewData.Author.FirstName = user.FirstName;
			}

			//ICacheKey cacheKey = new BlogListCacheKey();
			//viewData.FamilyEntries = Cache.Get<IEnumerable<IBlog>>(cacheKey.GenerateKey(""), null);

			return View("Details", viewData);

		}
		#endregion

		#region Create
		[Authorize]
		[AcceptVerbs(HttpVerbs.Get)]
		[Route("{user}/blog/create.aspx")]
		public ActionResult Create(string user)
		{
			EnsureInjectables();

			if (user.ToLower() != HttpContext.User.Identity.Name.ToLower())
				return RedirectToAction("Create", "Blog", new { user = HttpContext.User.Identity.Name });

			var dbUser = UserRepository.Load(user);
			if (dbUser == null)
			{
				var family = FamilyRepository.Load(user);
				if (family != null)
					TempData["Message"] = "Cannot create blog entries as Family";
				return RedirectToAction("Index", "Home");
			}

			var viewData = new BlogCreateModel();
			viewData.AuthorID = dbUser.UniqueKey;

			return View("Create", viewData);
		}

		[Authorize]
		[ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post)]
		[Route("{user}/blog/create.aspx", HttpVerbs.Post)]
		public ActionResult Create(BlogCreateModel viewData)
		{
			EnsureInjectables();

			if (string.IsNullOrEmpty(viewData.Title))
				ModelState.AddModelError("Title", "You must supply a Title");
			if (string.IsNullOrEmpty(viewData.Entry))
				viewData.Entry = Request.Form["textEntry"];
			if (string.IsNullOrEmpty(viewData.Entry))
				ModelState.AddModelError("Entry", "You must enter your Entry!");
			var previousEntry = BlogRepository.LoadBySlug(viewData.Title.BlogUrl());
			if (previousEntry != null)
				ModelState.AddModelError("DuplicateTitle", "An Entry with this Title already exists. Please enter a different Title.");

			if (!ModelState.IsValid)
				return View("Create", viewData);

			var blogEntry = BlogRepository.New();
			TryUpdateModel<IBlog>(blogEntry);
			BlogRepository.Save(blogEntry);

			if (blogEntry.IsPublished)
				Cache.Remove(new BlogListCacheKey().GenerateKey("SidebarList"));

			return RedirectToAction("UserList");
		}
		#endregion

		#region Edit Post
		[Authorize]
		[AcceptVerbs(HttpVerbs.Get)]
		[Route("{user}/blog/edit/{slug}.aspx")]
		public ActionResult Edit(string user, string slug)
		{
			EnsureInjectables();

			if (string.IsNullOrEmpty(slug))
				return RedirectToAction("Index", "Home");

			var blog = BlogRepository.LoadBySlug(slug);
			if (blog == null)// || blog.AuthorID != user)
				return RedirectToAction("Index", "Home");

			var data = new BlogCreateModel();
			data.Entry = blog.Entry;
			data.Title = blog.Title;
			data.Tags = blog.Tags;
			data.AuthorID = blog.AuthorID;
			data.IsEdit = true;
			data.IsPublished = blog.IsPublished;

			return View("Edit", data);
		}

		[Authorize]
		[ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post)]
		[Route("{user}/blog/edit/{slug}.aspx", HttpVerbs.Post)]
		public ActionResult Edit(BlogCreateModel viewData)
		{
			EnsureInjectables();

			if (string.IsNullOrEmpty(viewData.Entry))
				viewData.Entry = Request.Form["textEntry"];
			if (string.IsNullOrEmpty(viewData.Entry))
				ModelState.AddModelError("Entry", "You must enter your Entry!");

			if (!ModelState.IsValid)
				return View("Edit", viewData);

			var blog = BlogRepository.LoadBySlug(viewData.Title.BlogUrl());
			blog.Entry = viewData.Entry;
			blog.Tags = viewData.Tags;
			if (!blog.IsPublished && viewData.IsPublished)
				blog.IsPublished = viewData.IsPublished;
			BlogRepository.Save(blog);

			Cache.Remove(new BlogListCacheKey().GenerateKey("SidebarList"));

			return RedirectToAction("UserList");
		}
		#endregion
	}
}