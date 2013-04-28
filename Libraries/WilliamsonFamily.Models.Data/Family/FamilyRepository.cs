using System;
using System.Linq;
using StackExchange.Profiling;
using WilliamsonFamily.Models.Family;

namespace WilliamsonFamily.Models.Data
{
    public class FamilyRepository : ContextPersisterBase, IFamilyRepository
    {
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

		public IFamily LoadFromUsername(string username)
		{
			using (MiniProfiler.Current.Step("FamilyRepository.LoadFromUsername"))
			{
				using (var dc = DataContextFactory.GetDataContext())
				{
					return (from f in dc.Repository<Family>()
						   join uf in dc.Repository<UserFamily>() on f.PkID equals uf.FamilyID
						   join u in dc.Repository<User>() on uf.UserID equals u.PkID
						   where u.Username == username
						   select f).FirstOrDefault();
				}
			}
		}
    }
}