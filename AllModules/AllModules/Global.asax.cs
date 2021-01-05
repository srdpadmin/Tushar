using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using ASM = CoreAssemblies;
 


namespace AllModules
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            string connectionStringName = "AccessDbProvider";
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Encrypted"].ToString()))
            {
                EncodeDecode ed = new EncodeDecode();
                connectionString = ed.GetProperConnectionString(connectionString);
            } 
           
            //ASM.DbFactory.PayrollConnectionStringName = connectionString;
            //ASM.DbFactory.PayrollProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
           
            //ASM.DbFactory.QuotationConnectionStringName = connectionString;
            //ASM.DbFactory.QuotationProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
          
            ASM.DbFactory.AccessConnectionStringName = connectionString;
            ASM.DbFactory.AccessProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            // Code moved to accessmembershipprovider initialize
            //EncodeDecode ed = new EncodeDecode();
            //string connectionStringName = "AccessDbProvider";
            //ASM.DbFactory.PayrollConnectionStringName = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            //ASM.DbFactory.PayrollProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            //connectionStringName = "AccessDbProvider";
            //ASM.DbFactory.QuotationConnectionStringName = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            //ASM.DbFactory.QuotationProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
            //connectionStringName = "AccessDbProvider";
            //ASM.DbFactory.AccessConnectionStringName = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            //ASM.DbFactory.AccessProviderName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}