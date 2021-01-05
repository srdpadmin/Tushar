using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ENM = CoreAssemblies.EnumClass;
using AllModules;

namespace Enquiry.Forms
{
    public partial class Enquiries : System.Web.UI.Page
    {
        public bool ShowDelete
        {
            get
            {
                MembershipUser user = Membership.GetUser();
                int UID = Convert.ToInt32(user.ProviderUserKey.ToString());
                if ( AllModules.Validate.UserRoleAccess(UID,ENM.Roles.Admin))
                {
                    return true;
                }
                else
                    return false;
            }
        }
         
        protected void ClearAllText()
        {
            txtIDSearch.Text = string.Empty;
            txtCreatedBySearch.Text = string.Empty;
            txtCompanySearch.Text = string.Empty;
            txtSearch.Text = string.Empty;
            txtFromSearch.Text = string.Empty;
            txtToSearch.Text = string.Empty;

        }
        protected void SearchBtnClick(object sender, EventArgs e)
        {
            GetData();
        }

        protected void ClearBtnClick(object sender, EventArgs e)
        {
            ClearAllText();
            GetData();
        }
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!Page.IsPostBack)
            {
                PopulateDashboardDetails();
                GetData();
            }
            CompareEndTodayValidator.ValueToCompare = DateTime.Today.ToShortDateString();
        }

        public void PopulateDashboardDetails()
        {
            Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();
            int[] ids = enquiries.GetEnquiryDashboardDetails();

            lnkOpen.Text = "Open(" + ids[0] + ")";
            lnkPending.Text = "Pending(" + ids[1] + ")";
            lnkClosed.Text = "Closed(" + ids[2] + ")";
            lnkCallback.Text = "CallBack Required(" + ids[3] + ")";

        }
        public void GetData()
        {
            bool callback = false;
            Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();
            egv.DataSource = enquiries.GetAllEnquiries(txtSearch.Text, txtIDSearch.Text, txtCreatedBySearch.Text, txtCompanySearch.Text, txtFromSearch.Text, txtToSearch.Text,ddlStatusSearch.SelectedValue,callback);
            egv.DataBind();
            Page.DataBind();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {

            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            egv.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            egv.SelectedIndex = row.RowIndex;

        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            GridViewRow row = egv.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string ID = Convert.ToString(egv.DataKeys[row.RowIndex].Value);
                Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();

                if (enquiries.DeleteData(ID))
                {
                    GetData();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Enquiry deleted successfully" + "');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Problem deleting Enquiry" + "');", true);

                }
            }
        }
        protected void egv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            egv.PageIndex = e.NewPageIndex;
            GetData();
        }
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {

            string msg = txtMessage.Text;
            if (msg.Length > 0)
            {
                Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();
                Enquiry.Data.EnquiriesData enqData = new Enquiry.Data.EnquiriesData();
                MembershipUser user = Membership.GetUser();
                int UID = Convert.ToInt32(user.ProviderUserKey.ToString());
                try
                {
                    enqData.EName=txtName.Text;
                    enqData.Company = txtCompany.Text;
                    enqData.Email=txtEmail.Text;
                    enqData.Telephone=txtPhone.Text;
                    enqData.EnquiryType=Convert.ToInt32(ddlList.SelectedValue);
                    enqData.Subject=txtSubject.Text;
                    enqData.Message = msg;
                    enqData.CreatedBy = UID;
                    enqData.ModifiedBy = UID;
                    enqData.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                    if (!String.IsNullOrEmpty(txtCB.Text))
                        enqData.CallBackDate = Convert.ToDateTime(txtCB.Text);
                    else
                        enqData.CallBackDate = null;
                    enquiries.AddNewEnquiry(enqData);
                     
                    txtName.Text = string.Empty;
                    txtCompany.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtPhone.Text = string.Empty;
                    txtSubject.Text = string.Empty;
                    txtMessage.Text = string.Empty;
                    txtCB.Text = string.Empty;
                    
                }
                catch (Exception exc)
                {
                    Errors.LogError(exc);
                }
                finally
                {
                    GetData();

                }


            }
        }

        public void PopulateDataByDashboard(string status,bool callback)
        {
            Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();
            egv.DataSource = enquiries.GetAllEnquiries(null, null, null, null, null, null, status,callback);
            egv.DataBind();
        }

        protected void lnkOpen_Click(object sender, EventArgs e)
        {
            PopulateDataByDashboard("0",false);
        }

        protected void lnkPending_Click(object sender, EventArgs e)
        {
            PopulateDataByDashboard("1", false);
        }

        protected void lnkClosed_Click(object sender, EventArgs e)
        {
            PopulateDataByDashboard("2", false);
        }

        protected void lnkCallback_Click(object sender, EventArgs e)
        {
            PopulateDataByDashboard("1", true);
        }
    }
}
