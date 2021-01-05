using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using CoreAssemblies;
using System.Data.Common;
using ENM = CoreAssemblies.EnumClass;
using System.Data;
using TermsConditions.Data;

namespace TermsConditions.BusLogic
{
    public class MasterTermConditions
    {
        public SortedList GetAllTerms()
        {
            SortedList hTable = new SortedList();
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);
            DbDataReader reader = factory.GetDataReader("select * from MasterTerms order by ID ASC");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    hTable.Add(reader["ID"].ToString(), reader["Term"].ToString());
                }
            }
            return hTable;
        }
        public SortedList GetAllConditions()
        {
            SortedList hTable = new SortedList();
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);
            DbDataReader reader = factory.GetDataReader("select * from MasterConditions order by ID ASC");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    hTable.Add(reader["ID"].ToString(), reader["Condition"].ToString());
                }
            }
            return hTable;
        }

        //For Master Terms & Condition Page
        public DataSet GetAllTerms(string TermName)
        {
            string sql = "select * from MasterTerms where Term like '%" + TermName+ "%' ";
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);             
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }
        public DataSet GetAllConditions(string ConditionName)
        {
            string sql = "select * from MasterConditions where Condition like '%" + ConditionName + "%'";
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);             
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }

        public int InsertTermOrCondition(MasterTerms mt, MasterConditions mc)
        {
            string sql = string.Empty;
            if (mt == null)
            {
                sql = CoreAssemblies.Core.SetClassPropertiesValuesToSql(mc, "INSERT", "MasterConditions");
            }
            else
            {
                sql = CoreAssemblies.Core.SetClassPropertiesValuesToSql(mt, "INSERT", "MasterTerms");
            }

            int id = 0;
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);
            id = factory.RunCommand(sql);
            factory.Close();
            return id;

        }

        public int UpdateTermOrCondition(MasterTerms mt, MasterConditions mc)
        {
            string sql = string.Empty;
            if (mt == null)
            {
                sql = CoreAssemblies.Core.SetClassPropertiesValuesToSql(mc, "UPDATE", "MasterConditions");
            }
            else
            {
                sql = CoreAssemblies.Core.SetClassPropertiesValuesToSql(mt, "UPDATE", "MasterTerms");
            }

            int id = 0;
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);
            id = factory.RunCommand(sql);
            factory.Close();
            return id;

        }
        public int DeleteTermOrCondtion(string ID,bool mt)
        {
            string sql = string.Empty;
            if (mt == false)
            {
                sql = "Delete * from MasterConditions where ID=" + ID;
            }
            else
            {
                sql = "Delete * from MasterTerms where ID=" + ID; 
            }

            int id = 0;
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);
            id = factory.RunCommand(sql);
            factory.Close();
            return id;
        }
    }
}
