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
namespace Payroll.BusLogic
{
    public class EmployeeDetails
    {
        public DataSet GetEmployeeDetails(string EmpID)
        {
            string sql;
            //sql = "select * from Employee where ID ="; 
            sql = "SELECT * FROM EmployeeDetails where EmpID =";
            //sql += "where O.CreatedBy=U.ID AND O.VendorID = V.ID AND O.ID =";
            if (string.IsNullOrEmpty(EmpID))
            {
                sql += "0";
            }
            else
            {
                sql += EmpID;
            }
            CoreAssemblies.DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
            }
            catch (System.Data.Common.DbException exc)
            {
                AllModules.Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            } 
            
            return ds;
        }
       
    }
}
