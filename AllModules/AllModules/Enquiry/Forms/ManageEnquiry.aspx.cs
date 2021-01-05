using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Billing.Data;
using WC = Enquiry.Common.WebCommon;
using System.Collections;
using Billing.BusLogic ;
using Enquiry.BusLogic;
using System.Data;
using ASM = CoreAssemblies;
using System.Web.Security;
using Enquiry.Data;
using Authorization.BusLogic;
using CoreAssemblies;

namespace Enquiry.Forms
{
    public partial class ManageEnquiry : System.Web.UI.Page
    {
        #region Private Variables

        EnquiryData oData = null;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // case of insert or edit
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {   //case of amend
                    ToggleAmendInsertVisible(false);
                    ActionItemBtnVisible(false);
                    ActionTermBtnVisible(false);
                    ActionItemBtnPanelVisible(false);
                    ActionTermBtnPanelVisible(false);
                    ODView.DataBind();
                    PopulateItemsGrid();
                    PopulateTermsGrid();
                    DUP.Update();
                    ITUP.Update();
                }
                else
                {
                    //case of insert

                    ToggleAmendInsertVisible(true);
                    ActionItemBtnVisible(false);
                    ActionTermBtnVisible(false);
                    ODView.ChangeMode(DetailsViewMode.Insert);
                    ClearViewState();
                    PopulateItemsGrid();
                    PopulateTermsGrid();
                    ActionItemBtnPanelVisible(true);
                    ActionTermBtnPanelVisible(true);
                    DUP.Update();
                    ITUP.Update();

                }
            }
            else
            {
                if (ODView.CurrentMode == DetailsViewMode.Edit || ODView.CurrentMode == DetailsViewMode.Insert)
                {
                    string ControlName = ODView.CurrentMode == DetailsViewMode.Edit ? "Econtacts" : "contacts";
                    Contact.Controls.Contacts uc = ODView.Rows[0].FindControl(ControlName) as Contact.Controls.Contacts;
                    if (uc != null)
                    {
                        uc.ContactSelected += new EventHandler(uc_ContactSelected);

                    }
                }
            }

        }

        void ClearViewState()
        {
            ViewState["Items"] = null;
            ViewState["Terms"] = null;
            ViewState["ItemKey"] = null;
            ViewState["TermKey"] = null;
        }

        void uc_ContactSelected(object sender, EventArgs e)
        {
            Contact.Controls.Contacts uc = sender as Contact.Controls.Contacts;
            if (!String.IsNullOrEmpty(uc.ContactID))
            {
                Label hdnCustomerId = ODView.Rows[0].FindControl("hdnCustomerID") as Label;
                hdnCustomerId.Text = uc.ContactID;
                PopulateCustomerAddress();
            }
        }

        protected void PopulateCustomerAddress()
        {
            DetailsView view = ODView;
            DetailsViewRowCollection rows = view.Rows;
            if (rows != null && rows.Count>0)
            {
                DetailsViewRow row = rows[0];
                string custIDFieldName = ODView.CurrentMode == DetailsViewMode.Insert ? "txtCustomer" : "EtxtCustomer";
                Label CustID = row.FindControl("hdnCustomerID") as Label;
                if (CustID != null && CustID.Text.Length > 0 && CustID.Text != "0")
                {
                    Contact.BusLogic.Contacts cs = new Contact.BusLogic.Contacts();
                    Contact.Data.ContactsData cd = cs.GetContactsByID(CustID.Text);
                    if (cd.Company.Length > 0 || cd.FirstName.Length > 0)
                    {
                        Label CustomerRow1 = row.FindControl("CustomerRow1") as Label;
                        Label CustomerRow2 = row.FindControl("CustomerRow2") as Label;
                        Label CustomerRow3 = row.FindControl("CustomerRow3") as Label;

                        string CustomerName = string.Empty;
                        if (cd.Company.Length > 0)
                        {
                            CustomerName = cd.Company;
                            if (cd.FirstName.Length > 0)
                            {
                                CustomerName += " (" + cd.FirstName + " " + cd.MiddleName + " " + cd.LastName + ")";
                            }
                        }
                        else
                        {
                            CustomerName = cd.FirstName + " " + cd.MiddleName + " " + cd.LastName;
                        }
                        if (ODView.CurrentMode == DetailsViewMode.Insert || ODView.CurrentMode == DetailsViewMode.Edit)
                        {
                            LinkButton lnkName = row.FindControl(custIDFieldName) as LinkButton;
                            lnkName.Text = CustomerName;
                        }
                        else
                        {
                            Label lblName = row.FindControl("txtCustomer") as Label;
                            lblName.Text = CustomerName;
                        }
                        CustomerRow1.Text = cd.Address1 + " " + cd.Address2;
                        CustomerRow2.Text = cd.Address3 + " " + cd.City + " " + cd.State + " " + cd.PinCode;
                        CustomerRow3.Text = "Phone: " + cd.Phone1 + " / " + cd.Phone2;

                    }
                }
                else
                {
                    if (ODView.CurrentMode == DetailsViewMode.Edit || ODView.CurrentMode == DetailsViewMode.Insert)
                    {
                        LinkButton lnkName = row.FindControl(custIDFieldName) as LinkButton;
                        lnkName.Text = "Add New Customer";
                    }
                }
               

            }
        }

        protected void CustomerChanged(object sender, EventArgs e)
        {

            try
            {
                Hashtable vTable = (Hashtable)WC.GetAllContactsFromCache();
                TextBox A = (TextBox)sender;
                if (vTable.Count > 0)
                {
                    Label CustomerID = (Label)ODView.FindControl("lblCustomerID");
                    foreach (DictionaryEntry pair in vTable)
                    {
                        if (pair.Value.ToString().ToLower().Contains(A.Text.ToLower()))
                        {
                            CustomerID.Text = pair.Key.ToString();
                            break;

                        }
                    }
                }
                else
                    throw new Exception("Failed to gather Customer List.Please close this browser and restart.");
            }
            catch (Exception exc)
            {

            }
        }

        protected void ODView_DataBound(object sender, EventArgs e)
        {
            PopulateDropDown();
            PopulateCustomerAddress();
            PopulateDropdownReadOnly();
        }
        public void PopulateDropdownReadOnly()
        {
            if (ODView.CurrentMode == DetailsViewMode.ReadOnly )
            {
                DetailsView view = ODView;
                DetailsViewRowCollection rows = view.Rows;
                if (rows != null && rows.Count > 0)
                {
                    DetailsViewRow row = rows[0];
                    Label lblProductStatus = (Label)row.FindControl("StatusLbl");
                    Label lblEnquiryStatus = (Label)row.FindControl("EnStatusLbl");
                    Label lblFollowUpStatus = (Label)row.FindControl("FollowUpLbl");
                    Label lblAssignedTo = (Label)row.FindControl("AssignedToLbl");


                    lblProductStatus.Text = Enum.GetName(typeof(EnumClass.EnquiryStatus), Convert.ToInt32(lblProductStatus.Text));
                    lblEnquiryStatus.Text = Enum.GetName(typeof(EnumClass.EnquiryStatus), Convert.ToInt32(lblEnquiryStatus.Text));
                    lblFollowUpStatus.Text = Enum.GetName(typeof(EnumClass.EnquiryStatus), Convert.ToInt32(lblFollowUpStatus.Text));
                    UserModule um = new UserModule();
                    SortedDictionary<int, string> dict = um.GetUsers();
                    lblAssignedTo.Text = dict[Convert.ToInt32(lblAssignedTo.Text)];
                }
            }
        }
        public void PopulateDropDown()
        {
            if (ODView.CurrentMode == DetailsViewMode.Edit ||
               ODView.CurrentMode == DetailsViewMode.Insert)
            {
                DetailsView view = ODView;
                DetailsViewRowCollection rows = view.Rows;
                if (rows != null && rows.Count > 0)
                {
                    DetailsViewRow row = rows[0];
                    DropDownList ddlEnquiryStatus = row.FindControl("ddlEnquiryStatus") as DropDownList;
                    DropDownList ddlProductStatus = row.FindControl("ddlProductStatus") as DropDownList;
                    DropDownList ddlFollowUpStatus = row.FindControl("ddlFollowUpStatus") as DropDownList;
                    DropDownList ddlAssignedTo  =row.FindControl("ddlAssignedTo") as DropDownList;
                    IDictionary<string, Int32> dict = ConvertStatusEnumToDictionary();
                    ddlEnquiryStatus.DataSource = dict;
                    ddlEnquiryStatus.DataTextField = "Key";
                    ddlEnquiryStatus.DataValueField = "Value";
                   
                    ddlProductStatus.DataSource = dict;
                    ddlProductStatus.DataTextField = "Key";
                    ddlProductStatus.DataValueField = "Value";
                    
                    ddlFollowUpStatus.DataSource = dict;
                    ddlFollowUpStatus.DataTextField = "Key";
                    ddlFollowUpStatus.DataValueField = "Value";

                    UserModule um = new UserModule();
                    ddlAssignedTo.DataSource =  um.GetUsers();
                    ddlAssignedTo.DataTextField = "Value";
                    ddlAssignedTo.DataValueField = "Key";
                   
                   
                    if (ODView.CurrentMode == DetailsViewMode.Edit)
                    {
                        HiddenField hdnEnquiryStatus = row.FindControl("hdnEnquiryStatus") as HiddenField;
                        HiddenField hdnProductStatus = row.FindControl("hdnProductStatus") as HiddenField;
                        HiddenField hdnFollowUpStatus = row.FindControl("hdnFollowUpStatus") as HiddenField;
                        HiddenField hdnAssignedTo = row.FindControl("hdnAssignedTo") as HiddenField;

                        ddlAssignedTo.SelectedValue = hdnAssignedTo.Value;
                        ddlEnquiryStatus.SelectedValue = hdnEnquiryStatus.Value;
                        ddlProductStatus.SelectedValue =  hdnProductStatus.Value;
                        ddlFollowUpStatus.SelectedValue =  hdnFollowUpStatus.Value;
                    }
                    ddlEnquiryStatus.DataBind();
                    ddlProductStatus.DataBind();
                    ddlFollowUpStatus.DataBind();
                    ddlAssignedTo.DataBind();
                }
            }
        }

        public  IDictionary<String, Int32> ConvertStatusEnumToDictionary()
        {
            if (typeof(ASM.EnumClass.EnquiryStatus).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            return Enum.GetValues(typeof(ASM.EnumClass.EnquiryStatus)).Cast<int>().ToDictionary(currentItem => Enum.GetName(typeof(ASM.EnumClass.EnquiryStatus), currentItem));
        }
        #region Bill Details

        protected void ToggleAmendInsertVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Visible = false;
                btnPDFPrint.Visible = false;
                btnExcelPrint.Visible = false;
                btnUpdate.Visible = false;
                btnInsert.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                btnPDFPrint.Visible = true;
                btnExcelPrint.Visible = true;
                btnAmend.Visible = true;
                btnInsert.Visible = false;
                btnCancel.Visible = true;
                btnUpdate.Visible = false;
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {                   
                    btnPDFPrint.Attributes.Add("onclick", "PrintReportInNewWindow(" + Request.QueryString["ID"] + ",'PDF'); return false;");
                    btnExcelPrint.Attributes.Add("onclick", "PrintReportInNewWindow(" + Request.QueryString["ID"] + ",'EXCEL'); return false;");
                
                }
            }

        }

        protected void ToggleAmendUpdateVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Visible = true;
                btnUpdate.Visible = false;

            }
            else
            {
                btnAmend.Visible = false;
                btnUpdate.Visible = true;
            }

        }

        protected void btnAmend_Click(object sender, EventArgs e)
        {
            ToggleAmendUpdateVisible(false);
            ActionItemBtnPanelVisible(true);
            ActionTermBtnPanelVisible(true);
            ODView.ChangeMode(DetailsViewMode.Edit);
            ODView.DataBind();
            DUP.Update();
            ITUP.Update();

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            bool itemInsert = true;
            bool termInsert = true;
            // to check if any of the Items or Terms rows are in edit mode?
            foreach (GridViewRow GRow in ItemsGridView.Rows)
            {
                if ((GRow.RowState & DataControlRowState.Edit) > 0)
                {
                    itemInsert = false;
                }
            }

            foreach (GridViewRow GRow in TermsGridView.Rows)
            {
                if ((GRow.RowState & DataControlRowState.Edit) > 0)
                {
                    termInsert = false;
                }
            }
            if (itemInsert || termInsert)
            {
                // DView.InsertItem will only get the current PO object on UI into session

                EnquiryData oData = CopyUIToBillDataObject();
                DataSet dsItems = (DataSet)ViewState["Items"];
                DataSet dsTerms = (DataSet)ViewState["Terms"];
                ArrayList sqlCommands = new ArrayList();
                string updateSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "UPDATE", "Enquiry");
                sqlCommands.Add(updateSql);
                if (dsItems != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new EnquiryItemData(), dsItems.Tables[0], "EnquiryItem"));
                if (dsTerms != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new EnquiryTermData(), dsTerms.Tables[0], "EnquiryTerm"));

                int[] x = WC.UpdateEnquiryToDbUsingTransaction(sqlCommands);
                ClearViewState();
                if (x[0] < 0)
                {
                    //Error

                }
                else
                {
                    ToggleAmendUpdateVisible(true);
                    ODView.ChangeMode(DetailsViewMode.ReadOnly);
                    ActionItemBtnVisible(false);
                    ActionTermBtnVisible(false);
                    ActionItemBtnPanelVisible(false);
                    ActionTermBtnPanelVisible(false);
                    PopulateItemsGrid();
                    PopulateTermsGrid();
                    ODView.DataBind();
                    DUP.Update();
                    ITUP.Update();
                }
               
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (ODView.CurrentMode == DetailsViewMode.Edit)
            {
                ToggleAmendUpdateVisible(true);
            }
            else if (ODView.CurrentMode == DetailsViewMode.Insert)
            {
                Response.Redirect("~/Enquiry/Forms/SearchEnquiry.aspx");
            }
            else if (ODView.CurrentMode == DetailsViewMode.ReadOnly)
            {
                Response.Redirect("~/Enquiry/Forms/SearchEnquiry.aspx");
            }

            ODView.DataBind();
            ODView.ChangeMode(DetailsViewMode.ReadOnly);
            PopulateItemsGrid();
            PopulateTermsGrid();
            ActionItemBtnPanelVisible(false);
            ActionTermBtnPanelVisible(false);
            ITUP.Update();
            DUP.Update();
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            bool itemInsert = true;
            bool termInsert = true;

            // to check if any of the Items or Terms rows are in edit mode?
            foreach (GridViewRow GRow in ItemsGridView.Rows)
            {
                if ((GRow.RowState & DataControlRowState.Edit) > 0)
                {
                    itemInsert = false;
                }
            }

            foreach (GridViewRow GRow in TermsGridView.Rows)
            {
                if ((GRow.RowState & DataControlRowState.Edit) > 0)
                {
                    termInsert = false;
                }
            }

            if (itemInsert && termInsert)
            {
                EnquiryData oData = CopyUIToBillDataObject();
                //todo set createdby for oData to userid
               
                DataSet dsItems = (DataSet)ViewState["Items"];
                DataSet dsTerms = (DataSet)ViewState["Terms"];
                string insertSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "INSERT", "Enquiry");
                string identitySql = "Select @@Identity";
                ArrayList sqlCommands = new ArrayList();
                sqlCommands.Add(insertSql);
                sqlCommands.Add(identitySql);
                if(dsItems !=null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new EnquiryItemData(), dsItems.Tables[0], "EnquiryItem"));
                if (dsTerms != null)
                sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new EnquiryTermData(), dsTerms.Tables[0], "EnquiryTerm"));

                int x = WC.InsertEnquiryToDbUsingTransaction(sqlCommands);
                ViewState["Items"] = null;
                ViewState["Terms"] = null;
                ViewState["ItemKey"] = null;

                if (x < 0)
                {
                    //Error
                }
                else
                {
                    Response.Redirect("~/Enquiry/Forms/ManageEnquiry.aspx?ID=" + x.ToString());
                    ODView.DataBind();
                }
              
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        public EnquiryData CopyUIToBillDataObject()
        {
            int temp = 0;
            Label CustomerId = ODView.Rows[0].FindControl("hdnCustomerID") as Label;            
            TextBox productSuggested = ODView.Rows[0].FindControl("txtProductSuggested") as TextBox;
            DropDownList ddlEnquiryStatus = ODView.Rows[0].FindControl("ddlEnquiryStatus") as DropDownList;
            DropDownList ddlProductStatus = ODView.Rows[0].FindControl("ddlProductStatus") as DropDownList;
            DropDownList ddlFollowUpStatus = ODView.Rows[0].FindControl("ddlFollowUpStatus") as DropDownList;
            DropDownList ddlAssignedTo = ODView.Rows[0].FindControl("ddlAssignedTo") as DropDownList;

            TextBox estimation = ODView.Rows[0].FindControl("txtEstimation") as TextBox;
            TextBox assignedTo = ODView.Rows[0].FindControl("txtAssignedTo") as TextBox;
            TextBox contactName = ODView.Rows[0].FindControl("txtContactName") as TextBox;
            //TextBox contactNumber = ODView.Rows[0].FindControl("txtContactNumber") as TextBox; - TODO
            TextBox enquiryDate = ODView.Rows[0].FindControl("txtEnquiryDate") as TextBox;
            TextBox closureDate = ODView.Rows[0].FindControl("txtclosureDate") as TextBox;
            oData = new EnquiryData();
            MembershipUser user = Membership.GetUser();
            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                TextBox amendReason = ODView.Rows[0].FindControl("txtAmendReason") as TextBox;
                HiddenField hdnCreatedBy = ODView.Rows[0].FindControl("hdnCreatedBy") as HiddenField;
                HiddenField hdnCreatedOn = ODView.Rows[0].FindControl("hdnCreatedOn") as HiddenField;      
                Label id = ODView.Rows[0].FindControl("lblID") as Label;
                Label revision = ODView.Rows[0].FindControl("lblRevision") as Label;                                       
                Int32.TryParse(revision.Text, out temp);

                oData.ID = Convert.ToInt32(id.Text);
                oData.Revision  = temp + 1;
               
                oData.AmendReason  = amendReason.Text;                 
                oData.ModifiedOn = DateTime.Now;
                oData.CreatedBy = Convert.ToInt32(hdnCreatedBy.Value);                
                oData.ModifiedBy = Convert.ToInt32(user.ProviderUserKey.ToString());
                oData.CreatedOn = DateTime.ParseExact(hdnCreatedOn.Value, "dd/MM/yyyy", null);
                 
            }
            else
            {
                oData.CreatedOn = DateTime.Now;
                oData.CreatedBy = Convert.ToInt32(user.ProviderUserKey.ToString());
                oData.ModifiedBy = Convert.ToInt32(user.ProviderUserKey.ToString()); 
            }
            if (enquiryDate.Text != string.Empty)
            {
                oData.EnquiryDate = DateTime.ParseExact(enquiryDate.Text, "dd/MM/yyyy", null);
            }
            else
            {
                oData.EnquiryDate = DateTime.Now;
            }
            if (closureDate.Text != string.Empty)
            {
                oData.ClosureDate = DateTime.ParseExact(closureDate.Text, "dd/MM/yyyy", null);
            }
            else
            {
                oData.ClosureDate = DateTime.Now;
            }
            Int32.TryParse(CustomerId.Text, out temp);
            oData.CustID = temp;
            oData.ProductSuggested = productSuggested.Text;
            oData.ProductStatus = Convert.ToInt32(ddlProductStatus.SelectedValue);
            oData.FollowUpStatus = Convert.ToInt32(ddlFollowUpStatus.SelectedValue);
            oData.EnquiryStatus = Convert.ToInt32(ddlEnquiryStatus.SelectedValue);
            oData.Estimation = estimation.Text;            
            oData.AssignedTo = Convert.ToInt32(ddlAssignedTo.SelectedValue);
            oData.ContactName = contactName.Text;
          
            return oData;

        }

        #endregion

        #region Handle Items

        public void PopulateItemsGrid()
        {
            string EnquiryID = null;
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                EnquiryID = Request.QueryString["ID"].ToString();
            }

            EnquiryItem OI = new EnquiryItem();
            DataSet ds = OI.GetEnquiryItems(EnquiryID);
            ViewState["Items"] = ds;
            ItemsGridView.DataSource = ds;
            ItemsGridView.DataBind();
            //CalculateBillTotals();

        }

        protected void ItemChanged(object sender, EventArgs e)
        {

            //try
            //{

            //    //POTblAdpter.returnSelectedItem("Select ItemDescription from Items where ItemCode = '" + A.Text + "'", "ItemList");
            //    //if (POTblAdpter.resultDataSet.Tables["ItemList"] != null &&
            //    //    POTblAdpter.resultDataSet.Tables["ItemList"].Rows.Count > 0)
            //    //{
            //    //    TextBox Itemsdescription = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("ItemDescription") as TextBox;                   
            //    //    Itemsdescription.Text = POTblAdpter.resultDataSet.Tables["ItemList"].Rows[0]["ItemDescription"].ToString();

            //    //}
            //    QuoTableAdapter QTblAdapter = new QuoTableAdapter();
            //    TextBox A = (TextBox)sender;
            //    string[] range = A.Text.Split('.');
            //    if (A.ID == "ItemDescription")
            //    {
            //        QTblAdapter.returnSelectedItem("Select * from QItems where QItemDescription = '" + range[0] + "'", "ItemList");
            //    }
            //    else
            //    {
            //        QTblAdapter.returnSelectedItem("Select * from QItems where QItemCode = '" + range[0] + "'", "ItemList");
            //    }

            //    //POTblAdpter.returnSelectedItem("Select * from Items where ItemCode = '" + range[0] + "'", "ItemList");
            //    if (QTblAdapter.resultDataSet.Tables["ItemList"] != null &&
            //        QTblAdapter.resultDataSet.Tables["ItemList"].Rows.Count > 0)
            //    {
            //        TextBox ItemsCode = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("b2") as TextBox;
            //        TextBox ItemsDescription = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("ItemDescription") as TextBox;
            //        TextBox ItemQuantity = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("b4") as TextBox;
            //        TextBox ItemUnit = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("b5") as TextBox;
            //        TextBox ItemRatePerUnit = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("b6") as TextBox;
            //        TextBox ItemDiscount = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("b7") as TextBox;
            //        TextBox ItemTax = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("b8") as TextBox;
            //        Label ItemTaxAmount = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("b9") as Label;
            //        Label ItemTotal = ((GridViewRow)ItemsGridView.Rows[ItemsGridView.EditIndex]).FindControl("b10") as Label;

            //        if (A.ID != "ItemDescription")
            //        {
            //            ItemsCode.Text = range[0];
            //            ItemsDescription.Text = QTblAdapter.resultDataSet.Tables["ItemList"].Rows[0]["QItemDescription"].ToString();
            //        }
            //        else
            //        {
            //            ItemsCode.Text = QTblAdapter.resultDataSet.Tables["ItemList"].Rows[0]["QItemCode"].ToString();
            //            ItemsDescription.Text = range[0];
            //        }
            //        ItemQuantity.Text = QTblAdapter.resultDataSet.Tables["ItemList"].Rows[0]["QItemQuantity"].ToString();
            //        ItemUnit.Text = QTblAdapter.resultDataSet.Tables["ItemList"].Rows[0]["QUnit"].ToString();
            //        ItemRatePerUnit.Text = QTblAdapter.resultDataSet.Tables["ItemList"].Rows[0]["QRate"].ToString();
            //        ItemDiscount.Text = "0.0";
            //        ItemTax.Text = "0.0";
            //        ItemTaxAmount.Text = "0.0";
            //        ItemTotal.Text = "0.0";
            //    }
            //}
            //catch (Exception exc)
            //{
            //    ErrorLog.addError("BillingControl:ItemChanged()", exc.Message);
            //}

        }

        protected void ActionItemBtnVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                imgSave.Visible = true;
                imgCancel.Visible = true;
                imgAdd.Visible = false;
                imgEdit.Visible = false;
                imgDelete.Visible = false;
            }
            else
            {
                imgSave.Visible = false;
                imgCancel.Visible = false;
                imgAdd.Visible = true;
                imgEdit.Visible = true;
                imgDelete.Visible = true;
            }

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

        protected void imgAddItem_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)ViewState["Items"];
            int index = 0;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        index = Convert.ToInt32(dr["ID"]);
                    }
                }
                index += 1;
                DataRow drNewRow = ds.Tables[0].NewRow();
                drNewRow["ID"] = index;
                ds.Tables[0].Rows.Add(drNewRow);
                //ds.Tables[0].AcceptChanges();

                ViewState["Items"] = ds;
                ItemsGridView.DataSource = ds;
                ItemsGridView.DataBind();
                //CalculateBillTotals();
                ITUP.Update();
            }

        }
        protected void imgEditItem_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            int index = -1;
            foreach (GridViewRow dr in ItemsGridView.Rows)
            {
                cb = (CheckBox)dr.FindControl("cbSelect");
                if (cb.Checked)
                {
                    index = dr.RowIndex; //(int)tpoGrid.DataKeys[].Value;
                    ViewState["ItemKey"] = index;// Convert.ToInt32(cb.ToolTip);
                    System.Web.UI.ScriptManager.RegisterHiddenField(ItemsGridView, "RowIndex", index.ToString());

                    break;
                }
            }
            if (index >= 0)
            {
                ActionItemBtnVisible(true);
                ItemsGridView.EditIndex = index;
                ItemsGridView.DataSource = (DataSet)ViewState["Items"];
                ItemsGridView.DataBind();
                //CalculateBillTotals();
                ITUP.Update();
            }
        }
        protected void imgDeleteItem_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            ArrayList aList = new ArrayList();
            foreach (GridViewRow dr in ItemsGridView.Rows)
            {
                cb = (CheckBox)dr.FindControl("cbSelect");
                if (cb.Checked)
                {
                    aList.Add(dr.RowIndex); //(int)tpoGrid.DataKeys[].Value;                   

                }
            }
            if (aList.Count >= 0)
            {
                DataSet ds = (DataSet)ViewState["Items"];

                for (int i = aList.Count; i > 0; i--)
                {
                    ds.Tables[0].Rows[(int)aList[i - 1]].Delete();
                }
                ItemsGridView.DataSource = (DataSet)ViewState["Items"];
                ItemsGridView.DataBind();
                //CalculateBillTotals();
                ITUP.Update();
            }
        }
        protected void imgUpdateItem_Click(object sender, EventArgs e)
        {

            if (ViewState["ItemKey"] != null)
            {
                int index = Convert.ToInt32(ViewState["ItemKey"]);
                Single totalValue = 0.0F;
                Single parseValue = 0.0F;
                int parseValueInt = 0;
                if ((ItemsGridView.Rows[index].RowState & DataControlRowState.Edit) > 0)
                {
                    Label id = (Label)ItemsGridView.Rows[index].FindControl("ID");
                    TextBox txtEnquiryID = (TextBox)ItemsGridView.Rows[index].FindControl("txtEnquiryID");
                    TextBox txtCode = (TextBox)ItemsGridView.Rows[index].FindControl("txtCode");
                    TextBox txtItemDescription = (TextBox)ItemsGridView.Rows[index].FindControl("txtItemDescription");
                    TextBox txtQuantity = (TextBox)ItemsGridView.Rows[index].FindControl("txtQuantity");
                    TextBox txtUnit = (TextBox)ItemsGridView.Rows[index].FindControl("txtUnit");
                    TextBox txtRate = (TextBox)ItemsGridView.Rows[index].FindControl("txtRate");
                    TextBox txtTax = (TextBox)ItemsGridView.Rows[index].FindControl("txtTax");
                    //TextBox txtDiscount = (TextBox)ItemsGridView.Rows[index].FindControl("txtDiscount");                    
                    //Label txtTaxAmount = (Label)ItemsGridView.Rows[index].FindControl("lblTaxAmount");
                    //Label txtTotalAmount = (Label)ItemsGridView.Rows[index].FindControl("lblTotalAmount");

                    DataSet ds = (DataSet)ViewState["Items"];

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["ID"].ToString() == id.Text)
                        {
                            Int32.TryParse(txtEnquiryID.Text, out parseValueInt);
                            if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
                            {
                                parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
                            }
                            dr["EnquiryID"] = parseValueInt;
                            dr["Code"] = txtCode.Text;
                            dr["Description"] = txtItemDescription.Text;
                            float.TryParse(txtQuantity.Text, out parseValue);
                            dr["Quantity"] = parseValue;
                            //dr["Balance"] = parseValue;
                            totalValue = parseValue;
                            dr["Unit"] = txtUnit.Text;
                            float.TryParse(txtRate.Text, out parseValue);
                            dr["Rate"] = parseValue;
                            //totalValue = totalValue * parseValue;
                            //float.TryParse(txtDiscount.Text, out parseValue);
                            //dr["Discount"] = parseValue;
                            //if (totalValue > 0)
                            //    totalValue = totalValue - ((totalValue * parseValue) / 100);
                            float.TryParse(txtTax.Text, out parseValue);
                            dr["Tax"] = parseValue;
                            //if (totalValue > 0)
                            //{
                            //    totalValue = totalValue - ((totalValue * parseValue) / 100);
                            //    dr["TaxAmount"] = (totalValue * parseValue) / 100;
                            //}
                            
                            //dr["TotalAmount"] = totalValue;

                            break;
                        }
                    }
                    ViewState["Items"] = ds;
                    // change the editindex and rebind
                    ItemsGridView.EditIndex = -1;
                    //ItemsBtnPanel.Visible = false;
                    //TermsBtnPanel.Visible = false;
                    ActionItemBtnVisible(false);
                    ItemsGridView.DataSource = (DataSet)ViewState["Items"];
                    ItemsGridView.DataBind();
                    //CalculateBillTotals();
                    ITUP.Update();
                }
            }

        }
        protected void imgCancelItem_Click(object sender, EventArgs e)
        {
            ItemsGridView.EditIndex = -1;
            ActionItemBtnVisible(false);
            ItemsGridView.DataSource = (DataSet)ViewState["Items"];
            ItemsGridView.DataBind();
            ITUP.Update();
            //CalculateBillTotals();
        }

        private void CalculateBillTotals()
        {
            DataSet itemDS = (DataSet)ViewState["Items"];
            Single parseValue = 0.0F;
            Single discountSum = 0.0F;
            Single taxSum = 0.0F;
            Single totalSum = 0.0F;
            foreach (DataRow dRow in itemDS.Tables[0].Rows)
            {
                if (dRow.RowState != DataRowState.Deleted)
                {

                    float.TryParse(dRow["Discount"].ToString(), out parseValue);
                    discountSum += parseValue;
                    float.TryParse(dRow["Tax"].ToString(), out parseValue);
                    taxSum += parseValue;
                    float.TryParse(dRow["TotalAmount"].ToString(), out parseValue);
                    totalSum += parseValue;
                }
            }
            ViewState["ItemDiscount"] = discountSum > 0 ? discountSum = discountSum / itemDS.Tables[0].Rows.Count : 0;
            ViewState["ItemTax"] = taxSum > 0 ? taxSum = taxSum / itemDS.Tables[0].Rows.Count : 0;
            ViewState["ItemTotal"] = totalSum;
            GridViewRow row = ItemsGridView.FooterRow;

            if (row != null)
            {
                ((Label)row.FindControl("lblDiscount")).Text = discountSum.ToString();
                ((Label)row.FindControl("lblTax")).Text = taxSum.ToString();
                ((Label)row.FindControl("lblTotalAmount")).Text = totalSum.ToString();
            }
        }

        #endregion

        #region Handle Terms
        public void PopulateTermsGrid()
        {
            string billID = null;
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                billID = Request.QueryString["ID"].ToString();
            }

            EnquiryTerm OI = new EnquiryTerm();
            DataSet ds = OI.GetEnquiryTerms(billID);
            ViewState["Terms"] = ds;
            TermsGridView.DataSource = ds;
            TermsGridView.DataBind();

        }

        protected void ActionTermBtnVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                imgSaveTerm.Visible = true;
                imgCancelTerm.Visible = true;
                imgAddTerm.Visible = false;
                imgEditTerm.Visible = false;
                imgDeleteTerm.Visible = false;
            }
            else
            {
                imgSaveTerm.Visible = false;
                imgCancelTerm.Visible = false;
                imgAddTerm.Visible = true;
                imgEditTerm.Visible = true;
                imgDeleteTerm.Visible = true;
            }

        }

        protected void ActionTermBtnPanelVisible(Boolean mode)
        {
            if (mode)
            {
                TermsBtnPanel.Visible = true;
            }
            else
            {
                TermsBtnPanel.Visible = false;
            }
        }

        protected void imgAddTerm_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)ViewState["Terms"];
            int index = 0;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        index = Convert.ToInt32(dr["ID"]);
                    }
                }
                index += 1;
                DataRow drNewRow = ds.Tables[0].NewRow();
                drNewRow["ID"] = index;
                ds.Tables[0].Rows.Add(drNewRow);
                ViewState["Terms"] = ds;
                TermsGridView.DataSource = ds;
                TermsGridView.DataBind();
            }

        }
        protected void imgEditTerm_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            int index = -1;
            foreach (GridViewRow dr in TermsGridView.Rows)
            {
                cb = (CheckBox)dr.FindControl("cbSelect");
                if (cb.Checked)
                {
                    index = dr.RowIndex; //(int)tpoGrid.DataKeys[].Value;
                    ViewState["TermKey"] = index;// Convert.ToInt32(cb.ToolTip);
                    break;
                }
            }
            if (index >= 0)
            {
                ActionTermBtnVisible(true);
                TermsGridView.EditIndex = index;
                TermsGridView.DataSource = (DataSet)ViewState["Terms"];
                TermsGridView.DataBind();
                ITUP.Update();
            }
        }
        protected void imgDeleteTerm_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            ArrayList aList = new ArrayList();
            foreach (GridViewRow dr in TermsGridView.Rows)
            {
                cb = (CheckBox)dr.FindControl("cbSelect");
                if (cb.Checked)
                {
                    aList.Add(dr.RowIndex); //(int)tpoGrid.DataKeys[].Value;                   

                }
            }
            if (aList.Count >= 0)
            {
                DataSet ds = (DataSet)ViewState["Terms"];
                for (int i = aList.Count; i > 0; i--)
                {
                    ds.Tables[0].Rows[(int)aList[i - 1]].Delete();
                }
                TermsGridView.DataSource = (DataSet)ViewState["Terms"];
                TermsGridView.DataBind();
                ITUP.Update();
            }
        }
        protected void imgUpdateTerm_Click(object sender, EventArgs e)
        {
            int parseValueInt = 0;
            if (ViewState["TermKey"] != null)
            {
                int index = Convert.ToInt32(ViewState["TermKey"]);

                if ((TermsGridView.Rows[index].RowState & DataControlRowState.Edit) > 0)
                {
                    Label id = (Label)TermsGridView.Rows[index].FindControl("ID");
                    TextBox txtEnquiryID = (TextBox)TermsGridView.Rows[index].FindControl("txtEnquiryID");
                    TextBox txtTermBox = (TextBox)TermsGridView.Rows[index].FindControl("TermBox");
                    TextBox txtConditionBox = (TextBox)TermsGridView.Rows[index].FindControl("ConditionBox");

                    DataSet ds = (DataSet)ViewState["Terms"];

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["ID"].ToString() == id.Text)
                        {
                            Int32.TryParse(txtEnquiryID.Text, out parseValueInt);
                            if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
                            {
                                parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
                            }
                            dr["EnquiryID"] = parseValueInt;
                            dr["Term"] = txtTermBox.Text;
                            dr["Condition"] = txtConditionBox.Text;

                            break;
                        }
                    }
                    ViewState["Terms"] = ds;

                    ActionTermBtnVisible(false);
                    // change the editindex and rebind
                    TermsGridView.EditIndex = -1;
                    TermsGridView.DataSource = (DataSet)ViewState["Terms"];
                    TermsGridView.DataBind();
                    ITUP.Update();
                }
            }

        }
        protected void imgCancelTerm_Click(object sender, EventArgs e)
        {
            TermsGridView.EditIndex = -1;
            ActionTermBtnVisible(false);
            TermsGridView.DataSource = (DataSet)ViewState["Terms"];
            TermsGridView.DataBind();
        }

        protected void TermsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //e.Row.FindControl("Termddl");
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList condDdl = e.Row.Cells[0].FindControl("Conditionddl") as DropDownList;
                    DropDownList termDdl = e.Row.Cells[0].FindControl("Termddl") as DropDownList;
                    TextBox ConditionBox = (TextBox)e.Row.Cells[0].FindControl("ConditionBox");
                    TextBox TermBox = (TextBox)e.Row.Cells[0].FindControl("TermBox");
                    if (termDdl != null && TermBox != null && ConditionBox != null && condDdl != null)
                    {
                        termDdl.Attributes.Add("onchange", "CheckOtherOption(" + termDdl.ClientID + "," + condDdl.ClientID + "," + TermBox.ClientID + "," + ConditionBox.ClientID + ")");
                        condDdl.Attributes.Add("onchange", "CheckOtherOption(" + termDdl.ClientID + "," + condDdl.ClientID + "," + TermBox.ClientID + "," + ConditionBox.ClientID + ")");
                    }

                }
            }
            catch (Exception exc)
            {
                throw exc;

            }
        }


        #endregion

    }
}