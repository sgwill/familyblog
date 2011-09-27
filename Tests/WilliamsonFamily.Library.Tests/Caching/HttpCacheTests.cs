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
            // Arrange
            string key = "KeyA";
            string text = "value";
            var cache = GetCache();

            // Act
            cache.Insert(key, text);

            // Assert
            Assert.AreEqual(text, HttpRuntime.Cache[key]);
        }

        [TestMethod]
        public void HttpCache_Remove_RemovesValue()
        {
            // Arrange
            string key = "KeyB";
            string text = "value";
            var cache = GetCache();
            HttpRuntime.Cache[key] = text;

            // Act
            cache.Remove(key);

            // Assert
            Assert.IsNull(HttpRuntime.Cache[key]);
        }

        [TestMethod]
        public void HttpCache_Get_ReturnsValue()
        {
            // Arrange
            string key = "Keyc";
            string text = "value";
            var cache = GetCache();
            HttpRuntime.Cache[key] = text;

            // Act
            var test = cache.Get<string>(key, null);

            // Assert
            Assert.AreEqual(test, text);
        }

        [TestMethod]
        public void HttpCache_Get_DifferentReturnType_ReturnsNull()
        {
            // Arrange
            string key = "Keyd";
            int value = 1;
            var cache = GetCache();
            HttpRuntime.Cache[key] = value;

            // Act
            var test = cache.Get<string>(key, null);

            // Assert
            Assert.IsNull(test);
        }

        [TestMethod]
        public void HttpCache_Get_NoValue_ReturnsCallbackValue()
        {
            // Arrange
            string key = "Keyf";
            string value = "test";
            var cache = GetCache();

            // Act
            var test = cache.Get<string>(key, () => value);
            
            // Assert
            Assert.AreEqual(value, test);
        }

        [TestMethod]
        public void HttpCache_Get_NoValue_InsertsCallbackValueIntoHttpRuntime()
        {
            // Arrange
            string key = "Keyg";
            string value = "test";
            var cache = GetCache();

            // Act
            cache.Get<string>(key, () => value);

            // Assert
            Assert.AreEqual(value, HttpRuntime.Cache[key]);
        }

        #region GetHttpCache - Helper
        HttpCache GetCache()
        {
            
            return new HttpCache();
        }
        #endregion
    }
}