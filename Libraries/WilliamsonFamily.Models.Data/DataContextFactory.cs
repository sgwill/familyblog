using System;
using WilliamsonFamily.Models.Data.WilliamsonFamily;

namespace WilliamsonFamily.Models.Data
{
    public class DataContextFactory : IDataContextFactory
    {
        #region IDataContextFactory Members

        public IDataContext GetDataContext()
        {
            return new WilliamsonFamilyDataContext();
        }

        #endregion
    }
}