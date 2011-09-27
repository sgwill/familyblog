using System;
using WilliamsonFamily.Models.Caching;

namespace WilliamsonFamily.Library.Web.Caching
{
    public class BlogListCacheKey : CacheKeyBase, ICacheKey
    {
        #region ICacheKey Members

        public string GenerateKey(string partialKey)
        {
            return string.Format("{0}:BlogList:{1}", CacheKeyBaseKey, partialKey);
        }

        #endregion
    }
}
