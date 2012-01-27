using System;
using System.Collections.Generic;
using WilliamsonFamily.Models.Blog;

namespace WilliamsonFamily.Models.Search
{
    public interface IBlogIndexSearcher
    {
        IEnumerable<IBlog> SearchIndex(string author, string searchTerm);
    }
}