using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Authorization
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        // http://www.aspdotnet-suresh.com/2012/01/recover-forgot-password-using.html
        //http://www.aspnetbook.com/apps/design_secure_application_changepassword_control.php
        //Properties in Web.config
        //minRequiredPasswordLength="4" minRequiredNonalphanumericCharacters="0"
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.UrlReferrer != null && !Request.UrlReferrer.ToString().Contains("Authorization"))
            {
                string url = Request.UrlReferrer.ToString();
                if (url.Contains("Payroll"))
                {
                    this.Page.MasterPageFile = "~/Payroll/Payroll.master";
                    Session["MasterPageFile"] = "~/Payroll/Payroll.master";
                }
                else if (url.Contains("Quotation"))
                {
                    this.Page.MasterPageFile = "~/Quotation/Quotation.master";
                    Session["MasterPageFile"] = "~/Quotation/Quotation.master";
                }
                else if (url.Contains("Billing"))
                {
                    this.Page.MasterPageFile = "~/Billing/Billing.master";
                    Session["MasterPageFile"] = "~/Billing/Billing.master";
                }  
            }
            else
            {
                if (Session["MasterPageFile"] != null)
                {
                    this.Page.MasterPageFile = Session["MasterPageFile"].ToString();
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_update_Click(object sender, EventArgs e)
        {
            MembershipProvider currentProvider = System.Web.Security.Membership.Provider;
            MembershipUser user = Membership.GetUser();

            bool valid = currentProvider.ChangePassword(user.UserName, txt_cpassword.Text, txt_npassword.Text);
            if (valid)
            {
                lbl_msg.Text = "Password changed Successfully";
            }
            else
            {
                lbl_msg.Text = "Password change failed.Try entering correct current password.";
            }
        }
    }
}
