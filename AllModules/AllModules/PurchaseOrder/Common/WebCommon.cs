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
using PurchaseOrder.BusLogic;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;


namespace PurchaseOrder.Common
{
    public class WebCommon
    {
        public static object GetAllContactsFromCache()
        {
            return Contact.Common.WebCommon.GetAllContactsFromCache();
        }
        public static Hashtable GetMatchingOrdersByID(string ID)
        {
            Hashtable hTable = new Hashtable();
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);
            string sql = "select top 10 *,(select C.Company   from Contacts C  where CustID = C.ID) as Company from Purchase where ID Like '%" + ID + "%' and Status=1";
                // where ContactType =" + Convert.ToInt32(ENM.ContactType.Vendor).ToString();
            DbDataReader reader = factory.GetDataReader(sql);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    hTable.Add(reader["ID"].ToString(), reader["Company"].ToString());
                }
            }
            return hTable;
        }
        public static int InsertPurchaseToDbUsingTransaction(ArrayList PurchaseSql)
        {

            string[] queries = (string[])PurchaseSql.ToArray(typeof(string));
            DbFactory factory =null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new DbFactory(ENM.ModuleName.PurchaseOrder);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, true, "PurchaseID");

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

        public static int[] UpdatePurchaseToDbUsingTransaction(ArrayList PurchaseSql)
        {
            string[] queries = (string[])PurchaseSql.ToArray(typeof(string));
            DbFactory factory =null;
            int[] ids = new int[queries.Length];
            try
            {
                factory= new CoreAssemblies.DbFactory(ENM.ModuleName.PurchaseOrder);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, false, "PurchaseID");
                 
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

        public static DataSet GetDataTableOfPurchaseItems(string PurchaseID)
        {             
            DbFactory factory = null;             
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(PurchaseID))
                PurchaseID = "0";
            // The trick to use PI.* is to make sure ReceivedQuantity which is dummy should not come as first column
            string sql = " select PI.*, (select Sum(Credit) from ProductMasterTransactions  PMT where PMT.ItemCode =PI.Code and TransactionReferenceID=PI.PurchaseID) as ReceivedQuantity  ";
            sql+=" from [PurchaseItem] PI where PurchaseID="+ PurchaseID;
            try
            {
                factory = new DbFactory(ENM.ModuleName.PurchaseOrder);                 
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds, "PurchaseItem"); 
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