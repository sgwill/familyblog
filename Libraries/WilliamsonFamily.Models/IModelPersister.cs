using System;

namespace WilliamsonFamily.Models
{
    public interface IModelPersister<T> where T : class
    {
        T Save(T model);
    }
}
