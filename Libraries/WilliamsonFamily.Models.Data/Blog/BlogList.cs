using System;
using System.Collections.Generic;
using WilliamsonFamily.Models.Blog;

namespace WilliamsonFamily.Models.Data
{
    public class BlogList : IBlogList
    {
        public IEnumerable<IBlog> BlogEntries { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
    }
}