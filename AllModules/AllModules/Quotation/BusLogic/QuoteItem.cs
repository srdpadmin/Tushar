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
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;
using AllModules;
namespace Quotation.BusLogic
{
    public class QuoteItem
    {
        private string sql;

        public QuoteItem() 
        {              
             
        }

        public DataSet GetQuoteItems(string QuoteID)
        {
            DataSet ds = new DataSet();
            sql = "select * From QuoteItem where QuoteID=";
            DbFactory factory = null;
            if (string.IsNullOrEmpty(QuoteID))
            {
                sql += "0";
            }
            else
            {
                sql += QuoteID;
            }

            try
            {
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
                Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }
            
            return ds;
        }
        public DataSet GetQuoteItemDescription(string prefixText)
        {

            sql = "select Code,Description,Unit,Rate,Quantity,Discount,Tax From QuoteItem where Code Like '%" + prefixText + "%'";
            DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
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
