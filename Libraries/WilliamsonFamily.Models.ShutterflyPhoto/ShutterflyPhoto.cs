using System;
using WilliamsonFamily.Models.Photo;

namespace WilliamsonFamily.Models.ShutterflyPhoto
{
    public class ShutterflyPhoto : IPhoto
    {
        public string WebUrl { get; set; }
        public string Title { get; set; }
        public string UniqueKey { get; set; }
    }
}
