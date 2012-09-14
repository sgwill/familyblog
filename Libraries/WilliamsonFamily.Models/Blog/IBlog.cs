using System;
using System.Collections.Generic;

namespace WilliamsonFamily.Models.Blog
{
    public interface IBlog : IUniqueKey<int>
    {
        string AuthorID { get; set; }
        string AuthorName { get; set; }
        string Title { get; set; }
        string Slug { get; }
        string Entry { get; set; }
        string Tags { get; set; }
		DateTime? DatePublished { get; set; }
        bool IsPublished { get; set; }
    }
}