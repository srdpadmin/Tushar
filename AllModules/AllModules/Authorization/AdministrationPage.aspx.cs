using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using CoreAssemblies;

namespace Authorization
{
    public partial class AdministrationPage : System.Web.UI.Page
    {
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
            if (Request.IsAuthenticated )//&& !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                MembershipUser user = Membership.GetUser();
                if (!AllModules.Validate.UserRoleAccess(Convert.ToInt32(user.ProviderUserKey), EnumClass.Roles.Admin))
                {
                Response.Redirect("~/Default.aspx");
                }
                else
                {
                    string sel = Request.QueryString["Selection"];
                    if (!string.IsNullOrEmpty(sel))
                    {
                        switch (sel)
                        {
                            //case "1"://create user
                            //    createuser.Visible = true;
                            //    manageroles.Visible = false;
                            //    manageuser.Visible = false;
                            //    break;
                            //case "2"://manage users
                            //    createuser.Visible = false;
                            //    manageroles.Visible = false;
                            //    manageuser.Visible = true;
                            //    break;
                            //case "3"://manage roles
                            //    createuser.Visible = false;
                            //    manageroles.Visible = true;
                            //    manageuser.Visible = false;
                            //    break;
                            default:
                                createuser.Visible = true;
                                //manageroles.Visible = false;
                                //manageuser.Visible = false;
                                break;

                        }
                    }
                }
            }

            //or use the web.config way to avoid user on the pages
            // just dont show them the links
        }

        
    }
}
