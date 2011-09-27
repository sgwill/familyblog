using System;
using System.Collections.Generic;
using System.Linq;
using WilliamsonFamily.Models;

namespace WilliamsonFamily.Models.Data.Tests
{
    public class InMemoryDataContext : IDataContext
    {
        private readonly List<object> _inMemoryDataStore = new List<object>();

        public IQueryable<T> Repository<T>() where T : class
        {
            var query = from objects in _inMemoryDataStore
                        where typeof(T).IsAssignableFrom(objects.GetType())
                        select objects;

            return query.Select(o => (T)o).AsQueryable();
        }

        public void Insert<T>(T item) where T : class
        {
            _inMemoryDataStore.Add(item);
        }

        public void Delete<T>(T item) where T : class
        {
            _inMemoryDataStore.Remove(item);
        }

        public void Commit()
        {
            InvokeCompleted(EventArgs.Empty);
        }

        public event EventHandler Completed;

        private void InvokeCompleted(EventArgs e)
        {
            EventHandler completedHandler = Completed;
            if (completedHandler != null) completedHandler(this, e);
        }

        public void Dispose()
        {
            // Nothing to do here
        }
    }
}