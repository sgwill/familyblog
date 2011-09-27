using System;

namespace WilliamsonFamily.Models
{
    public interface IModelFactory<T> where T : class
    {
        T New();
    }
}
