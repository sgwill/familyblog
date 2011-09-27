using System;
using WilliamsonFamily.Models.Data.WilliamsonFamily;

namespace WilliamsonFamily.Models.Data
{
    public class ContextPersisterBase
    {
        public ContextPersisterBase() { }
        public ContextPersisterBase(IDataContext dataContext)
        {
            _DataContext = dataContext;
        }

        IDataContext _DataContext = null;
        public IDataContext DataContext
        {
            get { return _DataContext ?? (_DataContext = new WilliamsonFamilyDataContext()); }
            set { _DataContext = value; }
        }

        IDataContextFactory _DataContextFactory = null;
        public IDataContextFactory DataContextFactory
        {
            get { return _DataContextFactory ?? (_DataContextFactory = new DataContextFactory()); }
            set { _DataContextFactory = value; }
        }
    }
}