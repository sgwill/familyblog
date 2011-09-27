using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WilliamsonFamily.Models.Caching;

namespace WilliamsonFamily.Tests.Web.Helpers
{
    public class InMemoryCache : ICache
    {
        private readonly Dictionary<string, object> _inMemoryCache = new Dictionary<string, object>();
        
        #region ICache Members

        public T Get<T>(string key, Func<T> initializeIfNull)
        {
            var item = _inMemoryCache.ContainsKey(key) ? _inMemoryCache[key] : default(T);

            if (item == null)
            {
                if (initializeIfNull != null)
                {
                    item = initializeIfNull();
                    Insert(key, item);
                }
            }

            if (item is T)
                return (T)item;
            return default(T);
        }

        public void Insert(string key, object value)
        {
            _inMemoryCache[key] = value;
        }

        public void Remove(string key)
        {
            _inMemoryCache.Remove(key);
        }

        #endregion
    }
}
