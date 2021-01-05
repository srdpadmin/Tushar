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
using ASM = CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;

namespace Payroll.BusLogic
{
    public class EmployeeProfile
    {
        public DataSet GetAllItems()
        {
            string sql;
            sql = "select EP.*,(select Description from EmployeeType where EP.ID=ID) as EmpTypeDescription,";
            sql +="(select  FirstName & LastName from EmployeeDetails where EP.ID=ID) as EmpName";
            sql +=" from EmployeeProfile EP ";
           
            //if (string.IsNullOrEmpty(ID))
            //{
            //    sql += "0";
            //}
            //else
            //{
            //    sql += ID;
            //}
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
