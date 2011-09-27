using System;
using System.Linq;
using WilliamsonFamily.Models.Family;
using MvcMiniProfiler;

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