using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ENM = CoreAssemblies.EnumClass;
using Payroll.Data;
using System.Data.Common;
using AllModules;

namespace Payroll.BusLogic
{
    public class SalaryDetails
    {
        public DataSet GetSalaryDetails(string EmpID)
        {
            string sql;
            //sql = "select * from Employee where ID ="; 
            sql = "SELECT S.*,(select Description from EmployeeType where S.EmpType=ID ) AS EmpTypeDescription, ";
            sql += " (Select top 1 (CurrentBalance)  from Advance where Advance.EmpID =S.EmpID order by ID DESC) as PendingAdvance, ";
            sql += " (Select top 1 (CurrentBalance)  from Leaves where Leaves.EmpID =S.EmpID order by ID DESC) as BalanceLeaves, ";
            sql+=" (Select Description from OverTime where S.OverTimeRate=ID) as OverTimeRateDescription ";
            sql += " FROM SalaryDetails S where S.EmpID =";
            if (string.IsNullOrEmpty(EmpID))
            {
                sql += "0";
            }
            else
            {
                sql += EmpID;
            }

             
            DataSet ds = new DataSet();

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
                
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
	 
            return ds;
        }

        public SalaryDetailsData PopulateSalaryDetails(int EmpID)
        {
            string sql;
            SalaryDetailsData sdd = new SalaryDetailsData();
            sql = "select * from SalaryDetails where EmpID=" + EmpID.ToString();

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                DbDataReader dbReader = factory.GetDataReader(sql);
                if (dbReader.HasRows)
                {
                    while (dbReader.Read())
                    {
                        CoreAssemblies.Core.SetClassProperties(sdd, dbReader);
                        //Core.SetClassProperties(
                    }
                }
                dbReader.Close();
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
            return sdd;
        }
         
    }
}
