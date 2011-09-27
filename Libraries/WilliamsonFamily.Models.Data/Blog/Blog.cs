using System;
using System.Collections.Generic;
using WilliamsonFamily.Models.Blog;

namespace WilliamsonFamily.Models.Data
{
    public partial class Blog : IBlog
    {
        #region IUniqueKey<int> Members

        public int UniqueKey
        {
            get { return PkID; }
        }

        #endregion
    }
}