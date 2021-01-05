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

namespace AllModules.Payroll
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
                if (!Validate.UserModuleAccess(Convert.ToInt32(user.ProviderUserKey),EnumClass.Modules.Payroll)) //)
                {
                    Response.Redirect("~/Default.aspx");
                }
            }

        }
        protected void lnkSearchEmployees_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Payroll/Forms/SearchEmployees.aspx");

        }
        protected void lnkSalaryInformation_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Payroll/Forms/SalaryInformation.aspx?Selection=1");

        }
        protected void lnkReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Payroll/Forms/Reports.aspx?Selection=2");
        }
        protected void lnkConfig_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Payroll/Forms/Configuration.aspx");
        }
    }
}
