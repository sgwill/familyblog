using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WilliamsonFamily.Library.Web.Caching;
using System.Web;

namespace WilliamsonFamily.Library.Tests.Caching
{
    [TestClass]
    public class HttpCacheTests
    {
        [TestMethod]
        public void HttpCache_Insert_InsertsValueIntoHttpCache()
        {
            string key = "KeyA";
            string text = "value";

            cache.Insert(key, text);

            Assert.AreEqual(text, HttpRuntime.Cache[key]);
        }

        [TestMethod]
        public void HttpCache_Remove_RemovesValue()
        {
            string key = "KeyB";
            string text = "value";
            HttpRuntime.Cache[key] = text;

            cache.Remove(key);

            Assert.IsNull(HttpRuntime.Cache[key]);
        }

        [TestMethod]
        public void HttpCache_Get_ReturnsValue()
        {
            string key = "Keyc";
            string text = "value";
            HttpRuntime.Cache[key] = text;

            var test = cache.Get<string>(key, null);

            Assert.AreEqual(test, text);
        }

        [TestMethod]
        public void HttpCache_Get_DifferentReturnType_ReturnsNull()
        {
            string key = "Keyd";
            int value = 1;
            HttpRuntime.Cache[key] = value;

            var test = cache.Get<string>(key, null);

            Assert.IsNull(test);
        }

        [TestMethod]
        public void HttpCache_Get_NoValue_ReturnsCallbackValue()
        {
            string key = "Keyf";
            string value = "test";

            var test = cache.Get<string>(key, () => value);
            
            Assert.AreEqual(value, test);
        }

        [TestMethod]
        public void HttpCache_Get_NoValue_InsertsCallbackValueIntoHttpRuntime()
        {
            string key = "Keyg";
            string value = "test";

            cache.Get<string>(key, () => value);

            Assert.AreEqual(value, HttpRuntime.Cache[key]);
        }

        HttpCache cache;
        [TestInitialize]
        public void Init()
        {
            cache = new HttpCache();
        }
    }
}