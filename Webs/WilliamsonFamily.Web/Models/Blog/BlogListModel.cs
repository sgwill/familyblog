using System.Collections.Generic;
using WilliamsonFamily.Models;
using WilliamsonFamily.Models.Blog;
using WilliamsonFamily.Models.User;

namespace WilliamsonFamily.Web.Models.Blog
{
    public class BlogListModel
    {
        public BlogListModel()
        {
            Author = new BlogAuthorModel();
        }

        public IEnumerable<IBlog> BlogEntries { get; set; }
        public IDictionary<string, IDictionary<string, IEnumerable<IBlog>>> BlogTitles { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public BlogAuthorModel Author { get; set; }
        // TODO: Make me better
        public string Family { get; set; }
    }
}
