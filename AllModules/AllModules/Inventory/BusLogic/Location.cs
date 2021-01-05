using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using CoreAssemblies;
using System.Data.Common;
using ENM = CoreAssemblies.EnumClass;
using System.Data;
using Inventory.Data;

namespace Inventory.BusLogic
{
    public class Location
    {
        public SortedList GetAllLocations()
        {
            SortedList hTable = new SortedList();
            DbFactory factory = new DbFactory(ENM.ModuleName.Inventory);
            DbDataReader reader = factory.GetDataReader("select * from Location order by ID ASC");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    hTable.Add(reader["ID"].ToString(), reader["LocationName"].ToString());
                }
            }
            return hTable;
        }

        public DataSet GetAllLocations(string TermName)
        {
            string sql = "select * from Location where LocationName like '%" + TermName + "%' ";
            DbFactory factory = new DbFactory(ENM.ModuleName.Inventory);
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }

        public int InsertLocation(LocationData mt)
        {
            string sql = string.Empty;
            int id = 0;
            sql = CoreAssemblies.Core.SetClassPropertiesValuesToSql(mt, "INSERT", "Location");  
            DbFactory factory = new DbFactory(ENM.ModuleName.Inventory);
            id = factory.RunCommand(sql);
            factory.Close();
            return id;

        }

        public int UpdateLocation(LocationData mt)
        {
            string sql = string.Empty;             
            int id = 0;
            sql = CoreAssemblies.Core.SetClassPropertiesValuesToSql(mt, "UPDATE", "Location");
            DbFactory factory = new DbFactory(ENM.ModuleName.Inventory);
            id = factory.RunCommand(sql);
            factory.Close();
            return id;

        }

        public int DeleteLocation(string ID)
        {
            string sql = string.Empty;
            int id = 0;
            sql = "Delete * from Location where ID=" + ID;  
            DbFactory factory = new DbFactory(ENM.ModuleName.Inventory);
            id = factory.RunCommand(sql);
            factory.Close();
            return id;
        }
    }
}