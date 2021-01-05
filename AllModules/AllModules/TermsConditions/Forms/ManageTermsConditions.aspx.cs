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

namespace TermsConditions.Forms
{
    public partial class ManageTermsConditions : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.UrlReferrer != null)
            {
                string url = Request.UrlReferrer.ToString();
                if (url.Contains("Billing"))
                {
                    this.Page.MasterPageFile = "~/Billing/Billing.master";
                    Session["MasterPageFile"] = "~/Billing/Billing.master";
                }
                else if (url.Contains("Quotation"))
                {
                    this.Page.MasterPageFile = "~/Quotation/Quotation.master";
                    Session["MasterPageFile"] = "~/Quotation/Quotation.master";
                }
                else if (url.Contains("Inventory"))
                {
                    this.Page.MasterPageFile = "~/Inventory/Inventory.master";
                    Session["MasterPageFile"] = "~/Inventory/Inventory.master";
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
    }
}
