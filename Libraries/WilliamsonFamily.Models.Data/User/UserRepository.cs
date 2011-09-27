using System;
using System.Linq;
using WilliamsonFamily.Models.User;
using MvcMiniProfiler;

namespace WilliamsonFamily.Models.Data
{
    public class UserRepository : ContextPersisterBase, IUserRepository
    {
        #region Load

        public IUser Load(string username)
        {
            using (MiniProfiler.Current.Step("UserRepository.Load"))
            {
                using (var dc = DataContextFactory.GetDataContext())
                {
                    var user = dc.Repository<User>()
                        .SingleOrDefault(u => u.Username.ToLower() == username.ToLower());

                    if (user == null)
                        user = dc.Repository<User>()
                            .SingleOrDefault(u => u.PkID == username);

                    return user;
                }
            }
        }

        #endregion

        public IUser Save(IUser user)
        {
            using (MiniProfiler.Current.Step("UserRepository.Save"))
            {
                using (var dc = DataContextFactory.GetDataContext())
                {
                    User modelAsUser = null;
                    if (String.IsNullOrEmpty(user.UniqueKey))
                        modelAsUser = new User();
                    else
                        modelAsUser = dc.Repository<User>()
                            .Where(u => u.PkID == user.UniqueKey)
                            .FirstOrDefault();

                    if (modelAsUser == null)
                        throw new ArgumentException("Attempted to Update nonexisting User");

                    modelAsUser.Username = user.Username;
                    modelAsUser.LastName = user.LastName;
                    modelAsUser.FirstName = user.FirstName;
                    modelAsUser.Birthdate = user.Birthdate;

                    if (String.IsNullOrEmpty(modelAsUser.UniqueKey))
                        dc.Insert(modelAsUser);

                    dc.Commit();

                    return user;
                }
            }
        }

        #region ModelFactory
        public IUser New()
        {
            return new User();
        }
        #endregion
    }
}