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
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace Payroll.BusLogic
{
    public class DateMaster
    {
        public bool ExistingDMIDCheck(int DMID)
        {
            string sql;
            bool exists = false;
            sql = "select * from DateMaster where DM =" + DMID.ToString();

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader(sql);
                if (reader.HasRows)
                {
                    exists = true;
                }
                reader.Close();
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
            return exists;
        }
    }
}
