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

namespace Enquiry.BusLogic
{
    public class EnquiryItem
    {
        private string sql;

        public EnquiryItem() 
        {              
             
        }

        public DataSet GetEnquiryItems(string EnquiryID)
        {

            sql = "select * From EnquiryItem where EnquiryID=";

            if (string.IsNullOrEmpty(EnquiryID))
            {
                sql += "0";
            }
            else
            {
                sql += EnquiryID;
            }

            DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }
        public DataSet GetEnquiryItemDescription(string prefixText)
        {

            sql = "select Code,Description,Unit,Rate,Quantity,Tax From EnquiryItem where Code Like '%" + prefixText + "%'";

            DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }
    }
}
