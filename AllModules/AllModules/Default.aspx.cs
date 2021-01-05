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

namespace AllModules
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Session.Contents.RemoveAll();
                Response.Redirect("~/Authorization/Login.aspx");
            }
           
            
        }

        protected void QO_Click(object sender, EventArgs e)
        {
            Session.Contents.RemoveAll();
            Response.Redirect("~/Quotation/Forms/SearchOrder.aspx");
        }
        protected void SM_Click(object sender, EventArgs e)
        {
            Session.Contents.RemoveAll();
            Response.Redirect("~/Payroll/Forms/SearchEmployees.aspx");
        }
    }
}
