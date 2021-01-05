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
using ENM=CoreAssemblies.EnumClass;
using System.Collections;
using System.Data.Common;
using Contact.Data;

namespace Contact.BusLogic
{
    public class Contacts
    {
        public Hashtable GetAllContacts()
        {
            Hashtable hTable = new Hashtable();
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);
            string sql = "select * from Contacts";// where ContactType =" + Convert.ToInt32(ENM.ContactType.Vendor).ToString();
            DbDataReader reader = factory.GetDataReader(sql);
            if(reader.HasRows)
            {
                while (reader.Read())
                {                  
                    hTable.Add(reader["ID"].ToString(), reader["Company"].ToString());
                }
            }
            return hTable;
        }

        public DataSet GetContactDataSetById(string ID)
        {
            DataSet ds = new DataSet();
            DbFactory factory = new DbFactory(ENM.ModuleName.Quotation);
            factory.CreateDataAdapter("select * from Contacts where ID=" + ID);
            factory.Adapter.Fill(ds, "Contact");
            factory.Close();
            return ds;

        } 

        public DataSet GetContacts(string SearchString)
        {

            string sql = "select * From Contacts ";

            if (!string.IsNullOrEmpty(SearchString))
            {
                sql += "where (FirstName like '%" + SearchString + "%') ";
                sql += "OR (MiddleName Like '%" + SearchString + "%') ";
                sql += "OR (LastName Like '%" + SearchString + "%') ";
                sql += "OR (Company Like '%" + SearchString + "%') ";
                sql += "OR (City Like '%" + SearchString + "%') ";
            }            

            DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }
        public ContactsData GetContactsByID(string ID)
        {
            ContactsData cd = new ContactsData();
            string sql = "select * From Contacts where ID=" +ID;           

            DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
            DbDataReader reader = factory.GetDataReader(sql);
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CoreAssemblies.Core.SetClassProperties(cd, reader);
                }
            }
            
            factory.Close();
            return cd;
        }

        public int InsertContact(ContactsData CD)
        {
            string sql = CoreAssemblies.Core.SetClassPropertiesValuesToSql(CD, "INSERT", "Contacts");
            DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
            int id=factory.RunCommand(sql);
            return id;
        }
        public int UpdateContact(ContactsData CD)
        {
            string sql = CoreAssemblies.Core.SetClassPropertiesValuesToSql(CD, "UPDATE", "Contacts");
            DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
            int id = factory.RunCommand(sql);
            return id;
        }
        public int DeleteContact(string ID)
        {
            string sql = "Delete * from Contacts where Id =" + ID;
            DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Quotation);
            int id = factory.RunCommand(sql);
            return id;
        }
    }

}
