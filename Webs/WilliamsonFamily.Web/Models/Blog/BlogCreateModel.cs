using System;
using WilliamsonFamily.Models.Blog;
using WilliamsonFamily.Models.User;

namespace WilliamsonFamily.Web.Models.Blog
{
    public class BlogCreateModel
    {
        public string Title { get; set; }
        public string Entry { get; set; }
        public string Tags { get; set; }
        public string AuthorID { get; set; }
        public bool IsEdit { get; set; }
        public bool IsPublished { get; set; }
		public int UniqueKey { get; set; }
		public DateTime? DatePublished { get; set; }
    }
}