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
namespace Inventory.BusLogic
{
    public class StockItem
    {
        private string sql;

        public StockItem()
        {

        }

        public DataSet GetStockItems(string StockID, string StockType)
        {
            string stockID="0";
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(StockID))
            {
                stockID = "0";
            }
            else
            {
                stockID = StockID;
            }
            if (Convert.ToInt32(StockType) == (int)EnumClass.StockType.Receive)
            {
                //sql = "select PI.Quantity as ReferenceQuantity, StockItem.* from ";
                //sql += " (StockItem left join  PurchaseItem PI on PI.PurchaseId=StockItem.ReferenceID) left join ";
                //sql += " where   PI.Code=StockItem.Code and PI.Status = 1 and StockID=";
                sql = "select StockItem.*,PI.Quantity as OrderedQuantity,";
                sql += " (select Sum(Credit) from ProductMasterTransactions  PMT where PMT.ItemCode =PI.Code and TransactionReferenceID=PI.PurchaseID) as ReceivedQuantity  from ";
                sql += " (StockItem left join  PurchaseItem PI on PI.PurchaseId=StockItem.ReferenceID and PI.Code=StockItem.Code)  where  StockID="+stockID;
                //sql = "select StockItem.*,";
                //sql += " ( select Sum(Credit) from ProductMasterTransactions  PMT where PMT.ItemCode =PI.Code and TransactionReferenceID="+stockID+") as ReceivedQuantity ";
                //sql += " from (StockItem left join  PurchaseItem PI on PI.PurchaseId=StockItem.ReferenceID and PI.Code=StockItem.Code) ";
                //sql += " where  PI.Status=1 and StockID="+ stockID;
                //sql += " Union All Select StockItem.*,0 as OrderedQuantity,0 as ReceivedQuantity  from StockItem where  StockID=" + stockID;
                //sql += "  and ReferenceID Is Null";
            }
            else if (Convert.ToInt32(StockType) == (int)EnumClass.StockType.Deliver)
            {
                //sql = "select BI.Quantity as ReferenceQuantity,StockItem.* from ";
                //sql += " (StockItem left join  BillItem BI on BI.BillId=StockItem.ReferenceID) ";
                //sql += " where   BI.Code=StockItem.Code and BI.Status = 1 and StockID=";

                sql = "select StockItem.*,BI.Quantity as OrderedQuantity,";
                sql += " (select Sum(Debit) from ProductMasterTransactions  PMT where PMT.ItemCode =BI.Code and TransactionReferenceID=BI.BillID) as ReceivedQuantity  from ";
                sql += " (StockItem left join  BillItem BI on BI.BillId=StockItem.ReferenceID and BI.Code=StockItem.Code)  where  StockID=" + stockID;
               
                //sql = "select StockItem.*,PI.Quantity as OrderedQuantity,";
                //sql += " (select Sum(Credit) from ProductMasterTransactions  PMT where PMT.ItemCode =PI.Code and TransactionReferenceID=PI.PurchaseID) as ReceivedQuantity  from ";
                //sql += " (StockItem left join  PurchaseItem PI on PI.PurchaseId=StockItem.ReferenceID and PI.Code=StockItem.Code)  where  StockID=" + stockID;
               
                
            }
            DbFactory factory = null;
            
             
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
                Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }

            return ds;
        }
        public DataSet GetStockItemDescription(string prefixText)
        {

            sql = "select Code,Description,Unit,Rate,Quantity,RejectedQuantity,Discount,Tax From StockItem where Code Like '%" + prefixText + "%'";
            DbFactory factory = null;
            DataSet ds = new DataSet();
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
