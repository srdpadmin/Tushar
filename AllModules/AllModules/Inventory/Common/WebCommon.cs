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
using Inventory.BusLogic;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;
using Inventory.Data;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;


namespace Inventory.Common
{
    public class WebCommon
    {
        public static object GetAllContactsFromCache()
        {
            return Contact.Common.WebCommon.GetAllContactsFromCache();
        }
        public static int InsertStockToDbUsingTransaction(ArrayList StockSql)
        {

            string[] queries = (string[])StockSql.ToArray(typeof(string));
            DbFactory factory = null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new DbFactory(ENM.ModuleName.Inventory);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, true, "StockID");

            }
            catch (System.Data.Common.DbException exc)
            {
                ids[1] = -1;
                AllModules.Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                ids[1] = -1;
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }

            return ids[1];

        }

        public static int[] UpdateStockToDbUsingTransaction(ArrayList StockSql)
        {
            string[] queries = (string[])StockSql.ToArray(typeof(string));
            DbFactory factory = null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Inventory);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, false, "StockID");

            }
            catch (System.Data.Common.DbException exc)
            {
                ids[1] = -1;
                AllModules.Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                ids[1] = -1;
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            return ids;

        }

        public static object GetAllLocationFromCache()
        {
            try
            {
                if (HttpContext.Current.Cache["LocationCache"] == null)
                {
                    Location loc = new Location();
                    HttpContext.Current.Cache["LocationCache"] = loc.GetAllLocations();
                }
                
            }
            catch (Exception exc)
            {                 
                AllModules.Errors.LogError(exc);
            }

            return HttpContext.Current.Cache["LocationCache"];

        }

        public static DataSet CreateInventoryItemListFromPurchaseOrder(DataSet ds,string ID)
        {
            int index = 1;                      
            DbFactory factory = null;             
            
            DataSet PurchaseItemSet=null;
            try
            {               
                PurchaseItemSet=PurchaseOrder.Common.WebCommon.GetDataTableOfPurchaseItems(ID);
                foreach(DataRow dr in PurchaseItemSet.Tables["PurchaseItem"].Rows)
                {
                    DataRow drow = ds.Tables[0].NewRow();
                    drow["ID"] = index;
                    drow["ReferenceID"] = Convert.ToInt32(ID);
                    drow["StockType"] = (int)EnumClass.StockType.Receive;
                    drow["Code"] = dr["Code"];
                    drow["Description"] = dr["Description"];
                    drow["Quantity"] = 0.0;
                    drow["OrderedQuantity"] = dr["Quantity"];
                    drow["ReceivedQuantity"] = dr["ReceivedQuantity"];
                    drow["Rate"] = dr["Rate"];
                    drow["Unit"] = dr["Unit"];
                    drow["Tax"] = dr["Tax"];                   
                    drow["Discount"] = dr["Discount"];
                    drow["TaxAmount"] = 0;
                    drow["DiscountAmount"] =0;
                    drow["SubTotal"] = 0;
                    drow["Total"] = 0;
                    ds.Tables[0].Rows.Add(drow);
                    index += 1;
                }
            }
            catch (System.Data.Common.DbException exc)
            { 
                AllModules.Errors.LogError(exc);
                factory.Close();
            }
            catch (Exception exc)
            {
             
                AllModules.Errors.LogError(exc);
                factory.Close();
            }


            return ds;

        }
         

        #region For Inserting Stock items into Prodcut Master Transactions

        public static DataSet GetDataTableOfStockItems(string StockID)
        {
            DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
                factory = new DbFactory(ENM.ModuleName.Inventory);
                factory.CreateDataAdapter("select * from [StockItem] where StockID=" + StockID);
                factory.Adapter.Fill(ds, "StockItem");
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

        public static List<string> CreateStockItemsAsProductMasterTransactions(DataSet dt, int stockType)
        {

            List<string> list = new List<string>();
            ProductMasterTransactionsData pmt;
            int refID = 0;
            string[] codes = (from DataRow row in dt.Tables["StockItem"].Rows
                              select row["Code"].ToString()).ToArray();
            DataSet dtable = GetProductMasterIDBalanceByCode(codes);
            ProductMasterTransactions pmts = new ProductMasterTransactions();
            var dm = dtable.Tables[0].AsEnumerable();
            foreach (DataRow drow in dt.Tables["StockItem"].Rows)
            {
                var bal = from r in dm where r.Field<string>("Code").ToLower().Equals(drow["Code"].ToString().ToLower()) select r.Field<Single?>("Balance");
                var pId = from r in dm where r.Field<string>("Code").ToLower().Equals(drow["Code"].ToString().ToLower()) select r.Field<int>("ProductMasterID");
                pmt = new ProductMasterTransactionsData();
                pmt.ProductMasterID = CoreAssemblies.Core.ResolveInt(pId.FirstOrDefault()) != null ? Convert.ToInt32(pId.FirstOrDefault()) : 0;
                pmt.Balance = CoreAssemblies.Core.ResolveSingle(bal.FirstOrDefault()) != null ? Convert.ToSingle(bal.FirstOrDefault()) : 0;
                if (stockType == (int)ENM.StockType.Receive)
                {
                    pmt.TransactionType = Enum.GetName(typeof(ENM.StockType),stockType);
                    pmt.Credit = Convert.ToSingle(drow["Quantity"]);// -Convert.ToSingle(drow["RejectedQuantity"]);
                }
                else if(stockType == (int)ENM.StockType.Deliver)
                {
                    pmt.TransactionType = Enum.GetName(typeof(ENM.StockType), stockType);
                    pmt.Debit = Convert.ToSingle(drow["Quantity"]);// -Convert.ToSingle(drow["RejectedQuantity"]);
                }
                pmt.Balance = pmt.Balance - pmt.Debit +pmt.Credit;
                pmt.CreatedOn = DateTime.Now;
                pmt.TransactionID = Convert.ToInt32(drow["StockID"]);
                pmt.ItemCode = Convert.ToString(drow["Code"]);
                Int32.TryParse(drow["ReferenceID"].ToString(), out refID);
                pmt.TransactionReferenceID = refID;
               
                if (pmt.ProductMasterID != 0)
                {
                    list.Add(CoreAssemblies.Core.SetClassPropertiesValuesToSql(pmt, "INSERT", "ProductMasterTransactions"));
                }

            }


            return list;
        }

        public static bool InsertStockItemsToProductMasterTransactions(string StockID,int stockType)
        {
            bool status = false;
            // First get the Stock items details
            DataSet ds =  GetDataTableOfStockItems(StockID);
            // Second get the product master,balance and code 
            // Create a sql list for table ProductMasterTransactions to be insert from billitems into an array
            List<string> listSql = CreateStockItemsAsProductMasterTransactions(ds, stockType);
            //Insert the items to database
            string[] queries = listSql.ToArray();
            DbFactory factory = null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new DbFactory(ENM.ModuleName.Inventory);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, false, "ID");
                status = true;
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
        #endregion

        #region For Inserting Bill items into Prodcut Master Transactions
        public static DataSet GetProductMasterIDBalanceByCode(string[] codes)
        {

            string sql = "Select P.ID as ProductMasterID,P.Code, C.Balance from ProductMaster P ";
            sql +="Left join (select top 1 * from ProductMasterTransactions order by CreatedOn desc) C ";
            sql += " on P.ID=C.ProductMasterID where P.Code IN (";
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < codes.Length; i++)
            {
                str.Append("'" + codes[i] + "',");
            }
            str = str.Remove(str.Length-1, 1);
            sql += str.ToString() +")";          
          
            DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
                factory = new DbFactory(ENM.ModuleName.Inventory);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds, "Items");
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

        public static List<string> CreateBillItemsAsProductMasterTransactions(DataSet dt)
        {
            
            List<string> list = new List<string>();
            ProductMasterTransactionsData pmt;
            //var codes = dt.Tables["BillItem"].AsEnumerable()
            //.Select(row => row.Field<string>("Code"))
            //.ToArray();
            string[] codes = (from DataRow row in dt.Tables["BillItem"].Rows
                              select row["Code"].ToString()).ToArray();
            DataSet dtable = GetProductMasterIDBalanceByCode(codes);
            ProductMasterTransactions pmts = new ProductMasterTransactions();
            var dm = dtable.Tables[0].AsEnumerable();
            foreach (DataRow drow in dt.Tables["BillItem"].Rows)
            {              
                var bal = from r in dm where r.Field<string>("Code").ToLower().Equals(drow["Code"].ToString().ToLower()) select r.Field<Single?>("Balance");
                var pId = from r in dm where r.Field<string>("Code").ToLower().Equals(drow["Code"].ToString().ToLower()) select r.Field<int>("ProductMasterID");
                pmt =  new ProductMasterTransactionsData();
                pmt.ProductMasterID = CoreAssemblies.Core.ResolveInt(pId.FirstOrDefault()) != null ? Convert.ToInt32(pId.FirstOrDefault()) : 0;
                pmt.Balance = CoreAssemblies.Core.ResolveSingle(bal.FirstOrDefault()) != null ? Convert.ToSingle(bal.FirstOrDefault()) : 0;
                pmt.Debit = Convert.ToSingle(drow["Quantity"]);
                pmt.Credit = 0;
                pmt.Balance = pmt.Balance - pmt.Debit; 
                pmt.CreatedOn = DateTime.Now;
                pmt.TransactionID = Convert.ToInt32(drow["BillID"]);
                pmt.TransactionType = "Bill";
                if (pmt.ProductMasterID != 0)
                {
                    list.Add( CoreAssemblies.Core.SetClassPropertiesValuesToSql(pmt, "INSERT", "ProductMasterTransactions"));
                }
               
            }


            return list;
        }

        public static bool InsertBillItemsToProductMasterTransactions(string BillID)
        {
            bool status = false;
            // First get the billing items details
            DataSet ds = Billing.Common.WebCommon.GetDataTableOfBillItems(BillID);
            // Second get the product master,balance and code 
            // Create a sql list for table ProductMasterTransactions to be insert from billitems into an array
            List<string> listSql = CreateBillItemsAsProductMasterTransactions(ds);
            //Insert the items to database
            string[] queries =  listSql.ToArray();
            DbFactory factory = null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new DbFactory(ENM.ModuleName.Inventory);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, false, "ID");
                status = true;
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

        #endregion
    }
}