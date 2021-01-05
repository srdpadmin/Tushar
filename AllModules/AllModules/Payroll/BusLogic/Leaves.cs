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
using ASM = CoreAssemblies;
using System.Data.Common;
using System.Collections;
using Payroll.Data;
using CoreAssemblies;
using AllModules;

namespace Payroll.BusLogic
{
    public class Leaves : LeavesData
    {
        public Leaves()
        {
        }
        public Leaves(int empID, int dmID)
        {
            string sql;
            CoreAssemblies.DbFactory factory = null;
            try
            {
                sql = "select top 1 * from Leaves where EmpID =" + empID;
                if (dmID > 0)
                {
                    sql += " And DMID=" + dmID.ToString();
                }
                sql += " order by ID DESC";
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        this.EmpID = empID;
                        this.DMID = dmID;
                        this.CurrentBalance = CoreAssemblies.Core.ResolveSingle(reader["CurrentBalance"]) != null ? Convert.ToSingle(reader["CurrentBalance"]) : 0;
                        this.PreviousBalance = CoreAssemblies.Core.ResolveSingle(reader["PreviousBalance"]) != null ? Convert.ToSingle(reader["PreviousBalance"]) : 0;
                        this.Comments = CoreAssemblies.Core.ResolveString(reader["Comments"]);
                        this.Credit = CoreAssemblies.Core.ResolveSingle(reader["Credit"]) != null ? Convert.ToSingle(reader["Credit"]) : 0;
                        this.Debit = CoreAssemblies.Core.ResolveSingle(reader["Debit"]) != null ? Convert.ToSingle(reader["Debit"]) : 0;
                        this.SystemGenerated = CoreAssemblies.Core.ResolveInt(reader["SystemGenerated"]) != null ? Convert.ToInt32(reader["SystemGenerated"]) : 0;
                        break;
                    }
                    reader.Close();
                }
                 
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
	
        }
        public float? GetCurrentBalance(int EmpID)
        {            
            string sql;
            float? balance = 0;
            sql = "select top 1 CurrentBalance from Leaves where EmpID =" + EmpID + " order by ID DESC";

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        balance = CoreAssemblies.Core.ResolveSingle(reader["CurrentBalance"]);
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
            return balance;
        }

        public void GetCurrentBalance(LeavesData ld)
        {

            string sql;
            // If we find a system generated leave deduction, then we adjust against it and set system generated=0
            // so we have the latest leave, then we add our new deduction with system generated =1
            sql = "select top 1 * from Leaves where EmpID =" + ld.EmpID + " And DMID=" + ld.DMID.ToString() + " And SystemGenerated = 1 order by ID DESC";
                
            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ld.CurrentBalance = CoreAssemblies.Core.ResolveSingle(reader["CurrentBalance"]) != null ? Convert.ToSingle(reader["CurrentBalance"]) : 0;
                        ld.PreviousBalance = CoreAssemblies.Core.ResolveSingle(reader["PreviousBalance"]) != null ? Convert.ToSingle(reader["PreviousBalance"]) : 0;
                        ld.Comments = CoreAssemblies.Core.ResolveString(reader["Comments"]);
                        ld.Credit = CoreAssemblies.Core.ResolveSingle(reader["Credit"]) != null ? Convert.ToSingle(reader["Credit"]) : 0;
                        ld.Debit = CoreAssemblies.Core.ResolveSingle(reader["Debit"]) != null ? Convert.ToSingle(reader["Debit"]) : 0;
                        ld.SystemGenerated = CoreAssemblies.Core.ResolveInt(reader["SystemGenerated"]) != null ? Convert.ToInt32(reader["SystemGenerated"]) : 0;
                    }
                    reader.Close();
                }     
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
        }

        public int InsertLeaveTransaction(LeavesData leaves)
        {
            int x = 0;
            string insertSql = ASM.Core.SetClassPropertiesValuesToSql(leaves, "INSERT", "Leaves");

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                factory.UseTransaction = true;
                x = factory.RunCommand(insertSql);
                factory.CommitTransaction();
            }
            catch (System.Data.Common.DbException exc)
            {
                Errors.LogError(exc);
                x = -1;
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
                x = -1;
            }
            finally
            {
                factory.Close();
            } 
             
            return x;
        }
        public DataTable GetAllLeavesTransactions(int EmpID)
        {
            //sql = "select * from Employee where ID ="; 
            //string monthName = " MonthName(iMonth),";
            string yearName  = " L.iYear as YearName";
            string sql = "SELECT L.*," + yearName + " from Leaves L where L.EmpID = ";
            if (EmpID == 0)
            {
                sql += " 0";
            }
            else
            {
                sql += EmpID;
            }
            sql +=  " Order by ID Desc";

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
            return ds.Tables[0];

        }
        public void AdjustLeaves(float advBalancePrevious)
        {

            float temp = this.Credit;
            this.Credit = this.Debit;
            this.Debit = temp;
            this.PreviousBalance = advBalancePrevious;// this.CurrentBalance;
            this.CurrentBalance = advBalancePrevious + this.Credit - this.Debit;// this.CurrentBalance + this.Credit - this.Debit;
            this.SystemGenerated = 0; //closure, so no new adjustment
            this.MonthName = DateTime.Now.ToString("MMMM");
            this.iYear = DateTime.Now.Year;
            this.CreatedOn = DateTime.Now;
            this.Comments = this.MonthName + " " + this.iYear.ToString() + " Leaves Adjustment";

        }
        //public void AdjustLeaves()
        //{
        //    float temp = this.Credit;            
        //    this.Credit = this.Debit;
        //    this.Debit = temp;
        //    this.PreviousBalance = this.CurrentBalance;
        //    this.CurrentBalance = this.CurrentBalance + this.Credit - this.Debit;
        //    this.Comments = "Salary Adjustment";
        //    this.SystemGenerated = 0; //closure, so no new adjustment
        //    this.MonthName = DateTime.Now.ToString("MMMM");
        //    this.iYear = DateTime.Now.Year;
        //    this.CreatedOn = DateTime.Now;
        //}
    }
}
