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
        DateTime? DatePublished { get; }
        bool IsPublished { get; set; }
    }
}