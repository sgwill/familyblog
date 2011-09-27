using System;

namespace WilliamsonFamily.Models
{
    public interface IModelRepository<Model, LoadKey, ListKey> : IModelLoader<Model, LoadKey>, IModelLister<Model, ListKey>, IModelPersister<Model>, IModelFactory<Model>
         where Model : class
    {
    }
}
