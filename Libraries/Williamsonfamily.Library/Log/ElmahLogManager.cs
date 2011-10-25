using System;
using WilliamsonFamily.Models.Log;
using System.Data.SQLite;
using MvcMiniProfiler;
using System.Data;

namespace WilliamsonFamily.Library.Log
{
	public class ElmahLogManager : ILogManager
	{
		public string ConnectionString { get; set; }

		private void EnsureInjectables()
		{
			if (string.IsNullOrEmpty(ConnectionString)) throw new ApplicationException("ConnectionString required");
		}

		public void RemoveOldLogs(int daysToKeep)
		{
			using (MiniProfiler.Current.Step("ElmahLogManager.RemoveOldLogs"))
			{
				EnsureInjectables();

				if (daysToKeep < 30)
					throw new ArgumentException("Must specify more than 30 days to delete");

				using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
				{
					SQLiteCommand cmd = new SQLiteCommand("DELETE FROM [Error] WHERE TimeUtc < @Day", conn);
					
					cmd.Parameters.Add(new SQLiteParameter("@Day", DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd")));
					
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

		public int LogsCount()
		{
			using (MiniProfiler.Current.Step("ElmahLogManager.LogsCount"))
			{
				EnsureInjectables();
				int count = 0;

				using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
				{
					SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(ErrorId) FROM [Error]", conn);
					
					try
					{
						conn.Open();
						using (var reader = cmd.ExecuteReader())
						{
							count = Convert.ToInt32(reader[0]);
						}
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

				return count;
			}
		}

		public void Compact()
		{
			EnsureInjectables();

			using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
			{
				SQLiteCommand cmd = new SQLiteCommand("VACUUM", conn);

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