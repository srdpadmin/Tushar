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
using CoreAssemblies;

namespace Enquiry.BusLogic
{
    public class Enquiries
    {
        
        public Enquiries()
        {

        }
        public DataSet GetAllEnquiries(string searchText, string ID, string CreatedBy, string Company,string FromDate,string ToDate,string status,bool callback)
        {
            string sql = "Select *,(select EnumText from  EnumTable where EnumName='EnquiryType' and O.EnquiryType = EnumValue) as EnquiryTypeName,";
            sql += "(select EnumText from  EnumTable where EnumName='EnquiryStatus' and O.Status = EnumValue) as EnquiryStatusName,";
            sql += "(select U.UserName from [Users] as U where O.CreatedBy=U.UserID) as CreatedByName, ";
            sql += "(select U.UserName from [Users] as U where O.ModifiedBy=U.UserID) as ModifiedByName ";
            sql += " from Enquiries O ";
            ASM.DbFactory factory = null;
            DataSet ds = new DataSet();
                        
            try
            {
                int Num;
                bool isNum;
                if (!string.IsNullOrEmpty(searchText))
                {
                    isNum = Int32.TryParse(searchText, out Num);

                    if (isNum)
                    {
                        sql += " where";
                        sql += " (O.ID = " + searchText + ") OR ";
                        sql += " (O.Telephone = " + searchText + ")";
                    }
                    else
                    {
                        sql += " where (O.CreatedBy in (Select UserID from [Users] where UserName like '%" + searchText + "%')) OR";
                        sql += " (O.ModifiedBy in (Select UserID from [Users] where UserName like '%" + searchText + "%')) OR";
                        sql += " O.Company Like '%" + searchText + "%' OR";
                        sql += " O.EName Like '%" + searchText + "%' OR";
                        sql += " O.Email Like '%" + searchText + "%' OR";
                        sql += " O.Subject Like '%" + searchText + "%' OR";
                        sql += " O.Message Like '%" + searchText + "%' ";
                    }
                     
                }
                else if (!string.IsNullOrEmpty(ID) || !string.IsNullOrEmpty(CreatedBy) || !string.IsNullOrEmpty(Company)
                    || !string.IsNullOrEmpty(ToDate) || !string.IsNullOrEmpty(FromDate) || (!string.IsNullOrEmpty(status) && status !="All")
                    || callback != false)
                {
                    isNum = Int32.TryParse(ID, out Num);
                    sql += " where ";
                    if (isNum)
                    {
                        sql += " (O.ID = " + ID + ") OR";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(CreatedBy))
                        {
                            sql += " (O.CreatedBy in (Select UserID from Users where UserName like '%" + CreatedBy.ToLower() + "%')) OR";
                        }
                        if (!String.IsNullOrEmpty(Company))
                        {
                            sql += " (O.Company Like '%" + Company + "%') OR";
                            sql += " (O.EName Like '%" + Company + "%') OR";
                        }
                        if (!String.IsNullOrEmpty(status) && status != "All" && !callback)
                        {
                            sql += " (O.Status =" + status + ") OR";                            
                        }
                        if (callback)
                        {
                            sql += " (O.Status =1 And O.CallBackDate Is Not Null And Format(CallbackDate,\"Short Date\")<Format(Now(),\"Short Date\")) OR";
                        }
                        if (!String.IsNullOrEmpty(FromDate) && String.IsNullOrEmpty(ToDate))
                        {

                            sql += " (Format (O.CreatedOn,\"Short Date\") >=#" + Convert.ToDateTime(FromDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                        if (!String.IsNullOrEmpty(ToDate) && String.IsNullOrEmpty(FromDate))
                        {
                            sql += " (Format (O.CreatedOn,\"Short Date\") <=#" + Convert.ToDateTime(ToDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                        if (!String.IsNullOrEmpty(ToDate) && !String.IsNullOrEmpty(FromDate))
                        {
                            sql += " (Format (O.CreatedOn,\"Short Date\")  between #" + (Convert.ToDateTime(FromDate).ToString("MM/dd/yyyy")) + "# and #" + Convert.ToDateTime(ToDate).ToString("MM/dd/yyyy") + "#) OR";
                        }
                    }
                    sql = sql.Remove(sql.Length - 2, 2);
                }
                sql += " order by O.ModifiedOn DESC ";
                // Rules: * is for MS ACCESS, where as % is for ADO.NET

                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
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
            return ds;
        }
        public DataSet GetData(string enquiryID)
        {

            string sql = "Select *,(select EnumText from  EnumTable where EnumName='EnquiryType' and E.EnquiryType = EnumValue) as EnquiryTypeName,";
            sql += "(select EnumText from  EnumTable where EnumName='EnquiryStatus' and E.Status = EnumValue) as EnquiryStatusName,";
            sql += "(select U.UserName from [Users] as U where E.CreatedBy=U.UserID) as CreatedByName, ";
            sql += "(select U.UserName from [Users] as U where E.ModifiedBy=U.UserID) as ModifiedByName ";
            sql += " from Enquiries E where ID=" + enquiryID.ToString() + " order by ModifiedOn DESC";
            ASM.DbFactory factory = null;
            DataSet ds = new DataSet();
            
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
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
            return ds;
        }
        public DataSet GetEnquiryDetails(string enquiryID)
        {

            string sql = "Select *,(select U.UserName from [Users] as U where E.ModifiedBy=U.UserID) as ModifiedByName from EnquiryDetails E where E.EnquiryID=" + enquiryID.ToString() + " order by E.ModifiedOn ASC";
            ASM.DbFactory factory = null;
            DataSet ds = new DataSet();
             
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
                factory.CreateDataAdapter(sql);
                factory.Adapter.Fill(ds);
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
            return ds;
        }
        public int AddNewEnquiry(EnquiriesData enquiryData)
        {

            string sql = "INSERT into Enquiries (EName,Company,Email,Telephone,EnquiryType,Subject,Message,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,Status";
            if(enquiryData.CallBackDate !=null)
            sql +=",CallBackDate)";
            else
                sql += ")";
            sql += " Values ('" + enquiryData.EName + "','" + enquiryData.Company + "','" + enquiryData.Email + "','" + enquiryData.Telephone + "',";
            sql += enquiryData.EnquiryType + ",'" + enquiryData.Subject + "','" + enquiryData.Message + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "',";
            sql += enquiryData.CreatedBy + "," + enquiryData.ModifiedBy + "," + enquiryData.Status ;
            if (enquiryData.CallBackDate != null)
                sql += ",'" + enquiryData.CallBackDate+"')";
            else
                sql += ")";
            int ID = 0;

            DbFactory factory = null;

            try
            {
                factory = new DbFactory(ENM.ModuleName.Enquiry);
                factory.UseTransaction = true;
                ID = factory.RunCommand(sql);

            }
            catch (System.Data.Common.DbException exc)
            {
                ID = -1;
                AllModules.Errors.LogError(exc);
            }
            catch (Exception exc)
            {
                ID = -1;
                AllModules.Errors.LogError(exc);
            }
            finally
            {
                factory.Close();
            }

            return ID;
        }
        public bool AddEnquiryDetails(string EnquiryID, string sMessage, int userId,int status, string cbDate)
        {

            bool result = false;
            string sql1 = "INSERT into EnquiryDetails (EnquiryID,Message,ModifiedBy,ModifiedOn)";
            sql1 += " Values (" + EnquiryID + ",'" + sMessage + "'," + userId.ToString() + ",'" + DateTime.Now.ToString() + "')";

            string sql2 = "Update Enquiries SET Status=" + status + ", ModifiedBy =" + userId.ToString() + ", ModifiedOn ='" + DateTime.Now.ToString() + "'";
            if (!String.IsNullOrEmpty(cbDate))
                sql2 += ", CallBackDate = '" + cbDate + "'";           
            
            sql2 += " where ID=" + EnquiryID;

            string[] sql = new string[2];
            int[] ID = new int[2];
           
            DbFactory factory = null;
           
            try
            {
                factory = new DbFactory(ENM.ModuleName.Enquiry);
                sql[0] = sql1;
                sql[1] = sql2;               
                factory.UseTransaction = true;
                ID = factory.RuncommandsWithTransaction(sql, false, null);
                result = true;
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

            return result;
        }
        public int[] GetEnquiryDashboardDetails()
        {
            ASM.DbFactory factory = null;
            string sql1= " SELECT count(*) FROM Enquiries WHERE Status=0  ";
            string sql2= " SELECT count(*) FROM Enquiries WHERE Status=1  "; 
            string sql3= " SELECT count(*) FROM Enquiries WHERE Status=2 ";
            string sql4= " SELECT count(*) FROM Enquiries WHERE Status=1 And CallbackDate Is Not Null And Format(CallbackDate,\"Short Date\")<Format(Now(),\"Short Date\")  ";
            string[] queries = new string[] { sql1, sql2,sql3,sql4 };            
            int[] ids = new int[4];            
           
           
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
                ids = factory.RunScalarCommands(queries);
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
            return ids;
        }
        public bool DeleteData(string ID)
        {
            bool status = false;
            string sql1 = "Delete from Enquiries where ID=" + ID;
            string sql2 = "Delete * from [EnquiryDetails] where EnquiryID=" + ID;
            
            string[] sql= new string[2];
            int[] IDs= new int[2] ;
            ASM.DbFactory factory=null;
            try
            {
                factory = new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
                sql[0] = sql1;
                sql[1] = sql2;              
                factory.UseTransaction = true;
                IDs = factory.RuncommandsWithTransaction(sql, false, null);
                status = true;
            }
            catch (Exception exc)
            {
                AllModules.Validate.WriteToEventLog(exc, "Order:DeleteOrder");
            }
            finally
            {
                factory.Close();
            }
            return status;
        
        }
    }
}