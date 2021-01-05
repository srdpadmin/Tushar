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
using AllModules;
using CoreAssemblies;


namespace Quotation
{
    public partial class Site : System.Web.UI.MasterPage
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
                if (!Validate.UserModuleAccess(Convert.ToInt32(user.ProviderUserKey), EnumClass.Modules.Quotation)) //)
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
        }
        protected void lnkCreateOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Quotation/Forms/ManageQuote.aspx");

        }
        protected void lnkSearchOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Quotation/Forms/SearchQuotes.aspx");
        }
        protected void lnkSearchVendor_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Contact/Forms/ManageContacts.aspx?Quotation");
        }
        protected void lnkManageTermsConditions_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/TermsConditions/Forms/ManageTermsConditions.aspx?Quotation");
        }
        protected void lnkProductMaster_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Inventory/Forms/ManageProductMaster.aspx?Quotation");
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
                Errors.LogError(exc);

            }

        }
    }
}
