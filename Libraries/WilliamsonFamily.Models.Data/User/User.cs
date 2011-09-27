using System;
using WilliamsonFamily.Models.User;

namespace WilliamsonFamily.Models.Data
{
    public partial class User : IUser
    {
        #region IUniqueKey<int> Members

        public string UniqueKey
        {
            get { return PkID; }
        }

        #endregion
    }
}