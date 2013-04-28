using System;

namespace WilliamsonFamily.Models.User
{
    public interface IUserRepository : IModelLoader<IUser, string>, IModelFactory<IUser>, IModelPersister<IUser>
    {
    }
}