using System;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SQLite;
using Dapper;
using Dapper.Contrib.Extensions;
using WilliamsonFamily.Models.Content;

namespace WilliamsonFamily.Models.Data.Dapper.Content
{
	public class ContentRepository : IContentRepository
	{
		public IContent New()
		{
			return new Content();
		}

		public IContent Lookup(string token)
		{
			  string connString = ConfigurationManager.ConnectionStrings["WilliamsonFamilyConnectionString"].ConnectionString;
                    //var connection = new ProfiledDbConnection(new SQLiteConnection(connString), MiniProfiler.Current);
			  using (var conn = new SQLiteConnection(connString))
			  {
				  conn.Open();
				  var content =  conn.Query<Content>("SELECT * FROM Content WHERE Token = @token", new { @token = token }, null, true, null, null).FirstOrDefault();
				  conn.Close();

				  return content;
			  }
		}

		public void Set(IContent content)
		{
			 string connString = ConfigurationManager.ConnectionStrings["WilliamsonFamilyConnectionString"].ConnectionString;
                    //var connection = new ProfiledDbConnection(new SQLiteConnection(connString), MiniProfiler.Current);
			 using (var conn = new SQLiteConnection(connString))
			 {
				 conn.Open();

				 var contentExists = conn.Query<Content>("SELECT * FROM Content WHERE Token = @token", new { @token = content.Token }, null, true, null, null).FirstOrDefault();
				 if (contentExists == null)
					 conn.Execute("INSERT INTO Content (Token, Value) VALUES (@Token, @Value)", new { @token = content.Token, @value = content.Value }, null, null, null);
				 else
					 conn.Execute(@"
UPDATE Content
SET Token = @Token,
Value = @Value
WHERE ContentID = @ContentID", new { @token = content.Token, @value = content.Value, @ContentID = content.ContentID }, null, null, null);

				 conn.Close();
			 }
		}
	}
}