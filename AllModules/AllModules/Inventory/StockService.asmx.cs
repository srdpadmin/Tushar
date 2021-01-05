using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using WC = Contact.Common.WebCommon;
using Inventory.BusLogic;
using System.Collections.Generic;
using AjaxControlToolkit;

namespace Inventory
{
    /// <summary>
    /// Summary description for BillingService
    /// </summary>
    /// 
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class StockService : System.Web.Services.WebService
    {
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetCustomerOnDemand(string prefixText)
        {
            List<string> alist = new List<string>();
            try
            {
                Hashtable vTable = (Hashtable)WC.GetAllContactsFromCache();

                if (vTable.Count > 0)
                {
                    foreach (DictionaryEntry pair in vTable)
                    {
                        if (pair.Value.ToString().ToLower().Contains(prefixText.ToLower()))
                        {
                            string item = AutoCompleteExtender.CreateAutoCompleteItem(pair.Value.ToString(), pair.Key.ToString());
                            alist.Add(item);

                        }
                    }
                }

            }
            catch (Exception ecx)
            {

            }
            return alist.ToArray();
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetItemsOnDemand(string prefixText)
        {
            List<string> alist = new List<string>();
            try
            {
                ProductMaster oi = new ProductMaster();
                DataSet ds = (DataSet)oi.GetProductMasterForCache(prefixText);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dRow in ds.Tables[0].Rows)
                    {
                        string item = AutoCompleteExtender.CreateAutoCompleteItem(dRow["Code"].ToString(), dRow["Description"].ToString() + "|" + dRow["Quantity"].ToString() + "|" + dRow["Unit"].ToString() + "|" + dRow["Rate"].ToString() + "|" + dRow["Discount"].ToString() + "|" + dRow["Tax"].ToString());
                        alist.Add(item);
                    }
                }

            }
            catch (Exception ecx)
            {

            }
            return alist.ToArray();
        }
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] GetOrdersOnDemand(string prefixText)
        {
            List<string> alist = new List<string>();
            try
            {

                Hashtable vTable = (Hashtable)PurchaseOrder.Common.WebCommon.GetMatchingOrdersByID(prefixText);

                if (vTable.Count > 0)
                {
                    foreach (DictionaryEntry pair in vTable)
                    {                        
                        string item = AutoCompleteExtender.CreateAutoCompleteItem(pair.Key.ToString(), pair.Key.ToString());
                        alist.Add(item);
                    }
                }

            }
            catch (Exception ecx)
            {

            }
            return alist.ToArray();
        }
    }
}
