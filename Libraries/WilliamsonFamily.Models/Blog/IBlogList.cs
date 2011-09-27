using System;
using System.Collections.Generic;

namespace WilliamsonFamily.Models.Blog
{
    public interface IBlogList
    {
        IEnumerable<IBlog> BlogEntries { get; }
        int PageCount { get; }
        int PageIndex { get; }
    }
}
