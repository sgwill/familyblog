using System;
using System.Collections.Generic;

namespace WilliamsonFamily.Models.Photo
{
    public interface IPhoto : IUniqueKey<string>
    {
        string WebUrl { get; }
        string Title { get; }
    }
}