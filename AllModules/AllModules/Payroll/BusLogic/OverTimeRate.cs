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
using System.Collections;
using System.Data.Common;

namespace Payroll.BusLogic
{
    public class OverTimeRate
    {
        public Hashtable GetOverTimeRate()
        {
            Hashtable hTable = new Hashtable();

            CoreAssemblies.DbFactory factory = null;
            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                DbDataReader reader = factory.GetDataReader("select * from OverTime");
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        hTable.Add(reader["ID"].ToString(), reader["Description"].ToString());
                    }
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
            return hTable;
        }
    }
}
