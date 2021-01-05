using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AllModules.Payroll.Data;
using ENM = CoreAssemblies.EnumClass;
using CoreAssemblies;
using System.Data.Common;

namespace AllModules.Payroll.BusLogic
{
    public class clsConfiguration
    {
        public clsConfigurationData GetConfiguration()
        {
            string sql;
            //sql = "select * from Employee where ID ="; 
            sql = "SELECT * from Configuration";

            CoreAssemblies.DbFactory factory = null;
            clsConfigurationData cData = new clsConfigurationData();

            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader(sql);
                string empName = string.Empty;
                
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Core.SetClassProperties(cData, reader);
                    }
                }
                reader.Close();
            }
            catch (System.Data.Common.DbException exc)
            {
                Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            } 
            return cData;
        }
        public bool UpdateConfiguration(clsConfigurationData cData)
        {
            string sql;
            int x;

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                factory.UseTransaction = true;
                sql = Core.SetClassPropertiesValuesToSql(cData, "UPDATE", "Configuration");
                x = factory.RunCommand(sql);
                factory.CommitTransaction();
                
            }
            catch (System.Data.Common.DbException exc)
            {
                x = -1;
                Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                x = -1;
                Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }              
            return x > 0;
        }
        public bool InsertConfiguration(clsConfigurationData cData)
        {
            string sql;
            int x; 

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                factory.UseTransaction = true;
                sql = Core.SetClassPropertiesValuesToSql(cData, "INSERT", "Configuration");
                x = factory.RunCommand(sql);
                factory.CommitTransaction();

            }
            catch (System.Data.Common.DbException exc)
            {
                x = -1;
                Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                x = -1;
                Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            return x > 0;
        }
    }
}