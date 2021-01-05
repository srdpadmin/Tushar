using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AllModules;
using LogicNP.CryptoLicensing;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using AllModules.License;

namespace License
{
    public partial class ManageLicense : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {              
           

            if (!Page.IsPostBack)
            {                
              Checklicense();
            } 
            
           
        }
        public void Checklicense()
        {
              CustomCryptoLicense ccl = new CustomCryptoLicense();
              MachineKey.Text = ccl.LocalMachineCode;
              LicenseKey.Text = ccl.LicenseKey ;
                 if (ccl.LicenseValid )
                {
                    lblLicenseStatus.Text = "License Status: " + ccl.Status.ToString();              
                }
                else
                {
                    lblLicenseStatus.Text = "License not found"; 
                }
        }
        string GetAllStatusExceptionsAsString(CryptoLicense license)
        {
            LicenseStatus[] status = (LicenseStatus[])Enum.GetValues(typeof(LicenseStatus));
            StringBuilder sb = new StringBuilder();
            foreach (LicenseStatus ls in status)
            {
                Exception ex = license.GetStatusException(ls);
                if (ex != null) // Additional info available for the status
                {
                    if (sb.Length > 0)
                        sb.Append("\n");

                    sb.Append(ls.ToString());
                    sb.Append(": ");
                    sb.Append(ex.Message);
                }
            }

            return sb.ToString();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (LicenseKey.Text != string.Empty)
            {
                AllModules.License.LicenseLogic  ll = new AllModules.License.LicenseLogic();
                LicenseData ld = ll.GetLicense();
                ld.LicenseKey = LicenseKey.Text.Trim();
                ll.UpdateLicense(ld);
                Checklicense();
            }
        }
        protected void Login_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}
