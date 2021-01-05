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

using System.Data.Common;
using ENM = CoreAssemblies.EnumClass;
using System.Collections;
using CoreAssemblies;
using System.Collections.Generic;
using AllModules;

namespace Payroll.BusLogic
{
    public class Employee
    {
        public SortedDictionary<string, string> GetEmployees()
        {
            SortedDictionary<string, string> hTable = new SortedDictionary<string, string>();

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader("select * from Employee order by FirstName,MiddleName,LastName");
                string empName = string.Empty;

                if (reader.HasRows)
                {
                    hTable.Add("0", "Select Employee");
                    while (reader.Read())
                    {
                        empName = reader["FirstName"].ToString() + " " + reader["MiddleName"].ToString() + " " + reader["LastName"].ToString();
                        hTable.Add(reader["ID"].ToString(), empName);
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

        public DataSet GetEmployee(string ID)
        {
            string sql;
            //sql = "select * from Employee where ID ="; 
            sql = "SELECT E.*,(select  FirstName & LastName from Employee where E.ID=ID) AS EmployeeName ";
            sql += "FROM Employee AS E where E.ID =";
            //sql += "where O.CreatedBy=U.ID AND O.VendorID = V.ID AND O.ID =";
            if (string.IsNullOrEmpty(ID))
            {
                sql += "0";
            }
            else
            {
                sql += ID;
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
        public int[] DeleteEmployee(string id)
        {
            string sql0 = "Delete * from [Files] where ID in (Select FileID from Employee where ID=" + id+")";
            string sql1 = "Delete * from [Employee] where ID=" + id;
            string sql2 = "Delete * from [EmployeeDetails] where EmpID=" + id;
            string sql3 = "Delete * from [EmployeeProfile] where EmpID=" + id;
            string sql4 = "Delete * from [SalaryDetails] where EmpID=" + id;
            string sql5 = "Delete * from [Salary] where EmpID=" + id;
            
            string[] sql = new string[6];
            int[] ID = new int[6];
            DbFactory factory = null;  
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                sql[0] = sql0;
                sql[1] = sql1;
                sql[2] = sql2;
                sql[3] = sql3;
                sql[4] = sql4;
                sql[5] = sql5;
                factory.UseTransaction = true;
                ID = factory.RuncommandsWithTransaction(sql, false, null);
                factory.Close();
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
            return ID;
        }

        public DataSet SearchEmployees(string searchText, string EmployeeName, string EmpID)
        {
            string sql;
            //sql = "select * from Employee where ID ="; 
            sql = "SELECT E.ID,( E.FirstName & Space(1) & E.MiddleName & Space(1) & E.LastName) as EmployeeName,";
            sql += " (select Description from EmployeeType where S.EmpType=ID) as EmpTypeName,";
            sql += " (select Description from OverTime where S.OverTimeRate=ID) as OTRateName, ";
            sql += " iif(S.BasicMonthly =0,S.BasicDaily * 30,S.BasicMonthly) as BasicSalary,";
            sql += " S.OverTimeRate FROM Employee E INNER JOIN SalaryDetails S on S.EmpID=E.ID ";

            //if (!string.IsNullOrEmpty(EmpID))
            //{
            //    sql += " where E.ID =" + EmpID;
            //}
            int Num;
            bool isNum;
            if (!string.IsNullOrEmpty(searchText) && searchText.Trim().Length >0)
            {
                isNum = Int32.TryParse(searchText.Trim(), out Num);
                sql += " where ";
                if (isNum)
                {                   
                    sql += " (E.ID = " + searchText + ")";
                }
                else
                {
                    sql += " (E.FirstName like '%" + searchText + "%') OR";
                    sql += " (E.MiddleName like '%" + searchText + "%') OR";
                    sql += " (E.LastName  like '%" + searchText + "%') ";
                    //sql += " (O.City  like '%" + searchText + "%') OR";
                }
            }
            else if (!string.IsNullOrEmpty(EmpID) || !string.IsNullOrEmpty(EmployeeName))
            {
                isNum = Int32.TryParse(EmpID.Trim(), out Num);
                sql += " where ";
                if (isNum)
                {                    
                    sql += " (E.ID = " + EmpID + ") ";
                }               
                
                if (EmployeeName.Trim() != string.Empty)
                {
                    sql += " (E.FirstName like '%" + EmployeeName + "%') OR";
                    sql += " (E.MiddleName like '%" + EmployeeName + "%') OR";
                    sql += " (E.LastName  like '%" + EmployeeName + "%') ";
                } 
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

        public DataSet EmployeeLookup(string prefix)
        {
            string sql;
            //sql = "select * from Employee where ID ="; 
            sql = "SELECT E.ID,( E.FirstName & Space(1) & E.MiddleName & Space(1) & E.LastName) as EmployeeName,";
            sql += " FROM Employee E where E.FirstName like %" + prefix + "% OR E.MiddleName like %" + prefix;
            sql += "% Or E.LastName like %" + prefix + "%";

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
    }
}
