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
using ENM=CoreAssemblies.EnumClass;
using Contact.Data;

namespace Contact.Controls
{
    public partial class Contacts : System.Web.UI.UserControl
    {
        private ENM.ContactType contact;
        public ENM.ContactType ContactType
        {
            get { return contact; }
            set { contact = value; }
        }
        private string contactID;
        public string ContactID
        {
            get { return contactID; }
            set { contactID = value; }
        }

        private bool isPopup;
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }
        private bool IsUCPostBack
        {
            get
            {
                object o = ViewState["S2UC"];
                return o == null;
            }
            set
            {
                ViewState["S2UC"] = true;
            }
        }
        public event EventHandler ContactSelected;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            Status.Text = string.Empty;
            if (!Page.IsPostBack)
            {
                ToggleBetweenDetailsGridView(true);
                
            }
            if (isPopup && IsUCPostBack)
            {
                IsUCPostBack = true;
                ToggleBetweenDetailsGridView(true);
            }
             
            up.Update();
        }

         
        protected void ToggleAmendInsertVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Style.Add("display", "none");
                btnPrint.Style.Add("display", "none");
                btnUpdate.Style.Add("display", "none");
                btnInsert.Style.Add("display", "block");
                btnCancel.Style.Add("display", "block");
            }
            else
            {
                btnPrint.Style.Add("display", "block");
                btnAmend.Style.Add("display", "block");
                btnInsert.Style.Add("display", "none");
                btnCancel.Style.Add("display", "block");
                btnUpdate.Style.Add("display", "none");

                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    btnPrint.Attributes.Add("onclick", "PrintReportInNewWindow(" + Request.QueryString["ID"] + "); return false;");
                }
            }

        }
        protected void ToggleAmendUpdateVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Style.Add("display", "block");
                btnUpdate.Style.Add("display", "none");

            }
            else
            {
                btnAmend.Style.Add("display", "none");
                btnUpdate.Style.Add("display", "block");

            }

        }
        protected void ToggelInsertUpdateBtnVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnInsert.Style.Add("display", "block");
                btnCancel.Style.Add("display", "block");
                btnUpdate.Style.Add("display", "none");            
            }
            else
            {
                btnInsert.Style.Add("display", "block"); ;
                btnCancel.Style.Add("display", "block");
                btnUpdate.Style.Add("display", "block");
            }

        }

        protected void ToggleBetweenDetailsGridView(Boolean bMode)
        {
            if (bMode == true)
            {
                InsertUpdateCancelBtnPanel.Style.Add("display", "none");
               
                ItemsBtnPanel.Style.Add("display", "block");
                ContactListPanel.Style.Add("display", "block");
                
            }
            else
            {
                InsertUpdateCancelBtnPanel.Style.Add("display", "block");
                
                ItemsBtnPanel.Style.Add("display", "none");
                ContactListPanel.Style.Add("display", "none");
            }
        }
        protected void ToggleEnableDisableMode(Boolean bMode)
        {
            ddlContactType.Enabled = bMode;
            CompanyName.Enabled = bMode;
            FirstName.Enabled = bMode;
            MiddleName.Enabled = bMode;
            LastName.Enabled = bMode;
            Address1.Enabled = bMode;
            Address2.Enabled = bMode;
            Address3.Enabled = bMode;
            City.Enabled = bMode;
            State.Enabled = bMode;
            PinCode.Enabled = bMode;
            HomePhone.Enabled = bMode;
            WorkPhone.Enabled = bMode;
            Fax.Enabled = bMode;
            Email.Enabled = bMode;
        }

        protected void ActionItemBtnPanelVisible(Boolean mode)
        {
            if (mode)
            {
                ItemsBtnPanel.Visible = true;
            }
            else
            {
                ItemsBtnPanel.Visible = false;
            }
        }       
        
        protected void SelectButton_Click(object sender, EventArgs e)
        {
            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            ContactGridView.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            ContactGridView.SelectedIndex = row.RowIndex;
            contactID=ContactGridView.DataKeys[row.RowIndex].Value.ToString();
            if (ContactSelected != null)
            {
                ContactSelected(this, new EventArgs());
            }
        }

        public ContactsData CopyUIToObject()
        {
            ContactsData cd = new ContactsData();
            cd.ID = ID.Value;
            cd.ContactType = Convert.ToInt32(ddlContactType.SelectedValue);
            cd.Company = CompanyName.Text;
            cd.FirstName = FirstName.Text;
            cd.LastName = LastName.Text;
            cd.MiddleName = MiddleName.Text;
            cd.Address1 = Address1.Text;
            cd.Address2 = Address2.Text;
            cd.Address3 = Address3.Text;
            cd.City = City.Text;
            cd.State = State.Text;
            cd.PinCode = PinCode.Text;
            cd.Phone1 = HomePhone.Text;
            cd.Phone2 = WorkPhone.Text;
            cd.Fax = Fax.Text;
            cd.Email = Email.Text;
            return cd;

        }
        public void CopyObjectToUI(ContactsData cd)
        {
            ID.Value = cd.ID;
            ddlContactType.SelectedValue = (cd.ContactType == 0)? "3" : cd.ContactType.ToString();
            CompanyName.Text = cd.Company;
            FirstName.Text = cd.FirstName;
            LastName.Text = cd.LastName;
            MiddleName.Text = cd.MiddleName;
            Address1.Text = cd.Address1;
            Address2.Text = cd.Address2;
            Address3.Text = cd.Address3;
            City.Text = cd.City;
            State.Text = cd.State;
            PinCode.Text = cd.PinCode;
            HomePhone.Text = cd.Phone1;
            WorkPhone.Text = cd.Phone2;
            Fax.Text = cd.Fax;
            Email.Text = cd.Email;
        }

        #region Insert Update Amend Cancel 
        protected void btnAmend_Click(object sender, EventArgs e)
        {
             
            ToggleAmendUpdateVisible(false);
            ToggleEnableDisableMode(true);
            up.Update();
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            ContactsData cd = CopyUIToObject();
            
            Contact.BusLogic.Contacts cs = new Contact.BusLogic.Contacts();
            if (cs.InsertContact(cd)>0)
            {
                Status.Text = "Record successfully Added.";
                ToggleEnableDisableMode(false);
                btnInsert.Style.Add("display", "none");
            }
            else
            {
                Status.Text = "Error adding a new record.";
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            ContactsData cd = CopyUIToObject();
            Contact.BusLogic.Contacts cs = new Contact.BusLogic.Contacts();
            if (cs.UpdateContact(cd)>0)
            {
                Status.Text = "Record successfully Updated.";
                btnUpdate.Style.Add("display", "none");
                ToggleEnableDisableMode(false);
                ID.Value = string.Empty;
            }
            else
            {
                Status.Text = "Error updating record.";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ToggleBetweenDetailsGridView(true);
            ContactGridView.DataBind();
            up.Update();
        }
        #endregion

       

        #region Image Button Click Events
        protected void imgAddItem_Click(object sender, EventArgs e)
        {

            ToggleBetweenDetailsGridView(false);
            ToggelInsertUpdateBtnVisible(true);
            ToggleEnableDisableMode(true); //enabling all textboxes


        }
        protected void imgEditItem_Click(object sender, EventArgs e)
        {
             GridViewRow row = ContactGridView.SelectedRow;
             if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
             {
                 string sID = Convert.ToString(ContactGridView.DataKeys[row.RowIndex].Value);
                 ID.Value = sID;
                 ToggleBetweenDetailsGridView(false);
                 Contact.BusLogic.Contacts cs = new Contact.BusLogic.Contacts();
                 ContactsData CD = new ContactsData();
                 CD = cs.GetContactsByID(sID);
                 if (CD != null)
                 {
                     CopyObjectToUI(CD);
                     ToggleEnableDisableMode(false);
                     ToggleAmendInsertVisible(false);
                 }

             }
             
        }
        protected void imgDeleteItem_Click(object sender, EventArgs e)
        {
            GridViewRow row = ContactGridView.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string sID = Convert.ToString(ContactGridView.DataKeys[row.RowIndex].Value);
                Contact.BusLogic.Contacts cs = new Contact.BusLogic.Contacts();
                if (cs.DeleteContact(sID) > 0)
                {
                    ContactGridView.DataBind();
                }

            }
        }
         
        #endregion
       
    }
}