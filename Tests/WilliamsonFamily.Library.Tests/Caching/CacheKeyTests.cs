using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Library.Web.Caching;

namespace WilliamsonFamily.Library.Tests.Caching
{
    [TestClass]
    public class CacheKeyTests
    {
        string baseKey = "**WilliamsonFamilyWeb";

        [TestMethod]
        public void CacheKey_BaseCacheKey_ReturnsBaseValue()
        {
            CacheKeyBase cacheKey = new CacheKeyBase();
            Assert.AreEqual(baseKey, cacheKey.CacheKeyBaseKey);
        }

        [TestMethod]
        public void CacheKey_BlogEntryCacheKey_GeneratesKey()
        {
            string blogKey = "Some-title";
            BlogEntryCacheKey cacheKey = new BlogEntryCacheKey();
            Assert.AreEqual(string.Format("{0}:BlogEntry:{1}", baseKey, blogKey), cacheKey.GenerateKey(blogKey));
        }

        [TestMethod]
        public void CacheKey_UserCacheKey_GeneratesKey()
        {
            string userKey = "sgwill";
            UserCacheKey cacheKey = new UserCacheKey();
            Assert.AreEqual(string.Format("{0}:User:{1}", baseKey, userKey), cacheKey.GenerateKey(userKey));
        }

        [TestMethod]
        public void CacheKey_BlogListCacheKey_GeneratesKey()
        {
            string userKey = "sgwill";
            BlogListCacheKey cacheKey = new BlogListCacheKey();
            Assert.AreEqual(string.Format("{0}:BlogList:{1}", baseKey, userKey), cacheKey.GenerateKey(userKey));
        }
    }
}