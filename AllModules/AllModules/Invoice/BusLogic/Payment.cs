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
namespace Invoice.BusLogic
{
    public class Payment
    {
        private string sql;

        public Payment()
        {

        }

        public DataSet GetPayments(string RefID)
        {
            DataSet ds = new DataSet();
            sql = "select * From Payment where BillID=";
            DbFactory factory = null;
            if (string.IsNullOrEmpty(RefID))
            {
                sql += "0";
            }
            else
            {
                sql += RefID;
            }

            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Billing);
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
        
        public DataSet GetBillItemDescription(string prefixText)
        {

            sql = "select Code,Description,Unit,Rate,Quantity,Discount,Tax From BillItem where Code Like '%" + prefixText + "%'";
            DbFactory factory = null;
            DataSet ds = new DataSet();
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Billing);

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
