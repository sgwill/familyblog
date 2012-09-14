using System;
using System.Collections.Generic;
using System.Linq;
using WilliamsonFamily.Models.Blog;
using WilliamsonFamily.Models.Web;
using WilliamsonFamily.Library.Exceptions;
using MvcMiniProfiler;
using WilliamsonFamily.Models.Caching;
using WilliamsonFamily.Library.Web.Caching;

namespace WilliamsonFamily.Models.Data
{
    public class BlogRepository : ContextPersisterBase, IBlogRepository
    {
        public ITitleCleaner TitleCleaner { get; set; }
        public ICache Cache { get; set; }

        void EnsureInjectables()
        {
            if (TitleCleaner == null) throw new InjectablePropertyNullException("TitleCleaner");
            if (Cache == null) throw new InjectablePropertyNullException("Cache");
        }

        #region IModelLoader<IBlog,int> Members

        public IBlog Load(int Key)
        {
            using (var dc = DataContextFactory.GetDataContext())
            {
                return dc.Repository<Blog>()
                        .SingleOrDefault(b => b.PkID == Key);
            }
        }

        #endregion

        #region LoadBySlug
        public IBlog LoadBySlug(string slug)
        {
            using (MiniProfiler.Current.Step("BlogRepository.LoadBySlug"))
            {
                EnsureInjectables();

                using (var dc = DataContextFactory.GetDataContext())
                {
                    var blog = dc.Repository<Blog>()
                               .Where(b => b.Slug == slug)
                               .OrderByDescending(b => b.DatePublished)
                               .FirstOrDefault();

                    return blog;
                }
            }
        }
        #endregion

        #region LoadList
        public IBlogList LoadList(BlogFilter filter)
        {
            using (MiniProfiler.Current.Step("BlogRepository.LoadList"))
            {
                EnsureInjectables();

                BlogList model = new BlogList();

                using (var dc = DataContextFactory.GetDataContext())
                {
                    IEnumerable<IBlog> blogs = dc.Repository<Blog>()
                                                .AsEnumerable()
                                                .Cast<IBlog>();

                    if (filter.LoadBlogBy == LoadBlogBy.User)
                    {
                        blogs = LoadByUser(blogs, filter.AuthorName);
                    }
                    else if (filter.LoadBlogBy == LoadBlogBy.Family)
                    {
                        blogs = LoadByFamily(blogs, filter.AuthorName, dc);
                    }

                    if (!String.IsNullOrEmpty(filter.Date))
                    {
                        blogs = LoadByDate(blogs, filter.Date);
                    }

                    if (!String.IsNullOrEmpty(filter.Tags))
                    {
                        blogs = LoadByTags(blogs, filter.Tags);
                    }

					if (filter.IsPublished.HasValue)
					{
						blogs = blogs.Where(b => b.IsPublished == filter.IsPublished.Value);
						blogs = blogs.Where(b => b.DatePublished.Value <= DateTime.Now);
					}

                    blogs = blogs.OrderByDescending(b => b.DatePublished);

                    using (MiniProfiler.Current.Step("BlogRepository.LoadList.Paging"))
                    {
                        if (filter.PageSize.HasValue)
                        {
                            int count = Cache.Get<int>(new BlogListCountCacheKey().GenerateKey(filter.AuthorName), () => blogs.Count());
                            decimal pagesDecimal = (decimal)count / (decimal)filter.PageSize.Value;
                            if (pagesDecimal % 1  > 0) pagesDecimal += 0.5M;
                            int pages = Convert.ToInt32(Math.Round(pagesDecimal, 0, MidpointRounding.AwayFromZero));

                            int pageIndex = filter.PageIndex.HasValue ? filter.PageIndex.Value - 1 : 0;
                            if (pageIndex > pages)
                                pageIndex = 0;

                            var skip = pageIndex * filter.PageSize.Value;
                            model.PageIndex = pageIndex + 1;

                            model.PageCount = pages;
                            blogs = blogs.Skip(skip).Take(filter.PageSize.Value);
                        }
                    }

                    model.BlogEntries = blogs.ToList();

                    return model;
                }
            }
        }

        private IEnumerable<IBlog> LoadByUser(IEnumerable<IBlog> blogs, string user)
        {
            using (MiniProfiler.Current.Step("BlogRepository.LoadByUser"))
            {
                return blogs.Where(b => b.AuthorID == user);
            }
        }

        private IEnumerable<IBlog> LoadByFamily(IEnumerable<IBlog> blogs, string familyName, IDataContext dc)
        {
            using (MiniProfiler.Current.Step("BlogRepository.LoadByFamily"))
            {
                return from b in blogs
                       join uf in dc.Repository<UserFamily>() on b.AuthorID equals uf.UserID
                       join f in dc.Repository<Family>() on uf.FamilyID equals f.PkID
                       where f.FamilyName.ToLower() == familyName.ToLower()
                       select (IBlog)b;
            }
        }

        private IEnumerable<IBlog> LoadByDate(IEnumerable<IBlog> blogs, string date)
        {
            using (MiniProfiler.Current.Step("BlogRepository.LoadByDate"))
            {
                string year = "";
                string month = "";
                string day = "";
                string[] split = date.Split('/');

                if (split.Count() == 0)
                    year = date;
                if (split.Count() >= 1)
                    year = split[0];
                if (split.Count() >= 2)
                    month = split[1];
                if (split.Count() == 3)
                    day = split[2];

                if (!String.IsNullOrEmpty(month) && month[0] == '0')
                    month = month.Replace("0", "");
                if (!String.IsNullOrEmpty(day) && day[0] == '0')
                    day = day.Replace("0", "");

                if (!String.IsNullOrEmpty(year))
                    blogs = blogs.Where(b => b.DatePublished.Value.Year.ToString() == year);
                if (!String.IsNullOrEmpty(month))
                    blogs = blogs.Where(b => b.DatePublished.Value.Month.ToString() == month);
                if (!String.IsNullOrEmpty(day))
                    blogs = blogs.Where(b => b.DatePublished.Value.Day.ToString() == day);

                return blogs;
            }
        }

        private IEnumerable<IBlog> LoadByTags(IEnumerable<IBlog> blogs, string tags)
        {
            using (MiniProfiler.Current.Step("BlogRepository.LoadByTags"))
            {
                string[] split = tags.Split(' ');

                for (int i = 0; i < split.Count(); i++)
                {
                    string tag = split[i].ToLower(); // I have to do this because of a late-bound issue. weird
                    blogs = blogs.Where(b => b.Tags.ToLower().Contains(tag));
                }

                return blogs;
            }
        }
        #endregion

        #region IModelPersister<IBlog> Members

        public IBlog Save(IBlog model)
        {
            using (MiniProfiler.Current.Step("BlogRepository.Save"))
            {
                EnsureInjectables();

                using (var dc = DataContextFactory.GetDataContext())
                {
                    Blog modelAsBlog = null;
                    if (model.UniqueKey == 0)
                    {
                        modelAsBlog = new Blog();
                        modelAsBlog.DatePublished = DateTime.Now;
                    }
                    else
                        modelAsBlog = dc.Repository<Blog>()
                            .Where(b => b.PkID == model.UniqueKey)
                            .SingleOrDefault();

                    if (modelAsBlog == null)
                        throw new ArgumentException("Attempted to Update nonexisting Blog");

                    modelAsBlog.AuthorID = model.AuthorID;
                    modelAsBlog.Entry = model.Entry;
                    modelAsBlog.Tags = model.Tags;
                    modelAsBlog.Title = model.Title;
                    modelAsBlog.Slug = TitleCleaner.CleanTitle(model.Title);
                    if (!modelAsBlog.IsPublished && model.IsPublished)
                    {
						modelAsBlog.DatePublished = model.DatePublished.HasValue && model.DatePublished.Value > DateTime.Now ? model.DatePublished : DateTime.Now;
                        modelAsBlog.IsPublished = model.IsPublished;
                    }
                    modelAsBlog.AuthorName = dc.Repository<User>()
                                        .SingleOrDefault(u => u.PkID == modelAsBlog.AuthorID)
                                        .FirstName;

                    using (MiniProfiler.Current.Step("BlogRepository.Save.GenerateTags"))
                    {
                        // Note: This is probably slow. Good place for better algorithm
                        if (!String.IsNullOrEmpty(modelAsBlog.Tags))
                        {
                            var split = modelAsBlog.Tags.Split(' ').OrderBy(t => t[0]);
                            modelAsBlog.Tags = "";
                            foreach (var tag in split)
                                modelAsBlog.Tags += tag.ToLower() + " ";
                            modelAsBlog.Tags = modelAsBlog.Tags.TrimEnd();
                        }
                    }

                    if (modelAsBlog.PkID == 0)
                        dc.Insert(modelAsBlog);

                    dc.Commit();

                    return modelAsBlog;
                }
            }
        }

        #endregion

        #region IModelFactory<IBlog> Members

        public IBlog New()
        {
            return new Blog();
        }

        #endregion

		#region DeleteUnpublished
		public void DeleteUnpublished(int id)
		{
			using (MiniProfiler.Current.Step("BlogRepository.DeleteUnpublished"))
			{
				EnsureInjectables();

				using (var dc = DataContextFactory.GetDataContext())
				{
					var blog = dc.Repository<Blog>()
							   .Where(b => b.PkID == id)
							   .Where(b => b.IsPublished == false)
							   .FirstOrDefault();

					if (blog != null)
					{
						dc.Delete(blog);
						dc.Commit();
					}
				}
			}
		}
		#endregion
	}
}