using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllModules
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            populateErrors();
        }

        public void populateErrors()
        {
            errorGrid.DataSource = Errors.GetLoggedErrors();
            errorGrid.DataBind();
        }

        protected void DeleteAllErrors_Click(object sender, EventArgs e)
        {
            Errors.DeleteLoggedErrors();
            populateErrors();
        }
    }
}
