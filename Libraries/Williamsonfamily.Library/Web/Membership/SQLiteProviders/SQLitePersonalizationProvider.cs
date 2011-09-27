using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data.SQLite;
using System.Web.UI.WebControls.WebParts;

namespace WilliamsonFamily.Library.Web.Membership.SQLiteProviders
{
    public class SQLitePersonalizationProvider : PersonalizationProvider
    {
        #region Variables
        private string connectionString;
        #endregion

        #region Properties
        public string ConnectionStringName { get; set; }
        #endregion

        #region PersonalizationProvider Properties
        public override string ApplicationName { get; set; }
        #endregion

        #region PersonalizationProvider Methods

        #region Initialize
        public override void Initialize(string name, NameValueCollection config)
        {
            // Verify that config isn't null
            if (config == null)
                throw new ArgumentNullException("config");

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name))
                name = "SQLitePersonalizationProvider";

            // Add a default "description" attribute to config if the
            // attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description",
                    "Simple SQLite personalization provider");
            }

            // Call the base class's Initialize method
            base.Initialize(name, config);

            if (string.IsNullOrEmpty(config["connectionStringName"]))
            {
                throw new ProviderException
                    ("ConnectionStringName property has not been specified");
            }
            else
            {
                ConnectionStringName = config["connectionStringName"];
                config.Remove("connectionStringName");
            }

            if (string.IsNullOrEmpty(config["applicationName"]))
            {
                throw new ProviderException
                    ("applicationName property has not been specified");
            }
            else
            {
                ApplicationName = config["applicationName"];
                config.Remove("applicationName");
            }

            // Initialize SQLiteConnection.
            ConnectionStringSettings ConnectionStringSettings =
              ConfigurationManager.ConnectionStrings[ConnectionStringName];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = ConnectionStringSettings.ConnectionString;

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException
                        ("Unrecognized attribute: " + attr);
            }
        }
        #endregion

        #region LoadPersonalizationBlobs
        protected override void LoadPersonalizationBlobs (WebPartManager webPartManager, string path, string userName,
            ref byte[] sharedDataBlob, ref byte[] userDataBlob)
        {
            // Load shared state
            sharedDataBlob = null;
            userDataBlob = null;
            object sharedBlobDataObject = null;
            object userBlobDataObject = null;
            string sSQLShared = null;
            string sSQLUser = null;
            SQLiteConnection conn = new SQLiteConnection(connectionString);

            try
            {
                conn.Open();

                sSQLUser = "SELECT `personalizationblob` FROM `personalization`" + Environment.NewLine +
                    "WHERE `username` = '" + userName + "' AND " + Environment.NewLine +
                    "`path` = '" + path + "' AND " + Environment.NewLine +
                    "`applicationname` = '" + ApplicationName + "';";
                
                sSQLShared = "SELECT `personalizationblob` FROM `personalization`" + Environment.NewLine +
                    "WHERE `username` IS NULL AND " + Environment.NewLine +
                    "`path` = '" + path + "' AND " + Environment.NewLine +
                    "`applicationname` = '" + ApplicationName + "';";
                
                SQLiteCommand cmd = new SQLiteCommand(sSQLUser, conn);
                sharedBlobDataObject = cmd.ExecuteScalar();
                cmd.CommandText = sSQLShared;
                userBlobDataObject = cmd.ExecuteScalar();

                if (sharedBlobDataObject != null)
                    sharedDataBlob = (byte[])sharedBlobDataObject;
                
                if (userBlobDataObject != null)
                    userDataBlob = (byte[])userBlobDataObject;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region ResetPersonalizationBlog
        protected override void ResetPersonalizationBlob (WebPartManager webPartManager, string path, string userName)
        {
            // Delete the specified personalization file
            string sSQL = null;
            SQLiteConnection conn = new SQLiteConnection(connectionString);

            try
            {
                sSQL = "DELETE FROM `personalization` WHERE `username` = '" + userName + "' AND `path` = '" + path + "' AND `applicationname` = '" + ApplicationName + "';";
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sSQL, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception) { throw; }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region SavePersonalizationBlog
        protected override void SavePersonalizationBlob (WebPartManager webPartManager, string path, string userName,
            byte[] dataBlob)
        {
            string sSQL = null;
            SQLiteConnection conn = new SQLiteConnection(connectionString);

            try
            {
                conn.Open();
                sSQL = "SELECT COUNT(`username`) FROM `personalization` WHERE `username` = '" + userName + "' AND `path` = '" + path + "' and `applicationname` = '" + ApplicationName + "';";
                SQLiteCommand cmd = new SQLiteCommand(sSQL, conn);
                if (int.Parse(cmd.ExecuteScalar().ToString()) > 0)
                {
                    sSQL = @"UPDATE `personalization` SET `personalizationblob` = $personalizationblob
                    WHERE `username` = $username AND 
                    `applicationname` = $applicationname AND 
                    `path` = $path;";
                    cmd = new SQLiteCommand(sSQL, conn);
                    cmd.Parameters.AddWithValue("$personalizationblob", dataBlob);
                    cmd.Parameters.AddWithValue("$username", userName);
                    cmd.Parameters.AddWithValue("$applicationname", ApplicationName);
                    cmd.Parameters.AddWithValue("$path", path);
                }
                else
                {
                    sSQL = @"INSERT INTO `personalization` (`username`,`path`,`applicationname`,`personalizationblob`) 
                            VALUES ($username,$path,$applicationname,$personalizationblob);";
                    cmd = new SQLiteCommand(sSQL, conn);
                    cmd.Parameters.AddWithValue("$username", userName);
                    cmd.Parameters.AddWithValue("$path", path);
                    cmd.Parameters.AddWithValue("$applicationname", ApplicationName);
                    cmd.Parameters.AddWithValue("$personalizationblob", dataBlob);
                }

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region FindState
        public override PersonalizationStateInfoCollection FindState(PersonalizationScope scope, PersonalizationStateQuery query,
            int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region GetCountOfState
        public override int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region ResetState
        public override int ResetState(PersonalizationScope scope, string[] paths, string[] usernames)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region ResetUserState
        public override int ResetUserState(string path, DateTime userInactiveSinceDate)
        {
            throw new NotSupportedException();
        }
        #endregion

#endregion
    }
}
