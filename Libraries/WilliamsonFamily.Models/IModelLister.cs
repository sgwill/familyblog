using System;
using System.Collections.Generic;

namespace WilliamsonFamily.Models
{
    public interface IModelLister<Model, Key> where Model : class
    {
        IEnumerable<Model> LoadList(Key Key);
    }
}
