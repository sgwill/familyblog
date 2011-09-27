using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WilliamsonFamily.Models.Blog;

namespace WilliamsonFamily.Web.Models.Blog
{
    public class BlogModel
    {
        public BlogModel()
        {
            Author = new BlogAuthorModel();
        }

        public IBlog BlogEntry { get; set; }
        public BlogAuthorModel Author { get; set; }
        public IEnumerable<IBlog> FamilyEntries { get; set; }
        public IDictionary<string, IDictionary<string, IEnumerable<IBlog>>> BlogTitles { get; set; }
    }
}