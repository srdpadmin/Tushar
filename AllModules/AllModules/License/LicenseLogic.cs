using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace AllModules.License
{
    public class LicenseData
    {
        public int ID { get; set; }
        public string LicenseKey { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class LicenseLogic
    {
        public LicenseData GetLicense()
        {
            string sql;
            //sql = "select * from Employee where ID ="; 
            sql = "SELECT * from License";
            DbFactory factory = new DbFactory(ENM.ModuleName.Payroll);
            DbDataReader reader = factory.GetDataReader(sql);
            string empName = string.Empty;
            LicenseData cData = new LicenseData();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Core.SetClassProperties(cData, reader);
                }
            }
            else
            {//do single insert                
                cData.LicenseKey = string.Empty;
                cData.ModifiedOn = DateTime.Now;
                InsertLicense(cData);
            }
            if (cData.LicenseKey == null)
            {
                cData.LicenseKey = string.Empty;
            }
            return cData;
        }
        public bool InsertLicense(LicenseData cData)
        {
            string sql;
            int x;
            try
            {
                DbFactory factory = new DbFactory(ENM.ModuleName.Payroll);
                factory.UseTransaction = true;
                sql = Core.SetClassPropertiesValuesToSql(cData, "INSERT", "License");
                x = factory.RunCommand(sql);
                factory.CommitTransaction();
            }
            catch (Exception exc)
            {
                x = -1;
            }
            return x > 0;
        }
        public bool UpdateLicense(LicenseData cData)
        {
            string sql;
            int x;
            try
            {
                
                DbFactory factory = new DbFactory(ENM.ModuleName.Payroll);
                factory.UseTransaction = true;
                sql = Core.SetClassPropertiesValuesToSql(cData, "UPDATE", "License");
                x = factory.RunCommand(sql);
                factory.CommitTransaction();
            }
            catch (Exception exc)
            {
                x = -1;
            }
            return x > 0;
        }
    }
}
