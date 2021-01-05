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
using Payroll.BusLogic;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;
using System.Collections;
using System.Data.Common;

namespace Payroll.Common
{
    public class WebCommon
    {
        public static object GetEmployeeTypeFromCache()
        {
            if (HttpContext.Current.Cache["EmployeeTypeCache"] == null)
            {
                EmployeeType et = new EmployeeType();
                HttpContext.Current.Cache["EmployeeTypeCache"] = et.GetEmployeeType();
            }
            return HttpContext.Current.Cache["EmployeeTypeCache"];
        }

        public static object GetOverTimeRateFromCache()
        {
            if (HttpContext.Current.Cache["OverTimeRateCache"] == null)
            {
                OverTimeRate et = new OverTimeRate();
                HttpContext.Current.Cache["OverTimeRateCache"] = et.GetOverTimeRate();
            }
            return HttpContext.Current.Cache["OverTimeRateCache"];
        }

        public static int InsertEmployeeToDbUsingTransaction(ArrayList OrderSql)
        {

            string[] queries = (string[])OrderSql.ToArray(typeof(string));
            DbFactory factory = null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, true, "EmpID");

            }
            catch (Exception exc)
            {
                ids[1] = -1;
            }
            finally
            {
                factory.Close();
            }
            return ids[1];

        }

        public static int[] UpdateEmployeeToDbUsingTransaction(ArrayList OrderSql)
        {
            string[] queries = (string[])OrderSql.ToArray(typeof(string));
            DbFactory factory = null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, false, "EmpID");

            }
            catch (Exception exc)
            {
                ids[1] = -1;
            }
            finally
            {
                factory.Close();
            }
            return ids;

        }

        public static void GetLastAdvanceAndLeavesBalance(int EmpID, ref CurrentMonthDetails cmd)
        {
            //http://stackoverflow.com/questions/4682821/oracle-get-a-query-to-always-return-exactly-one-row-even-when-theres-no-data
            string sql=string.Empty;
            float advBalancePrevious = 0;
            float leavesBalancePrevious = 0;
            float leavesCredit = 0;
            float yearlyPaidLeaves = 0;

            sql += "select  DMID, LeavesBalanceCurrent,AdvBalanceCurrent , (select LeavesCredit from SalaryDetails where EmpId=104) as LeavesCredit, (select Yearlypaidleaves from SalaryDetails where EmpId=104) as YearlyPaidLeaves  from Salary where DMID = (SELECT Top 1 ID from DateMaster order by DM Desc,ID)  and EmpID=" + EmpID.ToString();
            sql+=" UNION (";
            sql += "select 0 as DMID, 0 as LeavesBalanceCurrent,0 as AdvBalanceCurrent,  LeavesCredit, YearlyPaidLeaves from SalaryDetails where EmpId=" + EmpID.ToString() + " and not exists ";
            sql += "(select  DMID from Salary where DMID = (SELECT Top 1 ID from DateMaster order by DM Desc,ID)  and EmpID=" + EmpID.ToString() + " ))";
            CoreAssemblies.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
            DbDataReader reader = factory.GetDataReader(sql);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    float.TryParse(reader["AdvBalanceCurrent"].ToString(), out advBalancePrevious);
                    float.TryParse(reader["LeavesBalanceCurrent"].ToString(), out leavesBalancePrevious);
                    float.TryParse(reader["LeavesCredit"].ToString(), out leavesCredit);
                    float.TryParse(reader["YearlyPaidLeaves"].ToString(), out yearlyPaidLeaves);
                    cmd.LeavesBalancePrevious = leavesBalancePrevious;
                    cmd.AdvBalancePrevious = advBalancePrevious;
                    if(leavesCredit > 0)
                    {
                        cmd.LeavesBalancePrevious = cmd.LeavesBalancePrevious + leavesCredit;
                        //cmd.LeavesCreditTaken = leavesCredit;
                    }
                }
            }
        //  return cmd;
        }

       
    
    }
}
