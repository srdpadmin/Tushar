using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Enquiry.Controls
{
    public partial class ucEnquiryDashboard : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateDashboardDetails();
            }
        }

        public void PopulateDashboardDetails()
        {
            Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();
            int[] ids = enquiries.GetEnquiryDashboardDetails();
            
            open.Text = "Open(" + ids[0] +")";
            pending.Text = "Pending(" + ids[1] + ")";
            closed.Text = "Closed(" + ids[2] + ")";
            callback.Text = "CallBack Required(" + ids[3] + ")";
             
        }
    }
}