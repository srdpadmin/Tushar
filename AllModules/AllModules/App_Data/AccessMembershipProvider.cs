using System.Security;
using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration.Provider;
using CoreAssemblies;
using System.Data.Common;
using ENM = CoreAssemblies.EnumClass;
using System.IO;
using ASM = CoreAssemblies;

//using IBM.Data.DB2
//using IBM.Data.DB2Types
//using ARCCore = ARC.Core
//using WC = WebCommon
namespace AllModules
{
    public class AccessMembershipProvider : MembershipProvider
    {
        // [Password] Should be used in query
        // IsApproved during insert/update changed to Convert.ToInt32
        // This provider works with the following schema for the table of user data.
        // 
        // CREATE TABLE UserLogin
        // (
        //   PKID INT NOT NULL PRIMARY KEY,
        //   Username Varchar (255) NOT NULL,
        //   ApplicationName Varchar (255) NOT NULL,
        //   Email Varchar (128) NOT NULL,
        //   Comment Varchar (255),
        //   Password Varchar (128) NOT NULL,
        //   PasswordQuestion Varchar (255),
        //   PasswordAnswer Varchar (255),
        //   IsApproved Varchar(1), 
        //   LastActivityDate TimeStamp,
        //   LastLoginDate TimeStamp,
        //   LastPasswordChangedDate TimeStamp,
        //   CreationDate TimeStamp, 
        //   IsOnLine Varchar(1),
        //   IsLockedOut Varchar(1),
        //   LastLockedOutDate TimeStamp,
        //   FailedPasswordAttemptCount Integer,
        //   FailedPasswordAttemptWindowStart TimeStamp,
        //   FailedPasswordAnswerAttemptCount Integer,
        //   FailedPasswordAnswerAttemptWindowStart TimeStamp
        // )

        #region Private Members

        DbDataReader reader = null;
        DbFactory factory = null;

        private int newPasswordLength = 8;
        private string eventSource = "AccessMembershipProvider";
        private string eventLog = "Application";
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";
        private string connectionString;
        //
        // Used when determining encryption key values.
        //
        private MachineKeySection machineKey;
        //
        // If False, exceptions are thrown to the caller. If True,
        // exceptions are written to the event log.
        //
        private bool pWriteExceptionsToEventLog;
        //
        // System.Web.Security.MembershipProvider properties.
        //
        private string pApplicationName;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private int pMinRequiredNonAlphanumericCharacters;
        private int pMinRequiredPasswordLength;
        private string pPasswordStrengthRegularExpression;

        private MembershipPasswordFormat pPasswordFormat;
        private string ppassPhrase;
        private string psaltValue;
        private string phashAlgorithm;
        private string phashKey;
        private int ppasswordIterations;
        private string pinitVector;
        private int pkeySize;
        #endregion

        #region Properties

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

        public string PassPhrase
        {
            get { return ppassPhrase; }
            set { ppassPhrase = value; }
        }

        public string SaltValue
        {
            get { return psaltValue; }
            set { psaltValue = value; }
        }

        public string HashAlgorithm
        {
            get { return phashAlgorithm; }
            set { phashAlgorithm = value; }
        }

        public string HashKey
        {
            get { return phashKey; }
            set { phashKey = value; }
        }

        public string InitVector
        {
            get { return pinitVector; }
            set { pinitVector = value; }
        }

        public int PasswordIterations
        {
            get { return ppasswordIterations; }
            set { ppasswordIterations = value; }
        }

        public int KeySize
        {
            get { return pkeySize; }
            set { pkeySize = value; }
        }


        #endregion

        #region Functions

        #region Initilize

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            //
            // Initialize values from web.config.
            //
            if (config == null)
            {
                throw (new ArgumentNullException("config"));
            }
            if (name == null || name.Length == 0)
            {
                name = "AccessMembershipProvider";
            }
            if (string.IsNullOrEmpty(System.Convert.ToString(config["description"])))
            {
                config.Remove("description");
                config.Add("description", "Access Membership provider");
            }
            // Initialize the abstract base class.
            base.Initialize(name, config);
            pApplicationName = GetConfigValue(System.Convert.ToString(config["applicationName"]), (string)System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(System.Convert.ToString(config["maxInvalidPasswordAttempts"]), "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(System.Convert.ToString(config["passwordAttemptWindow"]), "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(System.Convert.ToString(config["minRequiredAlphaNumericCharacters"]), "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(System.Convert.ToString(config["minRequiredPasswordLength"]), "7"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(System.Convert.ToString(config["passwordStrengthRegularExpression"]), ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(System.Convert.ToString(config["enablePasswordReset"]), "True"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(System.Convert.ToString(config["enablePasswordRetrieval"]), "True"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(System.Convert.ToString(config["requiresQuestionAndAnswer"]), "False"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(System.Convert.ToString(config["requiresUniqueEmail"]), "True"));
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(System.Convert.ToString(config["writeExceptionsToEventLog"]), "True"));
            ppassPhrase = GetConfigValue(System.Convert.ToString(config["passPhrase"]), "ARCollect");
            psaltValue = GetConfigValue(System.Convert.ToString(config["saltValue"]), "ARCollect");
            phashAlgorithm = GetConfigValue(System.Convert.ToString(config["hashAlgorithm"]), "SHA1");
            phashKey = GetConfigValue(System.Convert.ToString(config["hashKey"]), "ABCDE");
            pinitVector = GetConfigValue(System.Convert.ToString(config["initVector"]), "@1B2c3D4e5F6g7H8");
            pkeySize = Convert.ToInt32(GetConfigValue(System.Convert.ToString(config["keySize"]), "256"));
            ppasswordIterations = Convert.ToInt32(GetConfigValue(System.Convert.ToString(config["passwordIterations"]), "2"));
            string temp_format = System.Convert.ToString(config["passwordFormat"]);
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }
            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw (new ProviderException("Password format not supported."));
                    break;
            }
            //
            // Initialize DB2Connection.
            //
            ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];
            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw (new ProviderException("Connection string cannot be blank."));
            }
            connectionString = ConnectionStringSettings.ConnectionString;
            
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["Encrypted"].ToString()))
                {
                    EncodeDecode ed = new EncodeDecode();
                    connectionString = ed.GetProperConnectionString(connectionString);
                }
                string connectionStringName = "AccessDbProvider";
                //ASM.DbFactory.PayrollConnectionStringName = connectionString;
                //ASM.DbFactory.PayrollProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
                //connectionStringName = "AccessDbProvider";
                //ASM.DbFactory.QuotationConnectionStringName = connectionString;
                //ASM.DbFactory.QuotationProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
                //connectionStringName = "AccessDbProvider";
                ASM.DbFactory.AccessConnectionStringName = connectionString;
                ASM.DbFactory.AccessProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException();
            }
             
        }

        #endregion

        #region GetConfigValue
        //
        // A helper function to retrieve config values from the configuration file.
        //
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
            {
                return defaultValue;
            }
            return configValue;
        }
        #endregion

        #region ChangePassword
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
            {
                return false;
            }
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                {
                    throw (args.FailureInformation);
                }
                else
                {
                    throw (new ProviderException("Change password canceled due to New password validation failure."));
                }
            }
            int rowsAffected = 0;

            try
            {
                //aspnet_UpdateUserLogin
                string sql = " UPDATE UserLogin SET [Password] = '" + EncodePassword(newPassword) + "'";
                sql += ", LastPasswordChangedDate = #" + DateTime.Now.ToString() + "#";
                sql += " WHERE Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";
                //Execute the built query
                factory = new DbFactory(ENM.ModuleName.ACL);
                rowsAffected = factory.RunCommand(sql);

            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePassword");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }


        #endregion

        #region ChangePasswordQuestionAndAnswer
        //
        // MembershipProvider.ChangePasswordQuestionAndAnswer
        //
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            if (!ValidateUser(username, password))
            {
                return false;
            }
            int rowsAffected = 0;
            try
            {
                string sql = "UPDATE 	UserLogin  SET 	PasswordQuestion = '" + newPasswordQuestion + "', PasswordAnswer ='" + newPasswordAnswer + "'";
                sql += " WHERE 	Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";

                //aspnet_UpdateUserQAndA
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        rowsAffected = int.Parse(reader[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(ex, "ChangePasswordQuestionAndAnswer");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (ex);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region CreateUser
        //
        // MembershipProvider.CreateUser
        //
        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.ProviderError;
            string sql = string.Empty;
            ValidatePasswordEventArgs Args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(Args);
            if (Args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
            MembershipUser u = GetUser(username, false);
            int rowsAffected = 0;
            if (u == null)
            {
                //ASPNET_GETMAXPKID
                rowsAffected = 1;
                sql = "Select (MAX(UserID) + 1) as Maximum from Users";

                try
                {
                    //Execute the built query
                    factory = new DbFactory(ENM.ModuleName.ACL);
                    reader = factory.GetDataReader(sql);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                        {
                            rowsAffected = int.Parse(reader[0].ToString());
                        }
                    }

                }
                catch (Exception)
                {
                }
                finally
                {
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                        reader = null;
                    }
                }
                try
                {
                    //aspnet_CreateUser
                    //sql = "Select (MAX(PKID) + 1) as Maximum from UserLogin where ApplicationName ='" + ApplicationName + "'";
                    //Execute the built query
                    passwordQuestion = passwordQuestion.Replace("'", "''");

                    sql = "INSERT INTO UserLogin (";
                    sql += "PKID,Username,Password,Email,PasswordQuestion,PasswordAnswer,IsApproved,";
                    sql += "Comment,CreationDate,LastPasswordChangedDate,LastActivityDate,ApplicationName, ";
                    sql += "IsLockedOut,LastLockedOutDate,FailedPasswordAttemptCount,FailedPasswordAttemptWindowStart, ";
                    sql += "FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart) ";
                    sql += " VALUES ( ";
                    sql += " " + rowsAffected + ",'" + username + "','" + EncodePassword(password) + "','" + email;
                    sql += "','" + passwordQuestion + "','" + EncodePassword(passwordAnswer) + "','";
                    sql += Convert.ToInt32(isApproved) + "','" + string.Empty + "',Now(),Now(),Now(),'" + pApplicationName + "','0',Now(),0,Now(),0,Now())";


                    string[] names = new string[18];
                    names[0] = "PKID";
                    names[1] = "Username";
                    names[2] = "Password";
                    names[3] = "@Email";
                    names[4] = "@PasswordQuestion";
                    names[5] = "@PasswordAnswer";
                    names[6] = "@IsApproved";
                    names[7] = "@Comment";
                    names[8] = "@CreationDate";
                    names[9] = "@LastPasswordChangedDate";
                    names[10] = "@LastActivityDate";
                    names[11] = "@ApplicationName";
                    names[12] = "@IsLockedOut";
                    names[13] = "@LastLockedOutDate";
                    names[14] = "@FailedPasswordAttemptCount";
                    names[15] = "@FailedPasswordAttemptWindowStart";
                    names[16] = "@FailedPasswordAnswerAttemptCount";
                    names[17] = "@FailedPasswordAnswerAttemptWindowStart";
                    Object[] values = new Object[18];
                    values[0] = rowsAffected;
                    values[1] = username;
                    values[2] = EncodePassword(password);
                    values[3] = email;
                    values[4] = passwordQuestion;
                    values[5] = EncodePassword(passwordAnswer);
                    values[6] = Convert.ToInt32(isApproved).ToString();
                    values[7] = string.Empty;
                    values[8] = DateTime.Now.ToString("dd/MM/yyyy");
                    values[9] = DateTime.Now.ToString("dd/MM/yyyy");
                    values[10] = DateTime.Now.ToString("dd/MM/yyyy");
                    values[11] = pApplicationName;
                    values[12] = "0";
                    values[13] = DateTime.Now.ToString("dd/MM/yyyy");
                    values[14] = 0;
                    values[15] = DateTime.Now.ToString("dd/MM/yyyy");
                    values[16] = 0;
                    values[17] = DateTime.Now.ToString("dd/MM/yyyy");
                    sql = "INSERT INTO [UserLogin] (";
                    sql += "PKID,Username,[Password],Email,PasswordQuestion,PasswordAnswer,IsApproved,Comment,CreationDate,";
                    sql += "LastPasswordChangedDate,LastActivityDate,ApplicationName, ";
                    sql += "IsLockedOut,LastLockedOutDate,FailedPasswordAttemptCount,FailedPasswordAttemptWindowStart, ";
                    sql += "FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart) ";
                    sql += " VALUES ";
                    sql += "(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";//;,?,?)";//(@PKID,@Username,@Password);"; //,@Email,@PasswordQuestion,@PasswordAnswer,@IsApproved,@Comment)"; //,?,?,?,?,?,?,?,?,?,?)";    
                    //Rules:  Password is reserved word should be in square brackets
                    //Rules: In Parameters string goes like string, no need to add single quotes
                    //Rules: In Parameters date goes like string, no need to add single quotes or hash
                    factory = new DbFactory(ENM.ModuleName.ACL);
                    rowsAffected = factory.RunCommandWithParametersList(sql, names, values);
                    //reader = factory.GetDataReader(sql);
                    //if (reader.HasRows)
                    //{
                    //    reader.Read();
                    //    if (!reader.IsDBNull(0))
                    //    {
                    //        rowsAffected = int.Parse(reader[0].ToString());
                    //    }
                    //}
                    if (rowsAffected > 0)
                    {
                        status = MembershipCreateStatus.Success;
                    }
                    else
                    {
                        status = MembershipCreateStatus.UserRejected;
                    }
                }
                catch (System.Data.OleDb.OleDbException exc)
                {
                    throw exc;
                }
                catch (Exception ex)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(ex, "CreateUser");
                    }
                    status = MembershipCreateStatus.ProviderError;
                }
                finally
                {
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                        {
                            reader.Close();
                        }
                        reader = null;
                    }
                    if (factory != null)
                    {
                        factory.Close();
                        factory = null;
                    }
                }
                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            return null;
        }

        #endregion

        #region DeleteUser
        //
        // MembershipProvider.DeleteUser
        //
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            int rowsAffected = 0;
            if (username == string.Empty)
            {
                return false;
            }
            try
            {
                //aspnet_DeleteUser

                string sql = "DELETE FROM UserLogin";
                sql += " WHERE Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";

                //aspnet_UpdateUserQAndA
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        rowsAffected = int.Parse(reader[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(ex, "DeleteUser");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (ex);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region FindusersByEmail

        //
        // MembershipProvider.FindUsersByEmail
        //

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            try
            {
                //aspnet_GetUserByEmailCount
                string sql = "SELECT Count(*) AS COUNT FROM UserLogin WHERE Email LIKE '" + emailToMatch + "'";
                sql += " AND ApplicationName ='" + ApplicationName + "'";

                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        totalRecords = int.Parse(reader[0].ToString());
                    }
                }

                if (totalRecords <= 0)
                {
                    return users;
                }
                factory.Close();
                //aspnet_GetUserByEmail
                sql = " SELECT PKID, Username, Email, PasswordQuestion, ";
                sql += " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate, ";
                sql += " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate ";
                sql += " FROM UserLogin ";
                sql += " WHERE Email LIKE LIKE '" + emailToMatch + "'";
                sql += " AND ApplicationName ='" + ApplicationName + "'";
                sql += " ORDER BY Username Asc";

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }
                    if (counter >= endIndex)
                    {
                        break;
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(ex, "FindUsersByEmail");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (ex);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            return users;
        }

        #endregion

        #region FindUsersByName
        //
        // MembershipProvider.FindUsersByName
        //
        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            try
            {
                //aspnet_GetUserByNameCount
                string sql = "SELECT Count(*) AS COUNT FROM UserLogin WHERE  Username LIKE '" + usernameToMatch + "'";
                sql += " AND ApplicationName ='" + ApplicationName + "'";

                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        totalRecords = int.Parse(reader[0].ToString());
                    }
                }

                if (totalRecords <= 0)
                {
                    return users;
                }
                factory.Close();
                //aspnet_GetUserByName
                sql = " SELECT PKID, Username, Email, PasswordQuestion, ";
                sql += " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate, ";
                sql += " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate ";
                sql += " FROM UserLogin ";
                sql += " WHERE Username LIKE  '" + usernameToMatch + "'";
                sql += " AND ApplicationName ='" + ApplicationName + "'";
                sql += " ORDER BY Username Asc";

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }
                    if (counter >= endIndex)
                    {
                        break;
                    }
                    counter++;
                }

            }
            catch (Exception ex)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(ex, "FindUsersByName");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (ex);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            return users;
        }


        #endregion

        #region GetUserFromReader

        //
        // GetUserFromReader
        //    A helper function that takes the current row from the DB2DataReader
        // and hydrates a MembershiUser from the values. Called by the
        // MembershipUser.GetUser implementation.
        //

        private MembershipUser GetUserFromReader(DbDataReader reader)
        {
            object providerUserKey = reader.GetValue(0);
            string username = (string)(reader.GetString(1));
            string email = (string)(reader.GetString(2));
            string passwordQuestion = "";
            if (!(reader.GetValue(3) == DBNull.Value))
            {
                passwordQuestion = (string)(reader.GetString(3));
            }
            string comment = "";
            if (!(reader.GetValue(4) == DBNull.Value))
            {
                comment = (string)(reader.GetString(4));
            }
            bool isApproved = false;
            if (reader.GetString(5) == "1")
            {
                isApproved = true;
            }
            bool isLockedOut = false;
            if (reader.GetString(6) == "1")
            {
                isLockedOut = true;
            }
            DateTime creationDate = System.Convert.ToDateTime(reader["creationDate"]);
            DateTime lastLoginDate = new DateTime();
            if (!(reader.GetValue(8) == DBNull.Value))
            {
                lastLoginDate = System.Convert.ToDateTime(reader["lastLoginDate"]);
            }
            DateTime lastActivityDate = System.Convert.ToDateTime(reader["LastActivityDate"]);
            DateTime lastPasswordChangedDate = System.Convert.ToDateTime(reader["LastPasswordChangedDate"]);
            DateTime lastLockedOutDate = new DateTime();
            if (!(reader["LastLockedOutDate"] == DBNull.Value))
            {
                lastLockedOutDate = System.Convert.ToDateTime(reader["LastLockedOutDate"]);
            }
            MembershipUser u = new MembershipUser(this.Name, username, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockedOutDate);
            return u;
        }

        #endregion

        #region GetAllUsers
        //
        // MembershipProvider.GetAllUsers
        //
        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            try
            {
                //aspnet_GetAllUsersCount           

                string sql = "SELECT Count(*) AS COUNTKEY FROM UserLogin ";
                sql += " WHERE ApplicationName ='" + ApplicationName + "'";


                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        totalRecords = int.Parse(reader[0].ToString());
                    }
                }

                if (totalRecords <= 0)
                {
                    return users;
                }
                factory.Close();

                //ASPNET_GETALLUSERS
                sql = " SELECT PKID, Username, Email, PasswordQuestion, ";
                sql += " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate, ";
                sql += " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate ";
                sql += " FROM UserLogin ";
                sql += " WHERE ApplicationName ='" + ApplicationName + "'";
                sql += " ORDER BY Username Asc";

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }
                    if (counter >= endIndex)
                    {
                        break;
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(ex, "GetAllUsers");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (ex);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            return users;
        }


        #endregion

        #region GetNumberOfUsersOnline
        //
        // MembershipProvider.GetNumberOfUsersOnline
        //
        public override int GetNumberOfUsersOnline()
        {
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);
            int numOnline = 0;

            try
            {
                //aspnet_GetNumberOfUsersOnline

                string sql = "SELECT Count(*) AS COUNT FROM UserLogin WHERE LastActivityDate > #" + compareTime + "#";
                sql += " WHERE 	 ApplicationName ='" + ApplicationName + "'";

                //aspnet_UpdateUserQAndA
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        numOnline = int.Parse(reader[0].ToString());
                    }
                }
            }
            catch (DbException ex)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(ex, "GetNumberOfUsersOnline");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (ex);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            return numOnline;
        }


        #endregion

        #region GetPassword
        //
        // MembershipProvider.GetPassword
        //
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw (new ProviderException("Password Retrieval Not Enabled."));
            }
            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw (new ProviderException("Cannot retrieve Hashed passwords."));
            }
            if (username == string.Empty)
            {
                throw (new ProviderException("No UserName provided."));
            }
            string password = string.Empty;
            string passwordAnswer = string.Empty;

            try
            {
                //aspnet_GetPassword
                string sql = "SELECT Password, PasswordAnswer, IsLockedOut FROM UserLogin ";
                sql += " WHERE 	Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";

                //aspnet_UpdateUserQAndA
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader.GetString(2) == "1")
                    {
                        throw (new MembershipPasswordException("The supplied user is locked out."));

                    }
                    password = (string)(reader.GetString(0));
                    passwordAnswer = (string)(reader.GetString(1));
                }
                else
                {
                    throw (new MembershipPasswordException("The supplied user name is not found."));
                }
            }
            catch (DbException ex)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(ex, "GetPassword");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (ex);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw (new MembershipPasswordException("Incorrect password answer."));
            }
            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }
            return password;
        }

        #endregion

        #region GetUser
        //
        // MembershipProvider.GetUser(Object, Boolean)
        //
        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            MembershipUser u = null;
            if (System.Convert.ToInt32(providerUserKey) < 1)
            {
                return u;
            }

            try
            {
                //aspnet_GetUser           
                string sql = string.Empty;

                sql = " SELECT PKID, Username, Email, PasswordQuestion, ";
                sql += " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate, ";
                sql += " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate ";
                sql += " FROM UserLogin ";
                if (System.Convert.ToInt32(providerUserKey) > 0)
                {
                    sql += " WHERE PKID = " + System.Convert.ToInt32(providerUserKey);
                    sql += " AND ApplicationName ='" + ApplicationName + "'";
                }
                else
                {
                    sql += " WHERE Username = '' AND ApplicationName ='" + ApplicationName + "'";
                }
                //Actual -no username
                //"aspnet_GetUser", System.Convert.ToInt32(providerUserKey), "", ApplicationName);
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);
                    if (userIsOnline)
                    {
                        //"aspnet_UpdateUserLastActivity", providerUserKey, ApplicationName);
                        factory.Close();
                        sql = " UPDATE 	UserLogin SET LastActivityDate #" + RoundMinutes(DateTime.Now) + "#";
                        sql += " WHERE PKID = " + System.Convert.ToInt32(providerUserKey);
                        sql += " AND ApplicationName ='" + ApplicationName + "'";
                        factory = new DbFactory(ENM.ModuleName.ACL);
                        factory.RunCommand(sql);
                    }
                }
            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(Object, Boolean)");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            return u;
        }
        #endregion

        #region GetUser
        //
        // MembershipProvider.GetUser(String, Boolean)
        //
        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser u = null;
            if (username == "")
            {
                return u;
            }

            try
            {
                //aspnet_GetUser           
                string sql = string.Empty;

                sql = " SELECT PKID, Username, Email, PasswordQuestion, ";
                sql += " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate, ";
                sql += " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate ";
                sql += " FROM UserLogin ";
                sql += " WHERE Username = '" + username + "' AND ApplicationName ='" + ApplicationName + "'";

                //Actual -no username
                //"aspnet_GetUser", System.Convert.ToInt32(providerUserKey), "", ApplicationName);
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);
                    if (userIsOnline)
                    {
                        //"aspnet_UpdateUserLastActivity", providerUserKey, ApplicationName);
                        factory.Close();
                        sql = " UPDATE 	UserLogin SET LastActivityDate = #" + RoundMinutes(DateTime.Now) + "#";
                        sql += " WHERE PKID = " + System.Convert.ToInt32(u.ProviderUserKey);
                        sql += " AND ApplicationName ='" + ApplicationName + "'";
                        factory = new DbFactory(ENM.ModuleName.ACL);
                        factory.RunCommand(sql);
                    }
                }

            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(String, Boolean)");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            return u;
        }

        #endregion

        #region GetUserNameByEmail
        //
        // MembershipProvider.GetUserNameByEmail
        //
        public override string GetUserNameByEmail(string email)
        {
            string username = "";
            if (email == "")
            {
                return username;
            }

            try
            {
                string sql = "SELECT Username FROM UserLogin WHERE Email = '" + email + "'";
                sql += " AND ApplicationName ='" + ApplicationName + "'";
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        username = reader[0].ToString();
                    }
                }
            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserNameByEmail");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            if (username == null)
            {
                username = "";
            }
            return username;
        }

        #endregion

        #region ResetPassword
        //
        // MembershipProvider.ResetPassword
        //
        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw (new NotSupportedException("Password Reset is not enabled."));
            }
            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw (new ProviderException("Password answer required for password Reset."));
            }
            if (username == "")
            {
                throw (new ProviderException("Username not provided."));
            }
            string newPassword = (string)(System.Web.Security.Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters));
            ValidatePasswordEventArgs Args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(Args);
            if (Args.Cancel)
            {
                if (Args.FailureInformation != null)
                {
                    throw (Args.FailureInformation);
                }
                else
                {
                    throw (new MembershipPasswordException("Reset password canceled due to password validation failure."));
                }
            }
            int rowsAffected = 0;
            string passwordAnswer = "";

            try
            {
                //aspnet_GetUserAnswer

                string sql = "SELECT PasswordAnswer, IsLockedOut     FROM UserLogin ";
                sql += " WHERE 	Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";

                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader.GetString(1) == "1")
                    {
                        throw (new MembershipPasswordException("The supplied user is locked out."));
                    }
                    passwordAnswer = (string)(reader.GetString(0));
                    factory.Close();
                }
                else
                {
                    throw (new MembershipPasswordException("The supplied user name is not found."));
                }
                if (answer != "Change")
                {
                    if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                    {
                        UpdateFailureCount(username, "passwordAnswer");
                        throw (new MembershipPasswordException("Incorrect password answer."));
                    }
                }

                //aspnet_ResetPassword

                sql = "select @@Rowcount ";
                sql = "UPDATE UserLogin";
                sql += " SET [Password] ='" + EncodePassword(newPassword) + "',";
                sql += " LastPasswordChangedDate = #" + RoundMinutes(DateTime.Now) + "#,FailedPasswordAttemptCount = 0 ";
                sql += " WHERE Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";
                sql += " AND IsLockedOut = '0'";
                factory = new DbFactory(ENM.ModuleName.ACL);

                rowsAffected = factory.RunCommand(sql);
                factory.Close();

            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ResetPassword");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            if (rowsAffected > 0)
            {
                return newPassword;
            }
            else
            {
                throw (new MembershipPasswordException("User not found, or user is locked out. Password not Reset."));
            }
        }
        #endregion

        #region UnlockUser
        //
        // MembershipProvider.UnlockUser
        //
        public override bool UnlockUser(string userName)
        {
            int rowsAffected = 0;
            if (userName == "")
            {
                return false;
            }

            //aspnet_UnlockUser
            try
            {
                string sql = "Update UserLogin SET 	IsLockedOut = '0',LastLockedOutDate = #" + RoundMinutes(DateTime.Now) + "#";
                sql += " Where Userame='" + userName + "' AND ";
                sql += " ApplicationName='" + ApplicationName + "'";
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                    {
                        rowsAffected = int.Parse(reader[0].ToString());
                    }
                }
            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UnlockUser");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region UpdateUser
        //
        // MembershipProvider.UpdateUser
        //
        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            //aspnet_UpdateUser
            try
            {
                string sql = "Update UserLogin SET email='" + user.Email + "',Comment='" + user.Comment + "'";
                sql += "IsApproved='" + Convert.ToInt32(user.IsApproved) + "',Userame='" + user.UserName + "'";
                sql += "ApplicationName='" + ApplicationName + "'";
                factory = new DbFactory(ENM.ModuleName.ACL);
                factory.RunCommand(sql);
            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateUser");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
        }

        #endregion

        #region UpdateFailureCount

        //
        // UpdateFailureCount
        //   A helper method that performs the checks and updates associated with
        // password failure tracking.
        //
        // TODO NOW() function
        private void UpdateFailureCount(string username, string failureType)
        {
            int rowsAffected = 0;
            DateTime windowStart = new DateTime();
            DateTime CurrentDate = new DateTime();
            int failureCount = 0;
            if (username == "")
            {
                throw (new ProviderException("Username not provided."));
            }

            try
            {
                //aspnet_UserFailedLogins
                factory = new DbFactory(ENM.ModuleName.ACL);
                //Execute the built query
                string sql = "SELECT FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart,";
                sql += "FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart";
                //MSSQL.ROUND_TO_MINUTES(MSSQL.GETUTCDATE())  
                sql += " FROM UserLogin ";
                sql += " WHERE Username = '" + username + "' AND ApplicationName = '" + ApplicationName + "'";
                reader = factory.GetDataReader(sql);

                if (reader.HasRows)
                {
                    reader.Read();
                    if (failureType == "password")
                    {
                        if (reader.IsDBNull(0))
                        {
                            failureCount = 0;
                            windowStart = DateTime.Now;
                            CurrentDate = DateTime.Now;
                        }
                        else
                        {
                            failureCount = System.Convert.ToInt32(reader.GetInt32(0));
                            windowStart = System.Convert.ToDateTime(reader.GetDateTime(1));
                            CurrentDate = RoundMinutes(DateTime.Now);//TODO System.Convert.ToDateTime(reader.GetDateTime(4));
                        }
                    }
                    if (failureType == "passwordAnswer")
                    {
                        if (reader.IsDBNull(2))
                        {
                            failureCount = 0;
                            windowStart = DateTime.Now;
                            CurrentDate = DateTime.Now;
                        }
                        else
                        {
                            failureCount = System.Convert.ToInt32(reader.GetInt32(2));
                            windowStart = System.Convert.ToDateTime(reader.GetDateTime(3));
                            CurrentDate = RoundMinutes(DateTime.Now);//TODO System.Convert.ToDateTime(reader.GetDateTime(4));
                        }
                    }
                }
                DateTime windowEnd = windowStart.AddMinutes(System.Convert.ToDouble(PasswordAttemptWindow));
                if (failureCount == 0 || CurrentDate > windowEnd)
                {
                    //aspnet_UpdateFailedAttempt
                    // First password failure or outside of PasswordAttemptWindow.
                    // Start a New password failure count from 1 and a New window starting now.

                    if (failureType == "password")
                    {
                        sql = "UPDATE UserLogin SET FailedPasswordAttemptCount = " + failureCount + ",";
                        sql += " FailedPasswordAttemptWindowStart =#" + RoundMinutes(DateTime.Now) + "#";
                        sql += " WHERE 	Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";
                    }
                    else
                    {
                        sql += "UPDATE 	UserLogin SET FailedPasswordAnswerAttemptCount = " + failureCount + ",";
                        sql += " FailedPasswordAnswerAttemptWindowStart =#" + RoundMinutes(DateTime.Now) + "#";
                        sql += " WHERE 	Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";
                    }
                    factory.Close();
                    reader = factory.GetDataReader(sql);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (!reader.IsDBNull(0))
                        {
                            rowsAffected = int.Parse(reader[0].ToString());
                        }
                    }
                    if (rowsAffected < 0)
                    {
                        throw (new ProviderException("Unable to update failure count and window start."));
                    }
                }
                else
                {
                    //aspnet_UpdateUserLockOut
                    failureCount++;
                    if (failureCount >= MaxInvalidPasswordAttempts)
                    {
                        sql = "UPDATE 	UserLogin SET 	IsLockedOut = '1', LastLockedOutDate #" + RoundMinutes(DateTime.Now) + "#";
                        sql += " WHERE 	Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";

                        reader = factory.GetDataReader(sql);
                        if (reader.HasRows)
                        {
                            reader.Read();
                            if (!reader.IsDBNull(0))
                            {
                                rowsAffected = int.Parse(reader[0].ToString());
                            }
                        }
                        if (rowsAffected < 0)
                        {
                            throw (new ProviderException("Unable to lock out user."));
                        }
                    }
                    else
                    {
                        //aspnet_UpdateFailedAttempt
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.
                        if (failureType == "password")
                        {
                            sql = "UPDATE UserLogin SET FailedPasswordAttemptCount = " + failureCount + ",";
                            sql += " FailedPasswordAttemptWindowStart = #" + RoundMinutes(DateTime.Now) + "#";
                            sql += " WHERE 	Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";
                        }
                        else
                        {
                            sql += "UPDATE 	UserLogin SET FailedPasswordAnswerAttemptCount = " + failureCount + ",";
                            sql += " FailedPasswordAnswerAttemptWindowStart =#" + RoundMinutes(DateTime.Now) + "#";
                            sql += " WHERE 	Username ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";
                        }
                        reader = factory.GetDataReader(sql);
                        if (reader.HasRows)
                        {
                            reader.Read();
                            if (!reader.IsDBNull(0))
                            {
                                rowsAffected = int.Parse(reader[0].ToString());
                            }
                        }
                        if (rowsAffected < 0)
                        {
                            throw (new ProviderException("Unable to update failure count and window start."));
                        }
                    }
                }
            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateFailureCount");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
        }

        #endregion

        #region ValidateUser

        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            bool isApproved = false;
            string pwd = "";
            string sql = "SELECT Password, IsApproved  FROM UserLogin  WHERE UserName = '" + username +"'";
            sql += " AND 	ApplicationName = '" + ApplicationName + "'  AND IsLockedOut = '0'";
            if (username == string.Empty)
            {
                return false;
            }


            try
            {
                //ASPNET_VALIDATEUSER
                factory = new DbFactory(ENM.ModuleName.ACL);
                reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    pwd = (string)(reader.GetString(0));
                    if (reader.GetString(1) == "1")
                    {
                        isApproved = true;
                    }
                    factory.Close();

                }
                else
                {
                    return false;
                }
                if (CheckPassword(password, pwd))
                {
                    //aspnet_UpdateLastLogin
                    sql = "UPDATE 	UserLogin SET LastLoginDate =#" + RoundMinutes(DateTime.Now) + "#";
                    sql += " WHERE UserName ='" + username + "' AND ApplicationName ='" + ApplicationName + "'";
                    if (isApproved)
                    {
                        isValid = true;
                        reader = factory.GetDataReader(sql);
                    }
                }
                else
                {
                    UpdateFailureCount(username, "password");
                }
            }
            catch (DbException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateUser");
                    throw (new ProviderException(exceptionMessage));
                }
                else
                {
                    throw (e);
                }
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                    reader = null;
                }
                if (factory != null)
                {
                    factory.Close();
                    factory = null;
                }
            }
            return isValid;
        }

        #endregion

        #region CheckPassword

        //
        //   CheckPassword
        //   Compares password values based on the MembershipPasswordFormat.
        //

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;
            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                pass2 = UnEncodePassword(dbpassword);
            }
            else if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                pass1 = EncodePassword(password);
            }
            else
            {
            }
            if (pass1 == pass2)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region EncodePassword

        //
        // EncodePassword
        // Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        //

        private string EncodePassword(string password)
        {
            string encodedPassword = password;
            if (PasswordFormat == MembershipPasswordFormat.Clear)
            {
            }
            else if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                encodedPassword = Encrypt(password);
            }
            else if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                HMACSHA1 hash = new HMACSHA1();
                hash.Key = HexToByte((string)HashKey);
                encodedPassword = (string)(Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password))));
            }
            else
            {
                throw (new ProviderException("Unsupported password format."));
            }
            return encodedPassword;
        }

        #endregion

        #region UnEncodePassword

        //
        // UnEncodePassword
        // Decrypts or leaves the password clear based on the PasswordFormat.
        //

        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;
            if (PasswordFormat == MembershipPasswordFormat.Clear)
            {
            }
            else if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = Decrypt(password);
            }
            else if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw (new ProviderException("Cannot unencode a hashed password."));
            }
            else
            {
                throw (new ProviderException("Unsupported password format."));
            }
            return password;
        }

        #endregion

        #region HexToByte

        //
        // HexToByte
        // Converts a hexadecimal string to a byte array. Used to convert encryption
        // key values from the configuration.
        //

        private byte[] HexToByte(string hexString)
        {
            byte[] ReturnBytes = new byte[(hexString.Length / 2) - 1 + 1];
            for (int i = 0; i <= ReturnBytes.Length - 1; i++)
            {
                ReturnBytes[i] = System.Convert.ToByte(Convert.ToByte(hexString.Substring(i * 2, 2), 16));
            }
            return ReturnBytes;
        }

        #endregion

        #region WriteToEventLog

        //
        // WriteToEventLog
        //   A helper function that writes exception detail to the event log. Exceptions
        // are written to the event log as a security measure to aSub Private database
        // details from being Returned to the browser. If a method does not Return a status
        // or boolean indicating the action succeeded or failed, a generic exception is also
        // Thrown by the caller.
        //

        private void WriteToEventLog(Exception e, string action)
        {
            //throw e;
            //http://stackoverflow.com/questions/712203/writing-to-an-event-log-in-asp-net-on-windows-server-2008-iis7
            //EventLog log = new EventLog();
            //log.Source = eventSource;
            //log.Log = eventLog;
            //string message = "An exception occurred communicating with the DB2 data source." + "\r\n" + "\r\n";
            //message += "Action: " + action + "\r\n" + "\r\n";
            //message += "Exception: " + e.ToString();
            //log.WriteEntry(message);
            string path = ConfigurationManager.AppSettings["LogFilePath"].ToString();


            //if (!File.Exists(path))
            //{
            //    // Create a file to write to.

            //    File.WriteAllLines(path, e.ToString());
            //}
            //else
            //{
            //    string appendText = "This is extra text" + Environment.NewLine;
            //    File.AppendAllText(path, e.ToString());
            //}


            // This text is added only once to the file. 
            //if (!File.Exists(path))
            //{
            //    // Create a file to write to. 
            //    using (StreamWriter sw = File.CreateText(path))
            //    {
            //        sw.WriteLine(e.ToString());
            //        sw.WriteLine();
            //    }
            //}
            //else
            //{
            //    // This text is always added, making the file longer over time 
            //    // if it is not deleted. 
            //    using (StreamWriter sw = File.AppendText(path))
            //    {
            //        sw.WriteLine(e.ToString());
            //        sw.WriteLine();
            //    }
            //}

        }

        #endregion

        #region Encrypt

        private string Encrypt(string plainText)
        {
            string returnValue = default(string);
            byte[] initVectorBytes = null;
            initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] saltValueBytes = null;
            saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);
            byte[] plainTextBytes = null;
            plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = default(PasswordDeriveBytes);
            password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] keyBytes = null;
            keyBytes = password.GetBytes(KeySize / 8);
            RijndaelManaged symmetricKey = default(RijndaelManaged);
            symmetricKey = new RijndaelManaged();
            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;
            // Generate encryptor from the existing key bytes and initialization
            // vector. Key size will be defined based on the number of the key
            // bytes.
            ICryptoTransform encryptor = default(ICryptoTransform);
            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            // Define memory stream which will be used to hold encrypted data.
            System.IO.MemoryStream memoryStream = default(System.IO.MemoryStream);
            memoryStream = new System.IO.MemoryStream();
            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = default(CryptoStream);
            cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            // Finish encrypting.
            cryptoStream.FlushFinalBlock();
            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = null;
            cipherTextBytes = memoryStream.ToArray();
            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();
            // Convert encrypted data into a base64-encoded string.
            string cipherText = default(string);
            cipherText = (string)(Convert.ToBase64String(cipherTextBytes));
            // Return encrypted string.
            returnValue = cipherText;
            return returnValue;
        }

        #endregion

        #region Decrypt

        private string Decrypt(string cipherText)
        {
            string returnValue = default(string);
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = null;
            initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] saltValueBytes = null;
            saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);
            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = null;
            cipherTextBytes = Convert.FromBase64String(cipherText);
            // First, we must create a password, from which the key will be
            // derived. This password will be generated from the specified
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = default(PasswordDeriveBytes);
            password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations);
            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = null;
            keyBytes = password.GetBytes(KeySize / 8);
            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = default(RijndaelManaged);
            symmetricKey = new RijndaelManaged();
            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;
            // Generate decryptor from the existing key bytes and initialization
            // vector. Key size will be defined based on the number of the key
            // bytes.
            ICryptoTransform decryptor = default(ICryptoTransform);
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            // Define memory stream which will be used to hold encrypted data.
            System.IO.MemoryStream memoryStream = default(System.IO.MemoryStream);
            memoryStream = new System.IO.MemoryStream(cipherTextBytes);
            // Define memory stream which will be used to hold encrypted data.
            CryptoStream cryptoStream = default(CryptoStream);
            cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = null;
            plainTextBytes = new byte[cipherTextBytes.Length + 1];
            // Start decrypting.
            int decryptedByteCount = default(int);
            decryptedByteCount = System.Convert.ToInt32(cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length));
            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();
            // Convert decrypted data into a string.
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = default(string);
            plainText = (string)(Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount));
            // Return decrypted string.
            returnValue = plainText;
            return returnValue;
        }


        #endregion

        #endregion

        public DateTime RoundMinutes(DateTime dt)
        {
            DateTime result = dt.AddMinutes(dt.Minute >= 30 ? (60 - dt.Minute) : (30 - dt.Minute));
            result = result.AddSeconds(-1 * result.Second); // To reset seconds to 0
            result = result.AddMilliseconds(-1 * result.Millisecond); // To reset milliseconds to 0
            return result;
        }

    }
}





 
