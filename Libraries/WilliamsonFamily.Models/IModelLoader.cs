using System;

namespace WilliamsonFamily.Models
{
    public interface IModelLoader<Model, Key> 
        where Model : class
    {
        Model Load(Key Key);
    }
}
