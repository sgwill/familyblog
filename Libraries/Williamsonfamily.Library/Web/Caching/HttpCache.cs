using System;
using WilliamsonFamily.Models.Caching;
using System.Web;
using MvcMiniProfiler;

namespace WilliamsonFamily.Library.Web.Caching
{
    public class HttpCache : ICache
    {
        public HttpCache()
        {
            if (HttpRuntime.Cache == null) throw new ApplicationException("HttpRuntime.Cache is required for the HttpCache");
        }

        #region ICache Members

        public T Get<T>(string key, Func<T> initializeIfNull)
        {
            var item = HttpRuntime.Cache.Get(key);

            if (item == null)
            {
                using (MiniProfiler.Current.Step("Cache Miss"))
                {
                    if (initializeIfNull != null)
                    {
                        item = initializeIfNull();
                        Insert(key, item);
                    }
                }
            }

            if (item is T)
                return (T)item;
            return default(T);
        }

        public void Insert(string key, object value)
        {
            HttpRuntime.Cache.Insert(key, value);
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        #endregion
    }
}
