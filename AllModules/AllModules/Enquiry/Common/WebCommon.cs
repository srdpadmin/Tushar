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
using System.Collections;
using Enquiry.BusLogic;
using CoreAssemblies;
using ENM = CoreAssemblies.EnumClass;


namespace Enquiry.Common
{
    public class WebCommon
    {
        public static object GetAllContactsFromCache()
        {
            return Contact.Common.WebCommon.GetAllContactsFromCache();
        }
        public static int InsertEnquiryToDbUsingTransaction(ArrayList EnquirySql)
        {

            string[] queries = (string[])EnquirySql.ToArray(typeof(string));
            DbFactory factory =null;
            int[] ids = new int[queries.Length];
            try
            {
                factory = new DbFactory(ENM.ModuleName.Enquiry);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, true, "EnquiryID");

            }
            catch (Exception exc)
            {
                ids[1] = -1;
            }
            finally
            {
                factory.Close();
            }
            return ids[1];

        }

        public static int[] UpdateEnquiryToDbUsingTransaction(ArrayList EnquirySql)
        {
            string[] queries = (string[])EnquirySql.ToArray(typeof(string));
            DbFactory factory =null;
            int[] ids = new int[queries.Length];
            try
            {
                factory= new CoreAssemblies.DbFactory(ENM.ModuleName.Enquiry);
                factory.UseTransaction = true;
                ids = factory.RuncommandsWithTransaction(queries, false, "EnquiryID");
                 
            }
            catch (Exception exc)
            {
                ids[1] = -1;
            }
            finally
            {
                factory.Close();
            }
            return ids;

        }
    }
}