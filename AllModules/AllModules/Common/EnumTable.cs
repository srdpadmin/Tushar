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
using System.Collections.Generic;
using CoreAssemblies;
using System.Data.Common; 

namespace AllModules.Common
{
    public class EnumTable
    {
        public EnumTable()
        {

        }
        public static Dictionary<int,string> GetStatusFromCache()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader("Select * from EnumTable where EnumName='Status' order by EnumName ASC");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    dict.Add(Convert.ToInt32(reader["EnumValue"].ToString()), reader["EnumText"].ToString());
                }
            }
            return dict;
        }

        public static Dictionary<int, string> GetStockTypeFromCache()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader("Select * from EnumTable where EnumName='StockType' order by EnumValue ASC");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    dict.Add(Convert.ToInt32(reader["EnumValue"].ToString()), reader["EnumText"].ToString());
                }
            }
            return dict;
        }

        public static Dictionary<int, string> GetLocationFromCache()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader("Select * from Location order by ID ASC");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    dict.Add(Convert.ToInt32(reader["ID"].ToString()), reader["LocationName"].ToString());
                }
            }
            return dict;
        }
         
    }

}