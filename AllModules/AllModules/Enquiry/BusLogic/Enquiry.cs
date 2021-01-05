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
using Enquiry.Data;
using ENM = CoreAssemblies.EnumClass;
using System.Data.Common;

namespace Enquiry.BusLogic
{
    public class Enquiry
    {
        private string sql;

        public Enquiry()
        {

        }

        public DataSet GetEnquiry(string ID)
        {

            //sql = "select O.*,U.UserName as CreatedByName,V.CompanyName as CustomerName from [Bill] AS O,Users AS U,Customer as V ";
            //sql += "where O.CreatedBy=U.UserID AND O.CustID = V.ID AND O.ID =";

            sql = "select O.*,(select U.UserName from [Users] as U where O.CreatedBy=U.UserID) as CreatedByName,";
            sql += " (select U1.UserName from Users as U1 where O.ModifiedBy=U1.UserID) as ModifiedByName,";
            sql += " (select C1.Company as Company from Contacts C1 where O.CustID = C1.ID ) as Company from ";
            sql += " [Enquiry] AS O where O.ID =";

            if (string.IsNullOrEmpty(ID))
            {
                sql += "0";
            }
            else
            {
                sql += ID;
            }

            ASM.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
            DataSet ds = new DataSet();
            factory.CreateDataAdapter(sql);
            factory.Adapter.Fill(ds);
            factory.Close();
            return ds;
        }
        public DataSet GetEnquiries(string searchText, string ID, string CreatedBy, string Company)
        {

            //sql = "select O.*,U.UserName as CreatedByName,U1.UserName as AmendedByName,V.CompanyName as CustomerName from [Bill] AS O,Users AS U,Users AS U1,Customer as V ";
            //sql += "where O.CreatedBy=U.UserID AND O.AmendedBy=U1.ID AND O.CustID = V.ID ";
            //if (!string.IsNullOrEmpty(ID))
            //{
            //    sql += " AND O.ID =" +ID;
            //}
            DataSet ds = new DataSet();

            try
            {

                sql = "select O.*,(select U.UserName from [Users] as U where O.CreatedBy=U.UserID) as CreatedByName,";
                sql += " (select U1.UserName from Users as U1 where O.AssignedTo=U1.UserID) as AssignedToName,";
                sql += " (select C.Company   from Contacts C  where O.CustID = C.ID) as Company ";
                sql += " from [Enquiry] O ";
                int Num;
                bool isNum;
                if (!string.IsNullOrEmpty(searchText))
                {
                    isNum = Int32.TryParse(searchText, out Num);

                    if (isNum)
                    {
                        sql += " where";
                        sql += " (O.ID = " + searchText + ")";
                    }
                    else
                    {
                        sql += " where (O.CreatedBy in (Select UserID from [Users] where UserName like '%" + searchText + "%')) OR";
                        sql += " (O.ModifiedBy in (Select UserID from [Users] where UserName like '%" + searchText + "%')) OR";
                        sql += " (O.CustID in (Select ID from Contacts where Company Like '%" + searchText + "%')) OR";
                        sql += " (O.CustID in (Select ID from Contacts where FirstName Like '%" + searchText + "%')) OR";
                        sql += " (O.CustID in (Select ID from Contacts where MiddleName Like '%" + searchText + "%')) OR";
                        sql += " (O.CustID in (Select ID from Contacts where LastName Like '%" + searchText + "%')) ";

                    }
                }
                else if (!string.IsNullOrEmpty(ID) || !string.IsNullOrEmpty(CreatedBy) || !string.IsNullOrEmpty(Company))
                {
                    isNum = Int32.TryParse(ID, out Num);
                    sql += "where ";
                    if (isNum)
                    {
                        sql += " (O.ID = " + ID + ") OR";
                    }
                    else
                    {
                        if (CreatedBy != string.Empty)
                        {
                            sql += " (O.CreatedBy in (Select ID from Users where UserName like '%" + CreatedBy + "%')) OR";
                        }
                        if (Company != string.Empty)
                        {
                            sql += " (O.CustID in (Select ID from Contacts where Company Like '%" + Company + "%')) OR";
                        }
                    }
                    sql = sql.Remove(sql.Length - 2, 2);
                }
                sql += " order by O.ModifiedOn DESC ";
                // Rules: * is for MS ACCESS, where as % is for ADO.NET


                ASM.DbFactory factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);

                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
                factory.Close();

            }
            catch (Exception exc)
            {
            }
            return ds;
        }

        public int[] DeleteEnquiry(string id)
        {
            string sql1 = "Delete * from [Enquiry] where ID=" + id;
            string sql2 = "Delete * from [EnquiryTerm] where EnquiryID=" + id;
            string sql3 = "Delete * from [EnquiryItem] where EnquiryID=" + id;
            string[] sql = new string[3];
            int[] ID = new int[3];
            ASM.DbFactory factory = null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
                sql[0] = sql1;
                sql[1] = sql2;
                sql[2] = sql3;
                factory.UseTransaction = true;
                ID = factory.RuncommandsWithTransaction(sql, false, null);

            }
            catch (Exception exc)
            {
                AllModules.Validate.WriteToEventLog(exc, "Enquiry:DeleteEnquiry");
            }
            finally
            {
                factory.Close();
            }
            return ID;
        }
    }
}