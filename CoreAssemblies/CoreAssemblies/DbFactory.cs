using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Data.Common;
using System.Data.OleDb;

namespace CoreAssemblies
{
    public enum DatabaseType
    {
        SQL,MSAccess
    }
    public class DbFactory : IDisposable
    {
        //public static string PayrollProviderName = string.Empty;
        //public static string PayrollConnectionStringName = string.Empty;
        //public static string QuotationProviderName = string.Empty;
        //public static string QuotationConnectionStringName = string.Empty;
        public static string AccessProviderName = string.Empty;
        public static string AccessConnectionStringName = string.Empty;

        public const DatabaseType dbType = DatabaseType.MSAccess;
        private DbProviderFactory factory = null;
        private DbConnection connection = null;
        private DbCommandBuilder commandBuilder = null;
        private DbDataAdapter adapter = null;            
        private DbTransaction transaction = null;

        public bool UseTransaction
        {
            get;
            set;

        }
        public DbTransaction Transaction
        {
            get
            {
                return transaction;
            }
        }

        public DbDataAdapter Adapter
        {
            get
            {
                return adapter;
            }
            set { adapter = value; }
        }

        #region Commands

        private DbCommand insertCommand = null;
        public string InsertCommand
        {
            set
            {
                insertCommand.CommandText = value;
                Adapter.InsertCommand = insertCommand;
            }
        }

        private DbCommand deleteCommand = null;
        public string DeleteCommand
        {
            set
            {
                deleteCommand.CommandText = value;
                Adapter.DeleteCommand = deleteCommand;
            }
        }

        private DbCommand selectCommand = null;
        public string SelectCommand
        {
            set
            {
                selectCommand.CommandText = value;
                Adapter.SelectCommand = selectCommand;
            }
        }
        private DbCommand updateCommand = null;
        public string UpdateCommand
        {
            set
            {
                updateCommand.CommandText = value;
                Adapter.UpdateCommand = updateCommand;
            }
        }

        #endregion

        #region Transactions

        public void InitiateTransaction()
        {
            if (transaction == null)
            {
                //connection.Open(); // require to being transaction
                transaction = connection.BeginTransaction();

            }
        }
        public void CommitTransaction()
        {
            if (transaction != null)
            {
               transaction.Commit();
               connection.Close();
            }
        }
        public void RollBackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                connection.Close();
            }
        }

        #endregion

        //public DbFactory()
        //{           
        //    factory = DbProviderFactories.GetFactory(providerName);
        //    connection = factory.CreateConnection();
        //    connection.ConnectionString = connectionStringName;                        
        //}
        public DbFactory(CoreAssemblies.EnumClass.ModuleName moduleName)
        {
            bool foundFactory = false;
            switch (moduleName)
            {
                case CoreAssemblies.EnumClass.ModuleName.ACL:
                    foundFactory = true;
                    break;
                case CoreAssemblies.EnumClass.ModuleName.Payroll:
                    foundFactory = true;
                    break;
                case CoreAssemblies.EnumClass.ModuleName.Quotation:
                    foundFactory = true;
                    break;
                case CoreAssemblies.EnumClass.ModuleName.Billing:
                    foundFactory = true;
                    break;
                case CoreAssemblies.EnumClass.ModuleName.Enquiry:
                    foundFactory = true;
                    break;
                case CoreAssemblies.EnumClass.ModuleName.Inventory:
                    foundFactory = true;
                    break;

                case CoreAssemblies.EnumClass.ModuleName.PurchaseOrder:
                    foundFactory = true;
                    break;
            }
            if (foundFactory)
            {
                factory = DbProviderFactories.GetFactory(AccessProviderName);
                connection = factory.CreateConnection();
                connection.ConnectionString = AccessConnectionStringName;
            }
        }
        public void CreateDataAdapter(string sql)
        {
            adapter = factory.CreateDataAdapter();
            adapter.SelectCommand = connection.CreateCommand();
            adapter.SelectCommand.CommandText = sql;
            commandBuilder = factory.CreateCommandBuilder();
            commandBuilder.DataAdapter = adapter;
        }


        public int[] Runcommands(string[] sql)
        {
            int[] x= new int[sql.Length];
            connection.Open();           
            for(int i=0; i<sql.Length;i++)
            {
                DbCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql[i];
                x[i] = cmd.ExecuteNonQuery();               
            }
            connection.Close();
            return x;
        }
        public int[] RunScalarCommands(string[] sql)
        {
            int[] x = new int[sql.Length];
            connection.Open();
            for (int i = 0; i < sql.Length; i++)
            {
                DbCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql[i];
                x[i] = (int)cmd.ExecuteScalar();
            }
            connection.Close();
            return x;
        }
        [Obsolete]
        public int[] RuncommandsWithTransaction1(string[] sql,bool runScalar)
        {
            int[] x = new int[sql.Length];
            int index = 0;
            connection.Open();
            if (UseTransaction) 
            {
                // rollback/commit will be done later by user
                // connection will close by rollback or commit
                InitiateTransaction();
            }
            if (runScalar)
            {
                // Run the first command
                DbCommand cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = sql[0];
                x[0] = cmd.ExecuteNonQuery();
                // trying to get the identity value here
                cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = sql[1];
                x[1] = (int)cmd.ExecuteScalar();
                index = 2;
                // Set the remaining sql statements with OrderID as parameter
                for (int i = index; i < sql.Length; i++)
                {
                    cmd = connection.CreateCommand();
                    DbParameter dparam = cmd.CreateParameter();
                    dparam.DbType = DbType.Int32;
                    dparam.ParameterName = "@OrderID";
                    dparam.Value = x[1];
                    cmd.Parameters.Add(dparam);
                    cmd.Transaction = transaction;
                    cmd.CommandText = sql[i];
                    x[i] = cmd.ExecuteNonQuery();
                }
            }
            else
            {

                for (int i = index; i < sql.Length; i++)
                {

                    DbCommand cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = sql[i];
                    x[i] = cmd.ExecuteNonQuery();

                }
            }

            return x;
        }

        /// <summary>
        /// Use this method to run multiple commands with transactions not specific to identity storage in memory
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="runScalar"></param>
        /// <param name="identityColumn"></param>
        /// <returns></returns>
        public int[] RuncommandsWithTransaction(string[] sql, bool runScalar,string identityColumn)
        {
            int[] x = new int[sql.Length];
            int index = 0;
            try
            {
                connection.Open();

                if (UseTransaction)
                {
                    // rollback/commit will be done later by user
                    // connection will close by rollback or commit
                    InitiateTransaction();
                }
                if (runScalar)
                {
                    // Run the first command
                    DbCommand cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = sql[0];
                    x[0] = cmd.ExecuteNonQuery();
                    // trying to get the identity value here
                    cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = sql[1];
                    x[1] = (int)cmd.ExecuteScalar();
                    index = 2;
                    // Set the remaining sql statements with OrderID as parameter
                    for (int i = index; i < sql.Length; i++)
                    {
                        cmd = connection.CreateCommand();
                        DbParameter dparam = cmd.CreateParameter();
                        dparam.DbType = DbType.Int32;
                        dparam.ParameterName = "@" + identityColumn;
                        dparam.Value = x[1];
                        cmd.Parameters.Add(dparam);
                        cmd.Transaction = transaction;
                        cmd.CommandText = sql[i];
                        x[i] = cmd.ExecuteNonQuery();
                    }
                    CommitTransaction();
                }
                else
                {

                    for (int i = index; i < sql.Length; i++)
                    {

                        DbCommand cmd = connection.CreateCommand();
                        cmd.Transaction = transaction;
                        cmd.CommandText = sql[i];
                        x[i] = cmd.ExecuteNonQuery();

                    }
                    CommitTransaction();
                }
            }
            catch (Exception exc)
            {
                RollBackTransaction();
                
            }

            return x;
        }
        public int RunCommand(string sql)
        {
            int returnValue=0;
            connection.Open();        
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            returnValue= cmd.ExecuteNonQuery();
            connection.Close();
            return returnValue;            
        }
        [Obsolete]
        public int RunCommandWithParameter(string sql,params DbParameter[] list)
        {
            //Should make sure the files are stored using proper provider
            int returnValue = 0;
            connection.Open();
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            foreach (DbParameter param in list)
            {
                cmd.Parameters.Add(param);
            }
            returnValue = cmd.ExecuteNonQuery();
            cmd.CommandText = "Select @@Identity";
            returnValue = (int)cmd.ExecuteScalar();
            connection.Close();
            return returnValue;
        }
        public int RunCommandWithParametersList(string sql, string[] paramNames, object[] paramValue)
        {
            //Should make sure the files are stored using proper provider
            int returnValue = 0;
            try
            {
                connection.Open();
                DbCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                for (int i = 0; i < paramNames.Length; i++)
                {

                    var parameter = cmd.CreateParameter();
                    parameter.ParameterName = paramNames[i];
                    parameter.Value = paramValue[i];
                    cmd.Parameters.Add(parameter);
                }
                returnValue = cmd.ExecuteNonQuery();
            }
            catch (OleDbException exc)
            {
                
            }
            catch (DbException exc)
            {

            }
            
            finally
            {
                connection.Close();
            }

            
            //cmd.CommandText = "Select @@Identity";
            //returnValue = (int)cmd.ExecuteScalar();
            
            return returnValue;
        }
        public DbDataReader GetDataReader(string sql)
        {
            DbDataReader reader=null;
            connection.Open();
            DbCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            reader = cmd.ExecuteReader();            
            return reader;            
        }    

        #region IDisposable Members

        public void Dispose()
        {
            Dispose();
        }

        public void Close()
        {
            connection.Close();     
           
        }
        

        #endregion
    }
}

