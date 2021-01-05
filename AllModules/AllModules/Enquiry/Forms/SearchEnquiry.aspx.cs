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
using Authorization.BusLogic;
using System.Collections.Generic;
using Enquiry;

namespace Enquiry.Forms
{
    public partial class SearchEnquiry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ClearBtnClick(object sender, EventArgs e)
        {
            txtID.Text = string.Empty;
            txtCreatedBy.Text = string.Empty;
            txtCompany.Text = string.Empty;
        }
        protected void EnquiryGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            GridViewRow row = EnquiryGridView.SelectedRow;

            if (row != null && e.CommandName != "Page")
            {
                string args = (string)e.CommandArgument;
                string ID = Convert.ToString(EnquiryGridView.DataKeys[row.RowIndex].Value);
                if (args == "View")
                {
                    Response.Redirect("~/Enquiry/Forms/ManageEnquiry.aspx?ID=" + ID);
                }
            }

        }
        protected void EnquiryGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            EnquiryGridView.PageIndex = e.NewPageIndex;
            EnquiryGridView.DataBind();
        }
        protected void SelectButton_Click(object sender, EventArgs e)
        {
            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            EnquiryGridView.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            EnquiryGridView.SelectedIndex = row.RowIndex;
        }

        protected void EnquiryGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row as GridViewRow;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblProductStatus = (Label)e.Row.FindControl("lblProductStatus");
                Label lblEnquiryStatus = (Label)e.Row.FindControl("lblEnquiryStatus");
                Label lblFollowUpStatus = (Label)e.Row.FindControl("lblFollowUpStatus");
                Label lblAssignedTo = (Label)e.Row.FindControl("lblAssignedTo");
              
                lblProductStatus.Text = Enum.GetName(typeof(EnumClass.EnquiryStatus), Convert.ToInt32(lblProductStatus.Text));
                lblEnquiryStatus.Text = Enum.GetName(typeof(EnumClass.EnquiryStatus), Convert.ToInt32(lblEnquiryStatus.Text));
                lblFollowUpStatus.Text = Enum.GetName(typeof(EnumClass.EnquiryStatus), Convert.ToInt32(lblFollowUpStatus.Text));
                UserModule um = new UserModule();
                SortedDictionary<int,string> dict =um.GetUsers();
                lblAssignedTo.Text = dict[Convert.ToInt32(lblAssignedTo.Text)];
            }
            //IDictionary<string, Int32> dict = ConvertStatusEnumToDictionary();

        }
        protected void SearchBtnClick(object sender, EventArgs e)
        {
            EnquiryGridView.DataBind();
        }
        protected void AddNewButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Enquiry/Forms/ManageEnquiry.aspx");
        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            GridViewRow row = EnquiryGridView.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string ID = Convert.ToString(EnquiryGridView.DataKeys[row.RowIndex].Value);
                
                Enquiry.BusLogic.Enquiry ord = new Enquiry.BusLogic.Enquiry();
                int[] IDs = ord.DeleteEnquiry(ID);
                if (IDs[0] > 0)
                {
                    EnquiryGridView.DataBind();
                }
            }
        }
       

    }
}
