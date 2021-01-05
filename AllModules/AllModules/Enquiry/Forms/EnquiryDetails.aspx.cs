using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;

namespace Enquiry.Forms
{
    public partial class EnquiryDetails : System.Web.UI.Page
    {
        private string EnquiryID;
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (Request.QueryString["ID"] != null)
            {
                EnquiryID = Request.QueryString["ID"];
                GetData();
                CompareEndTodayValidator.ValueToCompare = DateTime.Today.ToShortDateString();
            }
            else
            {
                Response.Redirect("Enquiries.aspx");
            }

        }

        public void GetData()
        {
            Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();
            DataSet ds = enquiries.GetData(EnquiryID);
            if (!Convert.IsDBNull(ds.Tables[0].Rows[0]["Status"]) && Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString()) == 2)
            {
                createBtn.Visible = false;
            }
            mainRepeater.DataSource = ds;
            mainRepeater.DataBind();

            childRepeater.DataSource = enquiries.GetEnquiryDetails(EnquiryID);
            childRepeater.DataBind();
        }
        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Enquiry/Forms/Enquiries.aspx");
        }
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            string msg = Message.Text;
            if (msg.Length > 0)
            {
                Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();
                MembershipUser user = Membership.GetUser();
                int UID = Convert.ToInt32(user.ProviderUserKey.ToString());
                try
                {
                    enquiries.AddEnquiryDetails(EnquiryID, msg, UID,Convert.ToInt32(ddlStatus.SelectedValue),txtCB.Text);
                    Message.Text = string.Empty;
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    GetData();
                }


            }
        }
    }
}
