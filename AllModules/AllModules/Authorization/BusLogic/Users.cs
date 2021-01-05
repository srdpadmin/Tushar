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
using ASM = CoreAssemblies;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;
using System.Data.OleDb;

namespace Authorization.BusLogic
{
    public class Users
    {
        public int InsertNewUserProfile(int UserID, string UserName, string FirstName, string MiddleName, string LastName, string Company, string Email, string Phone)
        {

            string queries = "Insert into Users(UserID,UserName,FirstName,MiddleName,LastName,Company,Email,Phone,CreatedOn) values ";//(?,?,?,?,?,?,?,?,?)";
            queries +=  "(" + UserID + ",'" + UserName + "','" + FirstName ;
            queries += "','" +  MiddleName + "','" + LastName + "','" + Company ;
            queries += "','" +  Email + "','" + Phone + "',#" + DateTime.Parse(DateTime.Now.ToString()) + "#)";
            string query2 = "Select @@Identity";
            DbFactory factory = null;
            int ids;
            string[] sqlQueries = new string[2];
            sqlQueries[0] = queries;
            sqlQueries[1] = query2;
                    
            try
            {
                
                factory = new DbFactory(ENM.ModuleName.Payroll);
                int[] x = factory.Runcommands(sqlQueries);
                ids = x[1];
            }
            catch (Exception exc)
            {
                ids = -1;
                
            }
            return ids;

        }

        public int DeleteUserProfile(int UserID)
        {

            string queries = "Delete * from Users where UserID="+ UserID;             
            DbFactory factory = null;
            int ids;             

            try
            {
                factory = new DbFactory(ENM.ModuleName.Payroll);
                ids = factory.RunCommand(queries);               
            }
            catch (Exception exc)
            {
                ids = -1;
            }
            return ids;
        }

    }
}
