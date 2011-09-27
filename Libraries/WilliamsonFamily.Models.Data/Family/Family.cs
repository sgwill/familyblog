using System;
using WilliamsonFamily.Models.Family;

namespace WilliamsonFamily.Models.Data
{
    public partial class Family : IFamily
    {
        #region IUniqueKey<int> Members

        public int UniqueKey
        {
            get { return PkID; }
        }

        #endregion

    }
}