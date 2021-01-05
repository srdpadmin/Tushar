using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;

namespace Authorization
{
    public partial class Login : System.Web.UI.Page
    {       
        protected void Page_Load(object sender, EventArgs e)
        {
               
            AllModules.CustomCryptoLicense ccl = new AllModules.CustomCryptoLicense();
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Encrypted"].ToString()))
            {
                if(!ccl.LicenseValid)
                Response.Redirect("~/License/ManageLicense.aspx");
            }
            if (User.Identity.IsAuthenticated)
            {
               
                //    Response.Redirect("~/Authorization/Login.aspx?returnUrl=" + Request.Url.LocalPath);
                if (Request.Params["ReturnUrl"] != null && !Request.Params["ReturnUrl"].Contains("Default.aspx"))
                {
                    Response.Redirect(Request.Params["ReturnUrl"].ToString());
                }
                else
                {
                    //Response.Redirect("../" + ConfigurationManager.AppSettings["mykey"].ToString() + "Default.aspx");
                    Response.Redirect("~/Default.aspx");
                }
                
                
            }
        }
    }
}
