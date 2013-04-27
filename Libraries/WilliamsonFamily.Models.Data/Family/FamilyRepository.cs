using System;
using System.Linq;
using StackExchange.Profiling;
using WilliamsonFamily.Models.Family;

namespace WilliamsonFamily.Models.Data
{
    public class FamilyRepository : ContextPersisterBase, IFamilyRepository
    {
        #region IModelLoader<IFamily,int> Members

        public IFamily Load(string key)
        {
            using (MiniProfiler.Current.Step("FamilyRepository.Load"))
            {
                using (var dc = DataContextFactory.GetDataContext())
                {
                    return dc.Repository<Family>()
                               .SingleOrDefault(f => f.FamilyName == key);
                }
            }
        }

        #endregion
    }
}