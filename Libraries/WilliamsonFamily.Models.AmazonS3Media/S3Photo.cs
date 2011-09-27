using System;
using WilliamsonFamily.Models.Photo;

namespace WilliamsonFamily.Models.AmazonS3Media
{
    public class S3Photo : IPhoto
    {
        public string WebUrl { get; set; }
        public string Title { get; set; }
        public string UniqueKey { get; set; }
    }
}