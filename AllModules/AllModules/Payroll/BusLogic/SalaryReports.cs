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
using Payroll.Reports;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;
using AllModules.Payroll.Reports;
using AllModules;


namespace Payroll.BusLogic
{
    public class SalaryReports
    {    
        
        public SalaryDataSet qds = null;
        public SalaryReports()
        {
            if (qds == null)
            {
                qds = new SalaryDataSet();
            }
        }

        public SalaryDataSet.EmployeeDetailsDataTable GetAllEmployeeDetails(string DMID)
        {
            //Todo:Add names combo, address combo to select
            
                string namesql = "(FirstName & Space(1) & MiddleName & Space(1) & LastName) as EmployeeName";
                string addresssql = "(AddressLine1 & Space(1) & AddressLine2 & Space(1) & City & Space(1) & Country) as Address";
                string empTypesql = "(Select EmpType from SalaryDetails S where S.EmpID =ID) as EmpType";
                string salSql = " EmployeeDetails A INNER join  ( Employee INNER Join Salary on Salary.EmpID = Employee.ID )  on Employee.ID = A.EmpID where Salary.DMID = " + DMID;
                string sql = "Select Employee.*,Employee.ID as EmpID," + empTypesql + "," + namesql + "," + addresssql + " from " + salSql;
                qds.EmployeeDetails.Clear();
                 
                CoreAssemblies.DbFactory factory = null;
                try
                {
                    factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                    factory.CreateDataAdapter(sql);
                    factory.Adapter.Fill(qds, "EmployeeDetails");
                }
                catch (System.Data.Common.DbException exc)
                {
                    Errors.LogError(exc);
                    throw exc;
                }
                catch (Exception exc)
                {
                    Errors.LogError(exc);
                    throw exc;
                }
                finally
                {
                    factory.Close();
                } 
                 
                //todo: Need to print the header depending upon the employee type
                //foreach (SalaryDataSet.EmployeeDetailsRow empRow in qds.EmployeeDetails)
                //{
                //if (empRow. == (int)ENM.EmployeeType.DailyWithoutBenefits)
                //{

                //    empRow.BasicSalary = empRow.BasicDaily * qds.SalaryInformation[0].TotalPayableDays;
                //    empRow.Allowance = 0;
                //    empRow.YearlyPaidLeaves = 0;
                //    empRow.BalanceLeaves = 0;
                //}
                //}
             
            return qds.EmployeeDetails;
        }


        public SalaryDataSet.EmployeeDetailsDataTable GetEmployeeDetails(string EmpID)
        {
            //Todo:Add names combo, address combo to select
            string namesql = "(E.FirstName & Space(1) & E.MiddleName & Space(1) & E.LastName) as EmployeeName, ";
            string addresssql = "(A.AddressLine1 & Space(1) & A.AddressLine2 & Space(1) & A.City & Space(1) & A.Country) as Address";
            string sql = "Select E.*," + namesql +  addresssql + " from Employee E INNER Join EmployeeDetails A on E.ID=A.EMPID where E.ID=" + EmpID;
            qds.EmployeeDetails.Clear();

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(qds, "EmployeeDetails");
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
            //todo: Need to print the header depending upon the employee type
            //foreach (SalaryDataSet.EmployeeDetailsRow empRow in qds.EmployeeDetails)
            //{
                //if (empRow. == (int)ENM.EmployeeType.DailyWithoutBenefits)
                //{
                    
                //    empRow.BasicSalary = empRow.BasicDaily * qds.SalaryInformation[0].TotalPayableDays;
                //    empRow.Allowance = 0;
                //    empRow.YearlyPaidLeaves = 0;
                //    empRow.BalanceLeaves = 0;
                //}
            //}
            return qds.EmployeeDetails;
        }

        public SalaryDataSet.SalaryDataTable GetSalaryDetails(string EmpID, string DMID)
        {
            qds.Salary.Clear();
            string sql = "Select * from Salary where EmpID=" + EmpID + " and DMID=" +DMID;

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(qds, "Salary");
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
	 
            return qds.Salary;
        }

        public SalaryDataSet GetOverviewDetails(string DMID)
        {
            //"iif(s.EmpType = 3,s.BasicDaily * s.TotalPayableDays,(s.BasicDaily * s.TotalPayableDays) + (s.AllowanceDaily * s.TotalPayableDays) - s.PFDeduction - s.ESICDeduction - s.ProfTaxDeduction) AS NetSalary,"+ 
            string query =
            "SELECT    e.Active, e.ID As EmpID, ( e.FirstName & Space(1) & e.MiddleName & Space(1) & e.LastName) as EmployeeName, "+
            "(s.PaidAmount- s.PFDeduction - s.ESICDeduction - s.ProfTaxDeduction) AS NetSalary," +
            
            "s.OTAmount as OverTime,   NetSalary + OTAmount AS TotalSalary,s.DaysWorked,s.OTHours, "+
            "s.AdvBalanceDeduct as AdvDeduction, s.AdvBalanceAdd as NewAdvance, s.UnPaidLeavesTaken as UnPaidLeaves, "+
            "s.PaidLeavesTaken as PaidLeaves,  s.LeavesBalanceCurrent as BalanceLeaves," +
            "TotalSalary - AdvDeduction + NewAdvance as NetPayableSalary " +
            "FROM      Employee e  LEFT JOIN  Salary s on  e.ID = s.EmpID   WHERE e.Active = true AND s.DMID=" + DMID + " order by e.ID ";


            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                factory.CreateDataAdapter(query);
                factory.Adapter.Fill(qds, "OverView");
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
	 
            return qds;
        }
    }
}

