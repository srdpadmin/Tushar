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
using System.Collections.Generic;
using System.Data.Common;
using Authorization.Data;

namespace Authorization.BusLogic
{
    public class UserModule
    {
       public DataSet GetAllUserModules()
        {
            //sql = "select * from Employee where ID ="; 
            //string usernames = "(select UserName from Users where UserID=UM.UserID) as UserName";
            //string sql = "SELECT UM.*," + usernames + " from UserModule UM";
            string sql = "Select * from Users where Users.UserID in (select PKID from UserLogin)";
            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }
        public SortedDictionary<int, string> GetUsers()
        {
            SortedDictionary<int, string> hTable = new SortedDictionary<int, string>();

            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader("Select * from Users where Users.UserID in (select PKID from UserLogin) order by UserID ASC");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    hTable.Add(Convert.ToInt32(reader["UserID"].ToString()), reader["UserName"].ToString());
                }
            }
            return hTable;
        }
        public static Dictionary<int, string> ConvertModuleNamesToDictionary()
        {
            Dictionary<int, string> mydict = new Dictionary<int, string>();
            foreach (ENM.ModuleName foo in Enum.GetValues(typeof(ENM.ModuleName)))
            {
                mydict.Add((int)foo, foo.ToString());
            }

            return mydict;
        }
        public static Dictionary<int, string> ConvertRoleNamesToDictionary()
        {
            Dictionary<int, string> mydict = new Dictionary<int, string>();
            foreach (ENM.Roles foo in Enum.GetValues(typeof(ENM.Roles)))
            {
                mydict.Add((int)foo, foo.ToString());
            }
            return mydict;
        }
        public int InsertUserModules(UsersData u)
        {
            int id = 0;

            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            id = factory.RunCommand(CoreAssemblies.Core.SetClassPropertiesValuesToSql(u, "INSERT", "Users"));
            return id;
        }
        public int UpdateUserModules(UsersData u)
        {
            int id = 0;
            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            id = factory.RunCommand(CoreAssemblies.Core.SetClassPropertiesValuesToSql(u, "UPDATE", "Users"));
            return id;

        }
        public int DeleteUserModules(string userID,string userName)
        {             
            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            //TODO: Validate if user is superuser, otherwise delete
            //TODO: Check if UserId is used anywhere in any table, if so then change the user to archive rather
            //TODO: else delete from UserLogin & Users table
            int id = factory.RunCommand("Update Users SET Archive=1 where UserID=" + userID);
            Membership.DeleteUser(userName,true);               
            return id;
        }

        public bool CheckIfUserAlredyExisit(string id)
        {
            bool status = false;
            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader("Select * from Users where Users.UserID in (select PKID from UserLogin) order by UserID ASC");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader["UserID"].ToString() == id)
                    {
                        status = true;
                        break;
                    }
                }
            }
            return status;
        }
        public void GetUserModuleRolesByID(int userId, ref byte ModuleBit,ref byte RolesBit)
        {
            //sql = "select * from Employee where ID ="; 
            string sql = "select ModuleBit,RolesBit  from Users where UserID=" + userId.ToString();
            
            DbFactory factory = new DbFactory(ENM.ModuleName.ACL);
            DbDataReader reader = factory.GetDataReader(sql);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if(reader["ModuleBit"] != DBNull.Value)
                    {
                    ModuleBit = Convert.ToByte(reader["ModuleBit"].ToString());
                    }
                    if (reader["RolesBit"] != DBNull.Value)
                    {
                        RolesBit = Convert.ToByte(reader["RolesBit"].ToString());
                    }
                    break;                    
                }
            }
            factory.Close();
            
        }
    
    }
}
