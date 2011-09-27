using System;
using System.Collections.Generic;
using WilliamsonFamily.Models.Web;

namespace WilliamsonFamily.Models.Blog
{
    public interface IBlogRepository : IModelLoader<IBlog, int>, IModelPersister<IBlog>, IModelFactory<IBlog>
    {
        IBlogList LoadList(BlogFilter filter);
        ITitleCleaner TitleCleaner { get; set; }
        IBlog LoadBySlug(string slug);
    }

    public class BlogFilter
    {
        public string Date { get; set; }
        public string Tags { get; set; }
        public LoadBlogBy LoadBlogBy { get; set; }
        public string AuthorName { get; set; }
        public bool? IsPublished { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
        public int PageCount { get; set; }
    }

    public enum LoadBlogBy
    {
        None,
        User,
        Family
    }
}
