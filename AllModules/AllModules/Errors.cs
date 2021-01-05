using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;
using System.Configuration;
using System.Data;
namespace AllModules
{
    public class Errors
    {
        private static bool logError =LogErrors;

         
        public static bool LogErrors
        {
            get {
                if (ConfigurationManager.AppSettings["LogErrors"] != null && ConfigurationManager.AppSettings["LogErrors"] != string.Empty)
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["LogErrors"]);
                else
                    return false;
                }
        }
        public static int LogErrorsCount
        {
            get
            {
                if (ConfigurationManager.AppSettings["LogErrorsCount"] != null && ConfigurationManager.AppSettings["LogErrorsCount"] != string.Empty)
                    return Convert.ToInt32(ConfigurationManager.AppSettings["LogErrorsCount"]);
                else
                    return 20;
            }
        }
        public static void LogError1(Exception exc)
        {
            string message = exc.Message;
            string description = exc.InnerException + "Stack Trace " + exc.StackTrace;
            string sql = "Insert into AppMessaging values(Message,Description,CreatedOn) Values(";
            if (LogErrors)
            {
                sql += "" + message + "," + "" + "" + description + "," + DateTime.Now + ")";
                DbFactory factory = null;
                int id;
                try
                {
                    factory = new DbFactory(ENM.ModuleName.Billing);                   
                    factory.UseTransaction = true;
                    id = factory.RunCommand(sql);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    factory.Close();
                }
            }
        }
        public static void LogError(Exception exc)
        {
            string message = exc.Message;
            string description = exc.InnerException + "Stack Trace " + exc.StackTrace;
            
            if (LogErrors)
            {
                DataSet ds = GetLoggedErrors();
                if (ds != null && ds.Tables[0].Rows.Count > LogErrorsCount)
                {
                    DeleteLoggedErrors();
                }
                OleDbConnection conn = new OleDbConnection(ConfigurationManager.ConnectionStrings["AccessDbProvider"].ToString());                      
                try
                {
                    
                    conn.Open();
                   
                  // DbCommand also implements IDisposable
                  using (OleDbCommand cmd = conn.CreateCommand())
                  {
                       // create command with placeholders
                       cmd.CommandText = 
                          "INSERT INTO AppMessaging "+
                          "([Message], [Description],  [CreatedOn]) "+
                          "VALUES(@message, @description, @createdOn)";

                       // add named parameters
                       cmd.Parameters.AddRange(new OleDbParameter[]
                       {
                           new OleDbParameter("@message", message),
                           new OleDbParameter("@description", description),
                           new OleDbParameter("@createdOn", DateTime.Now.ToShortDateString())
                       });

                       // execute
                       cmd.ExecuteNonQuery();
                  }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                   conn.Close();
                }
            }
        }
        public static DataSet GetLoggedErrors( )
        {
            DataSet ds = new DataSet();
            DbFactory factory = null;
            string sql = "Select * from AppMessaging";
            try
            {
                factory = new DbFactory(ENM.ModuleName.ACL);   
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                factory.Close();
            }
            return ds;
        }

        public static DataSet DeleteLoggedErrors()
        {
            DataSet ds = new DataSet();
            DbFactory factory = null;
            string sql = "Delete from AppMessaging";
            try
            {
                factory = new DbFactory(ENM.ModuleName.Billing);
                factory.RunCommand(sql); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                factory.Close();
            }
            return ds;
        }
    }
}