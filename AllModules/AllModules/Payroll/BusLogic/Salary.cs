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
using System.Data.Common;
using System.Collections;
using Payroll.Data;
using AllModules;

namespace Payroll.BusLogic
{
    public class Salary
    {
        public bool ExistingSalaryCheck(int EmpID,int DMID)
        {
            string sql;
            bool exists=false;
            sql = "select DMID from Salary where EmpID =" + EmpID.ToString() +" AND DMID=" +DMID.ToString();           
            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    exists = true;
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
	
           
            return exists;
        }
        public DataSet GetSalary(string EmpID,string DMID)
        {           
            string sql;
            string monthName = "(select sMonth from DateMaster where DM=S.DMID) as MonthName";
            string yearName = ",(select iYear from DateMaster where DM=S.DMID) as YearName";
            sql = "SELECT S.*," + monthName + yearName + " from Salary S where S.EmpID = " + EmpID.ToString() + " AND DMID=" + DMID.ToString();           
            //sql = "SELECT * from Salary where EmpID =" + EmpID.ToString() + " AND DMID=" + DMID.ToString();               

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

        public DataSet GetSalaries(string EmpID)
        {                       
            //sql = "select * from Employee where ID ="; 
            string monthName = "(select sMonth from DateMaster where DM=S.DMID) as MonthName";
            string yearName = ",(select iYear from DateMaster where DM=S.DMID) as YearName";
            string sql = "SELECT S.*," + monthName + yearName + " from Salary S where S.EmpID = ";
            if (EmpID == null)
            {
                sql += " 0";
            }
            else
            {
                sql += EmpID;
            }
            sql += " Order By DMID Desc";
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

        public int LockUnlockSalary(string EmpID, string DMID, bool locked)
        {
            string sql;
            int id=0;
            sql = "Update Salary SET Locked =" + locked + " where EmpID =" + EmpID.ToString() + " AND DMID=" + DMID.ToString();             
            //sql = "SELECT * from Salary where EmpID =" + EmpID.ToString() + " AND DMID=" + DMID.ToString();   

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                id = factory.RunCommand(sql);
              
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
            return id;  

        }
        public bool SalaryLockCheck(int EmpID, int DMID)
        {
            string sql;
            bool locked = false;
            sql = "select Locked from Salary where EmpID =" + EmpID.ToString() + " AND DMID=" + DMID.ToString();


            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    reader.Read();
                    locked = Convert.ToBoolean(reader["Locked"].ToString());
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
            return locked;
        }
        public int DeleteSalary(string EmpID, string DMID)
        {
            int[] id = new int[2];
            Salary sal = new Salary();
            ArrayList sql = new ArrayList();
            string[] queries = null;
           
            Leaves leavesAdjustment = new Leaves(Convert.ToInt32(EmpID), Convert.ToInt32(DMID));
           
            if (leavesAdjustment.SystemGenerated == 1 && (leavesAdjustment.Credit!=0 ||leavesAdjustment.Debit!=0))
            {
                // pass in the current leaves balance to leaves object, deduct/add the salary leaves which is to be deleted 
                float? leavesBalance = leavesAdjustment.GetCurrentBalance(Convert.ToInt32(EmpID));
                leavesAdjustment.AdjustLeaves(leavesBalance.Value);
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(leavesAdjustment, "INSERT", "Leaves"));
                // Now create another row in leaves table so that when the salary is recreated, it will take
                // paid leaves as 0 or fresh start
                Leaves lv = new Leaves();
                lv.DMID = Convert.ToInt32(DMID);
                lv.EmpID = Convert.ToInt32(EmpID);
                lv.Credit = 0;
                lv.Debit = 0;
                lv.PreviousBalance = leavesBalance.Value;
                lv.CurrentBalance = leavesAdjustment.CurrentBalance;               
                lv.SystemGenerated = 1; //closure, so no new adjustment
                lv.MonthName = DateTime.Now.ToString("MMMM");
                lv.iYear = DateTime.Now.Year;
                lv.CreatedOn = DateTime.Now;
                lv.Comments = lv.MonthName + " " + lv.iYear.ToString() + " Leaves Adjusted After Salary Deletion";
                // previous balance is already set
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(lv, "INSERT", "Leaves"));
            }
            Advance advAdjustment = new Advance(Convert.ToInt32(EmpID), Convert.ToInt32(DMID), 1);
            // Get top 1 system generated=1 leave for the same month, even if it is saved last time, so we can adjust it
            if (advAdjustment.SystemGenerated == 1 && (advAdjustment.Credit != 0 || advAdjustment.Debit != 0))
            {
                float? previousBalance = advAdjustment.GetCurrentBalance(Convert.ToInt32(EmpID));
                advAdjustment.AdjustAdvance(previousBalance.Value);
                // previous balance is already set
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(advAdjustment, "INSERT", "Advance"));

                // Add new Advance to Transactions
                Advance newAdvance = new Advance();
                newAdvance.DMID = Convert.ToInt32(DMID);
                newAdvance.EmpID = Convert.ToInt32(EmpID);
                newAdvance.Credit = 0;
                newAdvance.Debit = 0;
                newAdvance.CurrentBalance = advAdjustment.CurrentBalance;                
                newAdvance.MonthName = DateTime.Now.ToString("MMMM");
                newAdvance.iYear = DateTime.Now.Year;
                newAdvance.PreviousBalance = advAdjustment.PreviousBalance;
                newAdvance.CreatedOn = DateTime.Now;
                newAdvance.SystemGenerated = 1; //open for new adjustment
                newAdvance.Comments = newAdvance.MonthName + " " +newAdvance.iYear.ToString() + " Advance Adjusted After Salary Deletion";
                sql.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(newAdvance, "INSERT", "Advance"));
             }   
                 
                sql.Add("Delete from Salary where EMPID=" + EmpID + " And DMID=" + DMID);
                queries = (string[])sql.ToArray(typeof(string));
    
	            CoreAssemblies.DbFactory factory = null;
		        try
		        {
			        factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                    factory.UseTransaction = true;
                    id = factory.Runcommands(queries);
                    factory.CommitTransaction();
		        }
		        catch (System.Data.Common.DbException exc)
		        {
			        Errors.LogError(exc);
                    id[0] = -1;
		        }		
		        catch (Exception exc)
		        {
			        Errors.LogError(exc);
                    id[0] = -1;
		        }
		        finally 
		        {
			         factory.Close();
		        } 
                return id[0];
        }
        public float? SalaryLeavesCreditCheck(string EmpID, string DMID)
        {
            string sql;            
            float? LeavesCreditTaken =0;
            sql = "select LeavesCreditTaken from Salary where EmpID =" + EmpID + " AND DMID=" + DMID; 

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        LeavesCreditTaken = CoreAssemblies.Core.ResolveSingle(reader["LeavesCreditTaken"]);
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
	 
            return LeavesCreditTaken;
        } 
    }
}
