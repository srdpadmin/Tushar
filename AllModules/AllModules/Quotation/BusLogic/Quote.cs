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
using Quotation.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;
 

namespace Quotation.BusLogic
{
    public class Quote
    {
        private string sql;

        public Quote()
        {

        }

        public DataSet GetQuote(string ID)
        {

            //sql = "select O.*,U.UserName as CreatedByName,V.CompanyName as CustomerName from [Quote] AS O,Users AS U,Customer as V ";
            //sql += "where O.CreatedBy=U.UserID AND O.CustID = V.ID AND O.ID =";
             ASM.DbFactory factory=null;
             DataSet ds = new DataSet();
            sql = "select O.*,(select U.UserName from [Users] as U where O.CreatedBy=U.UserID) as CreatedByName,";
            sql += " (select U1.UserName from Users as U1 where O.AmendedBy=U1.UserID) as AmendedByName,";
            sql += " (select C1.Company as Company from Contacts C1 where O.CustID = C1.ID ) as Company from ";
            sql += " [Quote] AS O where O.ID =";

            if (string.IsNullOrEmpty(ID))
            {
                sql += "0";
            }
            else
            {
                sql += ID;
            }
            try
            {
            factory= new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);         
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
        public DataSet GetQuotes(string searchText, string ID, string CreatedBy, string Company,string FromDate,string ToDate)
        {

            //sql = "select O.*,U.UserName as CreatedByName,U1.UserName as AmendedByName,V.CompanyName as CustomerName from [Quote] AS O,Users AS U,Users AS U1,Customer as V ";
            //sql += "where O.CreatedBy=U.UserID AND O.AmendedBy=U1.ID AND O.CustID = V.ID ";
            //if (!string.IsNullOrEmpty(ID))
            //{
            //    sql += " AND O.ID =" +ID;
            //}
            DataSet ds = new DataSet();
              ASM.DbFactory factory=null;
            try
            {

                sql = "select O.*,(select U.UserName from [Users] as U where O.CreatedBy=U.UserID) as CreatedByName,";
                sql += " (select U1.UserName from Users as U1 where O.AmendedBy=U1.UserID) as AmendedByName,";
                sql += " (select C.Company   from Contacts C  where O.CustID = C.ID) as Company ";
                sql += " from [Quote] O ";
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
                        sql += " (O.CustID in (Select ID from Contacts where Company Like '%" + searchText + "%')) OR";
                        sql += " (O.CustID in (Select ID from Contacts where FirstName Like '%" + searchText + "%')) OR";
                        sql += " (O.CustID in (Select ID from Contacts where MiddleName Like '%" + searchText + "%')) OR";
                        sql += " (O.CustID in (Select ID from Contacts where LastName Like '%" + searchText + "%')) ";

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
                            sql += " (O.CustID in (Select ID from Contacts where Company Like '%" + Company + "%')) OR";
                        }
                        if (!String.IsNullOrEmpty(FromDate) && String.IsNullOrEmpty(ToDate))
                        {
                            sql += " (Format (O.QuoteDate,\"Short Date\")  >=#" + Convert.ToDateTime(FromDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                        if (!String.IsNullOrEmpty(ToDate) && String.IsNullOrEmpty(FromDate))
                        {
                            sql += " (Format (O.QuoteDate,\"Short Date\")  <=#" + Convert.ToDateTime(ToDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                        if (!String.IsNullOrEmpty(ToDate) && !String.IsNullOrEmpty(FromDate))
                        {
                            sql += " (Format (O.QuoteDate,\"Short Date\") between #" + Convert.ToDateTime(FromDate).ToString("MM/dd/yyyy") + "# and #" + Convert.ToDateTime(ToDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                    }
                    sql = sql.Remove(sql.Length - 2, 2);
                }
                sql += " order by O.ModifiedOn DESC ";
                // Rules: * is for MS ACCESS, where as % is for ADO.NET


                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);

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

        public int[] DeleteQuote(string id)
        {
            string sql1 = "Delete * from [Quote] where ID=" + id;
            string sql2 = "Delete * from [QuoteTerm] where QuoteID=" + id;
            string sql3 = "Delete * from [QuoteItem] where QuoteID=" + id;
            string[] sql = new string[3];
            int[] ID = new int[3];
            ASM.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
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

        public int GetQuoteStatus(string ID)
        {
            ASM.DbFactory factory = null;
            int status = 0;
            sql = "select Status from [Quote]  where ID =" + ID;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
                DbDataReader reader = factory.GetDataReader(sql);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        status = CoreAssemblies.Core.ResolveInt(reader["Status"]) != null ? Convert.ToInt32(reader["Status"]) : 0;
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

        public int UpdateQuoteStatus(string ID)
        {
            ASM.DbFactory factory = null;
            int status = 0;
            sql = "Update  [Quote] Set Status =1 where ID =" + ID;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
                factory.UseTransaction = true;
                status= factory.RunCommand(sql);
                 
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
    }
}