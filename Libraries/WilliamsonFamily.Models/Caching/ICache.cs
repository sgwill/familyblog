using System;

namespace WilliamsonFamily.Models.Caching
{
    public interface ICache
    {
        T Get<T>(string key, Func<T> initializeIfNull);
        void Insert(string key, object value);
        void Remove(string key);
    }
}
