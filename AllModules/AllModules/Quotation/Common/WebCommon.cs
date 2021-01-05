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
using Quotation.BusLogic;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;


namespace Quotation.Common
{
    public class WebCommon
    {
        public static object GetAllContactsFromCache()
        {
            return Contact.Common.WebCommon.GetAllContactsFromCache();
        }
        public static int InsertQuoteToDbUsingTransaction(ArrayList QuoteSql)
        {

            string[] queries = (string[])QuoteSql.ToArray(typeof(string));
            DbFactory factory =null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new DbFactory(ENM.ModuleName.Quotation);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, true, "QuoteID");

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

        public static int[] UpdateQuoteToDbUsingTransaction(ArrayList QuoteSql)
        {
            string[] queries = (string[])QuoteSql.ToArray(typeof(string));
            DbFactory factory =null;
            int[] ids = new int[queries.Length];
            try
            {
                factory= new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, false, "QuoteID");
                 
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

        public static DataSet GetDataTableOfQuoteItems(string ID)
        {             
            DbFactory factory = null;             
            DataSet ds = new DataSet();
            try
            {
                factory = new DbFactory(ENM.ModuleName.Quotation);                 
                factory.CreateDataAdapter("select * from [QuoteItem] where QuoteID=" + ID);
                factory.Adapter.Fill(ds, "QuoteItem"); 
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