using System;
using WilliamsonFamily.Models.Photo;

namespace WilliamsonFamily.Models.FlickrPhoto
{
    public class FlickrPhoto : IPhoto
    {
        #region IUniqueKey<string> Members

        public string UniqueKey
        {
            get;
            set;
        }

        #endregion

        #region IPhoto Members

        public string WebUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        #endregion
    }
}