using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using DbLinq.Data.Linq;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace WilliamsonFamily.Models.Data.WilliamsonFamily
{
    public class WilliamsonFamilyDataContext : IDataContext
    {
        Main _main = null;
        public Main DataContext
        {
            get
            {
                if (_main == null)
                {
                    string connString = ConfigurationManager.ConnectionStrings["WilliamsonFamilyConnectionString"].ConnectionString;
                    var connection = new ProfiledDbConnection(new SQLiteConnection(connString), MiniProfiler.Current);
                    _main = new Main(connection);
                }
                return _main;
            }
            set { _main = value; }
        }

        public static Dictionary<Type, Type> TableMaps = new Dictionary<Type, Type>();

        #region IDataContext Members

        public IQueryable<T> Repository<T>() where T : class
        {
            return DataContext.GetTable<T>();
        }

        public void Insert<T>(T item) where T : class
        {
            DataContext.GetTable<T>().InsertOnSubmit(item);
        }

        public void Delete<T>(T item) where T : class
        {
            DataContext.GetTable<T>().DeleteOnSubmit(item);
        }

        public void Commit()
        {
            DataContext.SubmitChanges();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            DataContext.Dispose();
        }

        #endregion
    }

}
