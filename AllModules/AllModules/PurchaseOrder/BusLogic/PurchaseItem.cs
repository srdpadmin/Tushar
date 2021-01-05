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
namespace PurchaseOrder.BusLogic
{
    public class PurchaseItem
    {
        private string sql;

        public PurchaseItem() 
        {              
             
        }

        public DataSet GetPurchaseItems(string PurchaseID)
        {
            return PurchaseOrder.Common.WebCommon.GetDataTableOfPurchaseItems(PurchaseID);
        }

        //public DataSet GetPurchaseItems(string PurchaseID)
        //{
        //    DataSet ds = new DataSet();
        //    sql = "select * From PurchaseItem where PurchaseID=";
        //    DbFactory factory = null;
        //    if (string.IsNullOrEmpty(PurchaseID))
        //    {
        //        sql += "0";
        //    }
        //    else
        //    {
        //        sql += PurchaseID;
        //    }

        //    try
        //    {
        //        factory = new CoreAssemblies.DbFactory(ENM.ModuleName.PurchaseOrder);
        //        factory.CreateDataAdapter(sql);
        //        factory.Adapter.Fill(ds);
               
        //    }
        //    catch (System.Data.Common.DbException exc)
        //    {
        //        AllModules.Errors.LogError(exc);
        //    }	
        //    catch (Exception exc)
        //    {
        //        Errors.LogError(exc);
        //    }
        //    finally
        //    {
        //        factory.Close();
        //    }
            
        //    return ds;
        //}
        public DataSet GetPurchaseItemDescription(string prefixText)
        {

            sql = "select Code,Description,Unit,Rate,Quantity,Discount,Tax From PurchaseItem where Code Like '%" + prefixText + "%'";
            DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
             factory = new CoreAssemblies.DbFactory(ENM.ModuleName.PurchaseOrder);
            
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
