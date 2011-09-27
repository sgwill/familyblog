using System;
using WilliamsonFamily.Models;
using WilliamsonFamily.Models.Data.Tests;

namespace WilliamsonFamily.Models.Data.Tests
{
    public class InMemoryDataContextFactory : IDataContextFactory
    {
        internal IDataContext DataContext { get; set; }

        #region IDataContextFactory Members

        public IDataContext GetDataContext()
        {
            return DataContext ?? new InMemoryDataContext();
        }

        #endregion
    }
}