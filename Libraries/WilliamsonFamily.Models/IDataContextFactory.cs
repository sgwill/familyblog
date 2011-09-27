using System;

namespace WilliamsonFamily.Models
{
    public interface IDataContextFactory
    {
        IDataContext GetDataContext();
    }
}