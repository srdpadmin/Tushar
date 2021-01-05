using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq; 
using CoreAssemblies;


namespace AllModules.Enquiry
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Authorization/Login.aspx");
            }
            else
            {
                MembershipUser user = Membership.GetUser();
                if (!Validate.UserModuleAccess(Convert.ToInt32(user.ProviderUserKey), EnumClass.Modules.Enquiry)) //)
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        protected void lnkEnquiries_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Enquiry/Forms/Enquiries.aspx");
        }

        protected void lnkDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Enquiry/Forms/EnquiryDashboard.aspx");
        }
        
        
        protected void lnkCreateEnquiry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Enquiry/Forms/ManageEnquiry.aspx");

        }
        protected void lnkSearchEnquiry_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Enquiry/Forms/SearchEnquiry.aspx");
        }
        protected void lnkSearchVendor_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Contact/Forms/ManageContacts.aspx?Enquiry");
        }
        protected void lnkManageTermsConditions_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TermsConditions/Forms/ManageTermsConditions.aspx?Enquiry");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();
            }
            catch (Exception exc)
            {


            }

        }
    }
}
