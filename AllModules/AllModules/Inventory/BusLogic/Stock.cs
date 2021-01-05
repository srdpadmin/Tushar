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
using Inventory.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace Inventory.BusLogic
{
    public class Stock
    {
        private string sql;

        public Stock()
        {

        }

        public DataSet GetStock(string ID)
        {
            ASM.DbFactory factory = null;
            DataSet ds = new DataSet();
            sql = "select O.*,(select U.UserName from [Users] as U where O.CreatedBy=U.UserID) as CreatedByName,";
            sql += " (select U1.UserName from Users as U1 where O.AmendedBy=U1.UserID) as AmendedByName,";
            sql += " (select L1.LocationName from Location as L1 where O.LocationID=L1.ID) as LocationName,";
            sql += " (select E1.EnumText from EnumTable as E1 where O.StockType=E1.EnumValue And E1.EnumName='StockType') as StockTypeName,";
            sql += " (select C1.Company as Company from Contacts C1 where O.VendorID = C1.ID ) as Company from ";
            sql += " [Stock] AS O where O.ID =";

            if (string.IsNullOrEmpty(ID))
            {
                sql += "0";
            }
            else
            {
                sql += ID;
            }
            sql += " order by O.ModifiedOn DESC ";
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
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

        public DataSet GetStockEntries(string searchText, string ID, string CreatedBy, string Company, string FromDate, string ToDate)
        {

           
            DataSet ds = new DataSet();
            ASM.DbFactory factory = null;
            try
            {

                sql = "select O.*,(select U.UserName from [Users] as U where O.CreatedBy=U.UserID) as CreatedByName,";
                sql += " (select U1.UserName from Users as U1 where O.AmendedBy=U1.UserID) as AmendedByName,";
                sql += " (select C.Company   from Contacts C  where O.VendorID = C.ID) as Company, ";
                sql += " (select L1.LocationName from Location as L1 where O.LocationID=L1.ID) as LocationName,";
                sql += " (select E1.EnumText from EnumTable as E1 where O.StockType=E1.EnumValue And E1.EnumName='StockType') as StockTypeName";
                sql += " from [Stock] O ";
                int Num;
                bool isNum;
                if (!string.IsNullOrEmpty(searchText))
                {
                    isNum = Int32.TryParse(searchText, out Num);

                    if (isNum)
                    {
                        sql += " where";
                        sql += " (O.ID = " + searchText + ")";
                    }
                    else
                    {
                        sql += " where (O.CreatedBy in (Select UserID from [Users] where UserName like '%" + searchText + "%')) OR";
                        sql += " (O.AmendedBy in (Select UserID from [Users] where UserName like '%" + searchText + "%')) OR";
                        sql += " (O.VendorID in (Select ID from Contacts where Company Like '%" + searchText + "%')) OR";
                        sql += " (O.VendorID in (Select ID from Contacts where FirstName Like '%" + searchText + "%')) OR";
                        sql += " (O.VendorID in (Select ID from Contacts where MiddleName Like '%" + searchText + "%')) OR";
                        sql += " (O.VendorID in (Select ID from Contacts where LastName Like '%" + searchText + "%')) ";

                    }
                }
                else if (!string.IsNullOrEmpty(ID) || !string.IsNullOrEmpty(CreatedBy) || !string.IsNullOrEmpty(Company)
                    || !string.IsNullOrEmpty(ToDate) || !string.IsNullOrEmpty(FromDate))
                {
                    isNum = Int32.TryParse(ID, out Num);
                    sql += "where ";
                    if (isNum)
                    {
                        sql += " (O.ID = " + ID + ") OR";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(CreatedBy))
                        {
                            sql += " (O.CreatedBy in (Select UserID from Users where UserName like '%" + CreatedBy + "%')) OR";
                        }
                        if (!String.IsNullOrEmpty(Company))
                        {
                            sql += " (O.VendorID in (Select ID from Contacts where Company Like '%" + Company + "%')) OR";
                        }
                        if (!String.IsNullOrEmpty(FromDate) && String.IsNullOrEmpty(ToDate))
                        {
                            sql += " (Format (O.StockDate,\"Short Date\")  >=#" + Convert.ToDateTime(FromDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                        if (!String.IsNullOrEmpty(ToDate) && String.IsNullOrEmpty(FromDate))
                        {
                            sql += " (Format (O.StockDate,\"Short Date\")  <=#" + Convert.ToDateTime(ToDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                        if (!String.IsNullOrEmpty(ToDate) && !String.IsNullOrEmpty(FromDate))
                        {
                            sql += " (Format (O.StockDate,\"Short Date\") between #" + Convert.ToDateTime(FromDate).ToString("MM/dd/yyyy") + "# and #" + Convert.ToDateTime(ToDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                    }
                    sql = sql.Remove(sql.Length - 2, 2);
                }
                sql += " order by O.ModifiedOn DESC ";
                // Rules: * is for MS ACCESS, where as % is for ADO.NET


                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);

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

        public int[] DeleteStock(string id)
        {
            string sql1 = "Delete * from [Stock] where ID=" + id;
            string sql2 = "Delete * from [StockItem] where StockID=" + id;
            string sql3 = "Delete * from [StockTerm] where StockID=" + id;
            string[] sql = new string[3];
            int[] ID = new int[3];
            ASM.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                sql[0] = sql1;
                sql[1] = sql2;
                sql[2] = sql3;

                factory.UseTransaction = true;
                ID = factory.RuncommandsWithTransaction(sql, false, null);

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
            return ID;
        }

        public int GetStockStatus(string ID,ref string StockType)
        {
            ASM.DbFactory factory = null;
            int status = 0;
            int stockType = 0;
            sql = "select  Status,StockType from [Stock]  where ID ="+ID; 
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                DbDataReader reader = factory.GetDataReader(sql);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        status = CoreAssemblies.Core.ResolveInt(reader["Status"]) != null ? Convert.ToInt32(reader["Status"]) : 0;
                        stockType = CoreAssemblies.Core.ResolveInt(reader["StockType"]) != null ? Convert.ToInt32(reader["StockType"]) : 0;
                        StockType = stockType.ToString();
                    }
                    reader.Close();
                }
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
            return status;
        }

        public int UpdateStockStatus(string ID)
        {           
            string sql = "Update  [Stock] Set Status =1 where ID =" + ID;
            int id = 0;
            ASM.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.PurchaseOrder);

                id = factory.RunCommand(sql);
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
            return id;
        }
    }
}