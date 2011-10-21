using System;
using WilliamsonFamily.Models.Log;
using System.Data.SQLite;
using System.Data;

namespace WilliamsonFamily.Library.Log
{
	public class ElmahLogCleaner : ILogCleaner
	{
		public string ConnectionString { get; set; }

		private void EnsureInjectables()
		{
			if (string.IsNullOrEmpty(ConnectionString)) throw new ApplicationException("ConnectionString required");
		}

		public void RemoveOldLogs(int daysToKeep)
		{
			EnsureInjectables();

			if (daysToKeep < 30)
				throw new ArgumentException("Must specify more than 30 days to delete");

			using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
			{
				SQLiteCommand cmd = new SQLiteCommand("DELETE [ELMAH_Error]" +
						"WHERE TimeUtc < DATEADD(d, -$Days, getdate()", conn);

				cmd.Parameters.Add("$Days", DbType.Int32).Value = daysToKeep;

				try
				{
					conn.Open();
					cmd.ExecuteNonQuery();
				}
				catch (SQLiteException e)
				{
					throw e;
				}
				finally
				{
					conn.Close();
				}
			}
		}
	}
}