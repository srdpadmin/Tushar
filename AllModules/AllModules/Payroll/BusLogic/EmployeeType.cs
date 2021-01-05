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
using System.Collections;
using CoreAssemblies;
using System.Data.Common;
using ENM = CoreAssemblies.EnumClass;
using System.Collections.Generic;
using AllModules;

namespace Payroll.BusLogic
{
    public class EmployeeType
    {
        public SortedDictionary<int, string> GetEmployeeType()
        {
            SortedDictionary<int,string> hTable = new SortedDictionary<int,string>();
           
           

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader("select * from EmployeeType order by ID ASC");
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        hTable.Add(Convert.ToInt32(reader["ID"].ToString()), reader["Description"].ToString());
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
            return hTable;
        }
        public string GetEmployeeTypeByName(int empID )
        {
            string returnString = string.Empty;
           
            string sql = "(select ( E.FirstName & Space(1) & E.MiddleName & Space(1) & E.LastName & Space(1)) as EmployeeName from Employee E where E.ID=S.EmpID)";

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader("select " + sql + " AS EmployeeName,S.EmpType,ET.Description from SalaryDetails S inner join EmployeeType ET on S.EmpType = ET.ID where S.EmpID=" + empID);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        returnString = empID.ToString() + "   ";
                        returnString += reader["EmployeeName"].ToString() + "  -  ";
                        returnString += reader["Description"].ToString();
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
	
            
            return returnString;
        }
    }
}
