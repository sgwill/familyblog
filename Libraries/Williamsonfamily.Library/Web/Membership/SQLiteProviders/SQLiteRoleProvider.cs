using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Web.Security;

namespace WilliamsonFamily.Library.Web.Membership.SQLiteProviders
{
    public sealed class SQLiteRoleProvider : RoleProvider
    {
        #region Variables
        private string rolesTable = "Roles";
        private string usersInRolesTable = "UsersInRoles";
        private string eventSource = "SQLiteRoleProvider";
        private string eventLog = "Application";
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";
        private ConnectionStringSettings _connectionStringSettings;
        private string connectionString;
        #endregion

        #region Properties
        /// <summary>
        /// If false, exceptions are thrown to the caller. If true,
        /// exceptions are written to the event log.
        /// </summary>
        public bool WriteExceptionsToEventLog { get; set; } 
        #endregion

        #region RoleProvider Properties
        public override string ApplicationName { get; set; }
        #endregion

        #region RoleProvider Methods

        #region Initialize
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "SQLiteRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "SQLite Role provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
            {
                ApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                ApplicationName = config["applicationName"];
            }

            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                {
                    WriteExceptionsToEventLog = true;
                }
            }

            // Initialize SQLiteConnection.
            _connectionStringSettings = ConfigurationManager.
              ConnectionStrings[config["connectionStringName"]];

            if (_connectionStringSettings == null || _connectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = _connectionStringSettings.ConnectionString;
        }
        #endregion

        #region AddUsersToRoles
        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                if (username.IndexOf(',') > 0)
                {
                    throw new ArgumentException("User names cannot contain commas.");
                }

                foreach (string rolename in rolenames)
                {
                    if (IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is already in role.");
                    }
                }
            }

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO `" + usersInRolesTable + "`" +
                    " (Username, Rolename, ApplicationName) " +
                    " Values($Username, $Rolename, $ApplicationName)", conn);
           
            SQLiteParameter userParm = cmd.Parameters.Add("$Username", DbType.String, 255);
            SQLiteParameter roleParm = cmd.Parameters.Add("$Rolename", DbType.String, 255);
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SQLiteTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                foreach (string username in usernames)
                {
                    foreach (string rolename in rolenames)
                    {
                        userParm.Value = username;
                        roleParm.Value = rolename;
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (SQLiteException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }

                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "AddUsersToRoles");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region CreateRole
        public override void CreateRole(string rolename)
        {
            if (rolename.IndexOf(',') > 0)
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (RoleExists(rolename))
            {
                throw new ProviderException("Role name already exists.");
            }

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO `" + rolesTable + "`" +
                    " (Rolename, ApplicationName) " +
                    " Values($Rolename, $ApplicationName)", conn);

            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateRole");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region DeleteRole
        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM `" + rolesTable + "`" +
                    " WHERE Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SQLiteCommand cmd2 = new SQLiteCommand("DELETE FROM `" + usersInRolesTable + "`" +
                    " WHERE Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd2.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd2.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SQLiteTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                cmd2.Transaction = tran;

                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();

                tran.Commit();
            }
            catch (SQLiteException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }

                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteRole");
                    return false;
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return true;
        }
        #endregion

        #region GetAllRoles
        public override string[] GetAllRoles()
        {
            string tmpRoleNames = "";

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("SELECT Rolename FROM `" + rolesTable + "`" +
                      " WHERE ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SQLiteDataReader reader = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmpRoleNames += reader.GetString(0) + ",";
                }
            }
            catch (SQLiteException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllRoles");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }
        #endregion

        #region GetRolesForUser
        public override string[] GetRolesForUser(string username)
        {
            string tmpRoleNames = "";

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("SELECT Rolename FROM `" + usersInRolesTable + "`" +
                    " WHERE Username = $Username AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Username", DbType.String, 255).Value = username;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SQLiteDataReader reader = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmpRoleNames += reader.GetString(0) + ",";
                }
            }
            catch (SQLiteException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetRolesForUser");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }
        #endregion

        #region GetUsersInRole
        public override string[] GetUsersInRole(string rolename)
        {
            string tmpUserNames = "";

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("SELECT Username FROM `" + usersInRolesTable + "`" +
                      " WHERE Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SQLiteDataReader reader = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmpUserNames += reader.GetString(0) + ",";
                }
            }
            catch (SQLiteException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUsersInRole");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }
        #endregion

        #region IsUserInRole
        public override bool IsUserInRole(string username, string rolename)
        {
            bool userIsInRole = false;

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM `" + usersInRolesTable + "`" +
                    " WHERE Username = $Username AND Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Username", DbType.String, 255).Value = username;
            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            try
            {
                conn.Open();
                long numRecs = (long)cmd.ExecuteScalar();

                if (numRecs > 0)
                {
                    userIsInRole = true;
                }
            }
            catch (SQLiteException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "IsUserInRole");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return userIsInRole;
        }
        #endregion

        #region RemoveUsersFromRoles
        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                foreach (string rolename in rolenames)
                {
                    if (!IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is not in role.");
                    }
                }
            }

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM `" + usersInRolesTable + "`" +
                    " WHERE Username = $Username AND Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            SQLiteParameter userParm = cmd.Parameters.Add("$Username", DbType.String, 255);
            SQLiteParameter roleParm = cmd.Parameters.Add("$Rolename", DbType.String, 255);
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            SQLiteTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                foreach (string username in usernames)
                {
                    foreach (string rolename in rolenames)
                    {
                        userParm.Value = username;
                        roleParm.Value = rolename;
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (SQLiteException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }

                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RemoveUsersFromRoles");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region RoleExists
        public override bool RoleExists(string rolename)
        {
            bool exists = false;

            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM `" + rolesTable + "`" +
                      " WHERE Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);

            cmd.Parameters.Add("$Rolename", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            try
            {
                conn.Open();
                long numRecs = (long)cmd.ExecuteScalar();

                if (numRecs > 0)
                {
                    exists = true;
                }
            }
            catch (SQLiteException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RoleExists");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return exists;
        }
        #endregion

        #region FindUsersInRole
        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            SQLiteConnection conn = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand("SELECT Username FROM `" + usersInRolesTable + "` " +
                      "WHERE Username LIKE $UsernameSearch AND Rolename = $Rolename AND ApplicationName = $ApplicationName", conn);
            cmd.Parameters.Add("$UsernameSearch", DbType.String, 255).Value = usernameToMatch;
            cmd.Parameters.Add("$RoleName", DbType.String, 255).Value = rolename;
            cmd.Parameters.Add("$ApplicationName", DbType.String, 255).Value = ApplicationName;

            string tmpUserNames = "";
            SQLiteDataReader reader = null;

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tmpUserNames += reader.GetString(0) + ",";
                }
            }
            catch (SQLiteException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersInRole");
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }
        #endregion

        #endregion

        #region WriteToEventLog
        /// <summary>
        /// A helper function that writes exception detail to the event log. Exceptions
        /// are written to the event log as a security measure to avoid private database
        /// details from being returned to the browser. If a method does not return a status
        /// or boolean indicating the action succeeded or failed, a generic exception is also 
        /// thrown by the caller.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="action"></param>
        private void WriteToEventLog(SQLiteException e, string action)
        {
            EventLog log = new EventLog();
            log.Source = eventSource;
            log.Log = eventLog;

            string message = exceptionMessage + "\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e.ToString();

            log.WriteEntry(message);
        }
        #endregion
    }
}