using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.Data;
using WC = Inventory.Common.WebCommon;
using System.Collections; 
using ASM = CoreAssemblies;
using System.Web.Security;
using AllModules.Common;
using AllModules;
using Inventory.BusLogic;
using System.Data;
using CoreAssemblies;

namespace Inventory.Forms
{
    public partial class ManageStock : System.Web.UI.Page
    {
        #region Private Variables

        StockData oData = null;
        private bool isEditMode = false;
        protected bool IsInEditMode
        {
            get
            {

                this.isEditMode = ViewState["IsEditMode"] != null ? (bool)ViewState["IsEditMode"] : false;
                return this.isEditMode;
            }
            set
            {
                this.isEditMode = value;
                ViewState["IsEditMode"] = value;
            }
        }

        protected string RequestBillID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return Request.QueryString["ID"].ToString();
                else
                    return string.Empty;

            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {              

                if (!Page.IsPostBack)
                {
                    // case of insert or edit
                    if (RequestBillID != string.Empty)
                    {   //case of amend                       
                        ToggleAmendInsertVisible(false);
                        ActionItemBtnVisible(false);
                        ActionTermBtnVisible(false);
                        ActionItemBtnPanelVisible(false);
                        ActionTermBtnPanelVisible(false);
                        CheckStatus(); 
                        ODView.DataBind();
                        PopulateItemsGrid();
                        PopulateTermsGrid();
                        DUP.Update();
                        ITUP.Update();

                    }
                    else
                    {
                        //case of insert
                        IsInEditMode = true;
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
                        //Get the data from grid into datatable
                        UpdateGridViewDetailsToDataTable();
                        CalculateBillTotals();
                    }
                }
                string parameter = Request["__EVENTARGUMENT"];
                if (parameter == "forcePostback")
                    btnPostBack_Click(sender, e);

            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }


        }

        public void CheckStatus()
        {              
            Inventory.BusLogic.Stock stock = new Inventory.BusLogic.Stock();
            string StockType = string.Empty;
            int stockType = 0;
            EnumClass.Status status = (EnumClass.Status)stock.GetStockStatus(RequestBillID, ref StockType); 

            if (status == EnumClass.Status.Submitted)
            {
                ActionItemBtnPanelVisible(false);
                ActionTermBtnPanelVisible(false);
                AllButtonsSubmittedState();
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
            try
            {
                Contact.Controls.Contacts uc = sender as Contact.Controls.Contacts;
                if (!String.IsNullOrEmpty(uc.ContactID))
                {
                    Label hdnCustomerId = ODView.Rows[0].FindControl("hdnCustomerID") as Label;
                    hdnCustomerId.Text = uc.ContactID;
                    PopulateCustomerAddress();
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        protected void PopulateCustomerAddress()
        {
            DetailsView view = ODView;
            DetailsViewRowCollection rows = view.Rows;
            try
            {
                if (rows != null && rows.Count > 0)
                {
                    DetailsViewRow row = rows[0];

                    // For File download display link
                    HiddenField hdnFileID = row.FindControl("hdnFileID") as HiddenField;
                    if (hdnFileID!=null && hdnFileID.Value != string.Empty && hdnFileID.Value != "0")
                    {
                        Files files = new Files();
                        lnkFileDownload.NavigateUrl = "~/FileHandler.ashx?FID=" + hdnFileID.Value;
                        lnkFileDownload.Text = files.GetFileName(hdnFileID.Value);
                    }

                    // This is for StockType & Location only
                    if (ODView.CurrentMode == DetailsViewMode.Insert || ODView.CurrentMode == DetailsViewMode.Edit)
                    {
                        fupload.Enabled = true;

                        HiddenField hdnStockType = row.FindControl("hdnStockType") as HiddenField;
                        DropDownList ddlStockType = row.FindControl("ddlStockType") as DropDownList;
                        ddlStockType.DataSource = EnumTable.GetStockTypeFromCache();
                        ddlStockType.DataTextField = "Value";
                        ddlStockType.DataValueField = "Key";
                        ddlStockType.DataBind();

                        HiddenField hdnLocation = row.FindControl("hdnLocation") as HiddenField;
                        DropDownList ddlLocation = row.FindControl("ddlLocation") as DropDownList;
                        ddlLocation.DataSource = EnumTable.GetLocationFromCache();
                        ddlLocation.DataTextField = "Value";
                        ddlLocation.DataValueField = "Key";
                        ddlLocation.DataBind();

                        if (hdnStockType.Value != string.Empty)
                        {
                            ddlStockType.SelectedValue = hdnStockType.Value;
                        }
                        if (hdnLocation.Value != string.Empty)
                        {
                            ddlLocation.SelectedValue = hdnLocation.Value;
                        }

                    }
                    else
                    {
                        fupload.Enabled = false;
                    }

                    // Following is for Customer only
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
                            lnkName.Text = "Add New Vendor";
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
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
                Errors.LogError(exc);
            }
        }

        protected void ODView_DataBound(object sender, EventArgs e)
        {
            PopulateCustomerAddress();
        }

        protected void btnPostBack_Click(object sender, EventArgs e)
        {
            string purchaseID=((TextBox)ODView.Rows[0].FindControl("txtReferencePO")).Text;
            DataSet dsItems = (DataSet)ViewState["Items"];
            //Remove all existing rows before updating the Purchase Items, as we need to remove from existing rows from Stockitem
            // for earlier purchase Items entered.
            for (int i = dsItems.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                dsItems.Tables[0].Rows[i].Delete();
            }
            dsItems = Inventory.Common.WebCommon.CreateInventoryItemListFromPurchaseOrder(dsItems,purchaseID);
            ViewState["Items"] = dsItems; 
            ItemsGridView.DataSource = dsItems;
            ItemsGridView.DataBind();
            CalculateBillTotals();
            ITUP.Update();
        }
        #region Bill Details

        protected void AllButtonsSubmittedState()
        {
            btnAmend.Visible = false;
            btnUpdate.Visible = false;
            btnSubmit.Visible = false;
            btnInsert.Visible = false;
            btnCancel.Visible = false;
        }

        protected void ToggleAmendInsertVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Visible = false;
                //btnPDFPrint.Visible = false;
                //btnExcelPrint.Visible = false;
                btnUpdate.Visible = false;             
                btnSubmit.Visible = false;
                btnInsert.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                //btnPDFPrint.Visible = true;
                //btnExcelPrint.Visible = true;
                btnAmend.Visible = true;
                btnCancel.Visible = false;
                btnSubmit.Visible = true;

                btnInsert.Visible = false; 
                btnUpdate.Visible = false;
                
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    //btnPDFPrint.Attributes.Add("onclick", "PrintReportInNewWindow(" + Request.QueryString["ID"] + ",'PDF'); return false;");
                    //btnExcelPrint.Attributes.Add("onclick", "PrintReportInNewWindow(" + Request.QueryString["ID"] + ",'EXCEL'); return false;");

                }
            }

        }

        protected void ToggleAmendUpdateVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Visible = true;
                btnUpdate.Visible = false;
                btnSubmit.Visible = true;
                btnCancel.Visible = false;
            }
            else
            {
                btnAmend.Visible = false;
                btnUpdate.Visible = true;
                btnSubmit.Visible = false;
                btnCancel.Visible = true;
            }

        }

        protected void btnAmend_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleAmendUpdateVisible(false);
                ActionItemBtnPanelVisible(true);
                ActionTermBtnPanelVisible(true);
                ODView.ChangeMode(DetailsViewMode.Edit);
                ODView.DataBind();
                IsInEditMode = true;
                PopulateItemsGrid();
                DUP.Update();
                ITUP.Update();
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                StockData oData = CopyUIToStockDataObject();
                DataSet dsItems = (DataSet)ViewState["Items"];
                DataSet dsTerms = (DataSet)ViewState["Terms"];
                FilesData fsData = CopyUIToFilesData();
                Files files = new Files();
                if (oData.FileID > 0 && fsData != null && fsData.OleAttach != null)
                {
                    files.ReplaceFile(fsData, oData.FileID);
                }
                else if (fsData != null && fsData.FileName != null && fsData.OleAttach != null)
                {
                    oData.FileID = files.InsertNewFile(fsData);
                }
                ArrayList sqlCommands = new ArrayList();
                string updateSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "UPDATE", "Stock");
                sqlCommands.Add(updateSql);
                if (dsItems != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new StockItemData(), dsItems.Tables[0], "StockItem"));
                if (dsTerms != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new StockTermData(), dsTerms.Tables[0], "StockTerm"));

                int[] x = WC.UpdateStockToDbUsingTransaction(sqlCommands);
                ClearViewState();
                if (x[0] < 0)
                {
                    //Error

                }
                else
                {

                    IsInEditMode = false;
                    ToggleAmendUpdateVisible(true);
                    ODView.ChangeMode(DetailsViewMode.ReadOnly);
                    ActionItemBtnVisible(false);
                    ActionTermBtnVisible(false);
                    ActionItemBtnPanelVisible(false);
                    ActionTermBtnPanelVisible(false);
                    PopulateItemsGrid();
                    PopulateTermsGrid();
                    CalculateBillTotals();
                    ODView.DataBind();
                    DUP.Update();
                    ITUP.Update();
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Only change the value of status to submitted
            Inventory.BusLogic.Stock stock= new Inventory.BusLogic.Stock();
            string StockType=string.Empty;
            int stockType =0;
            EnumClass.Status status = (EnumClass.Status)stock.GetStockStatus(RequestBillID, ref StockType);
            if (status == EnumClass.Status.Draft)
            {
                stockType = Convert.ToInt32(StockType);
                if (Inventory.Common.WebCommon.InsertStockItemsToProductMasterTransactions(RequestBillID, stockType))
                {
                    stock.UpdateStockStatus(RequestBillID);
                    CheckStatus();
                }
            }
        }

        public void ShowMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", "javascript:alert('" + msg + "')", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (ODView.CurrentMode == DetailsViewMode.Edit)
                {
                    ToggleAmendUpdateVisible(true);
                }
                else if (ODView.CurrentMode == DetailsViewMode.Insert)
                {
                    Response.Redirect("~/Inventory/Forms/StockLedger.aspx", false);
                }
                else if (ODView.CurrentMode == DetailsViewMode.ReadOnly)
                {
                    Response.Redirect("~/Inventory/Forms/StockLedger.aspx", false);
                }
                IsInEditMode = false;
                ODView.DataBind();
                ODView.ChangeMode(DetailsViewMode.ReadOnly);
                PopulateItemsGrid();
                PopulateTermsGrid();
                ActionItemBtnPanelVisible(false);
                ActionTermBtnPanelVisible(false);
                ITUP.Update();
                DUP.Update();
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {

                StockData oData = CopyUIToStockDataObject();
                //todo set createdby for oData to userid

                DataSet dsItems = (DataSet)ViewState["Items"];
                DataSet dsTerms = (DataSet)ViewState["Terms"];
                FilesData fsData = CopyUIToFilesData();
                Files files = new Files();
                if (oData.FileID > 0 && fsData != null && fsData.OleAttach != null)
                {
                    files.ReplaceFile(fsData, oData.FileID);
                }
                else if (fsData != null && fsData.FileName != null && fsData.OleAttach != null)
                {
                    oData.FileID = files.InsertNewFile(fsData);
                }
                string insertSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "INSERT", "Stock");
                string identitySql = "Select @@Identity";
                ArrayList sqlCommands = new ArrayList();
                sqlCommands.Add(insertSql);
                sqlCommands.Add(identitySql);
                if (dsItems != null)
                {
                    dsItems.Tables[0].Columns.Remove("OrderedQuantity");
                    dsItems.Tables[0].Columns.Remove("ReceivedQuantity");
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new StockItemData(), dsItems.Tables[0], "StockItem"));
                }
                if (dsTerms != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new StockTermData(), dsTerms.Tables[0], "StockTerm"));

                int x = WC.InsertStockToDbUsingTransaction(sqlCommands);
                ViewState["Items"] = null;
                ViewState["Terms"] = null;
                ViewState["ItemKey"] = null;

                if (x < 0)
                {
                    //Error
                }
                else
                {
                    string redirectUrl = "~/Inventory/Forms/ManageStock.aspx?ID=" + x.ToString();
                    Response.Redirect(redirectUrl,true);
                    //ODView.DataBind();
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        public StockData CopyUIToStockDataObject()
        {
            int temp = 0;
            oData = new StockData();
            try
            {
                //TextBox modeOfDelivery = ODView.Rows[0].FindControl("txtModeOfDelivery") as TextBox;
                //TextBox modeOfPayment = ODView.Rows[0].FindControl("txtModeOfPayment") as TextBox;
                //  TextBox requestedFor = ODView.Rows[0].FindControl("txtrequestedFor") as TextBox;
                Label   CustomerId = ODView.Rows[0].FindControl("hdnCustomerID") as Label;
                TextBox refId = ODView.Rows[0].FindControl("txtReferencePO") as TextBox;
                
                TextBox txtNotes = ODView.Rows[0].FindControl("txtNotes") as TextBox;
                TextBox billDate = ODView.Rows[0].FindControl("txtBillDate") as TextBox; 
                TextBox txtSenderName = ODView.Rows[0].FindControl("txtSenderName") as TextBox;
                TextBox txtSenderPhone = ODView.Rows[0].FindControl("txtSenderPhone") as TextBox;
                TextBox txtDeliveryTo = ODView.Rows[0].FindControl("txtDeliveryTo") as TextBox;               
                TextBox txtTrackingNumber = ODView.Rows[0].FindControl("txtTrackingNumber") as TextBox;
                TextBox txtDeliveryBy   = ODView.Rows[0].FindControl("txtDeliveryBy") as TextBox;
                TextBox txtCourierPhone = ODView.Rows[0].FindControl("txtCourierPhone") as TextBox;
                TextBox txtCourierCompany = ODView.Rows[0].FindControl("txtCourierCompany") as TextBox;
                DropDownList ddlStockType = ODView.Rows[0].FindControl("ddlStockType") as DropDownList;
                DropDownList ddlLocation = ODView.Rows[0].FindControl("ddlLocation") as DropDownList;
                HiddenField hdnFileID = ODView.Rows[0].FindControl("hdnFileID") as HiddenField;

                MembershipUser user = Membership.GetUser();
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    TextBox amendReason = ODView.Rows[0].FindControl("txtAmendReason") as TextBox;
                    HiddenField hdnCreatedBy = ODView.Rows[0].FindControl("hdnCreatedBy") as HiddenField;
                    //  TextBox RequestedFor = ODView.Rows[0].FindControl("txtRequestedFor") as TextBox;
                    Label id = ODView.Rows[0].FindControl("lblID") as Label;
                    Label revision = ODView.Rows[0].FindControl("lblRevision") as Label;
                    HiddenField hdnCreatedOn = ODView.Rows[0].FindControl("hdnCreatedOn") as HiddenField;
                    oData.ID = Convert.ToInt32(id.Text);
                    Int32.TryParse(revision.Text, out temp);
                    oData.Revision = temp + 1;
                    oData.CreatedBy = Convert.ToInt32(hdnCreatedBy.Value);
                    oData.AmendReason = amendReason.Text;
                    //oData.RequestedFor = RequestedFor.Text;
                    oData.ModifiedOn = DateTime.Now;
                    oData.AmendedBy = Convert.ToInt32(user.ProviderUserKey.ToString());
                    oData.CreatedOn = DateTime.ParseExact(hdnCreatedOn.Value, "dd/MM/yyyy", null);
                    if (hdnFileID.Value != "0")
                    {
                        Int32.TryParse(hdnFileID.Value, out temp);
                        oData.FileID = temp;
                    }
                }
                else
                {
                    oData.CreatedOn = DateTime.Now;
                    oData.CreatedBy = Convert.ToInt32(user.ProviderUserKey.ToString());
                    oData.Status = (int)CoreAssemblies.EnumClass.Status.Draft;
                }
                if (billDate.Text != string.Empty)
                {
                    oData.TransactionDate = DateTime.ParseExact(billDate.Text, "dd/MM/yyyy", null);
                }
                else
                {
                    oData.TransactionDate = DateTime.Now;
                }
                Int32.TryParse(CustomerId.Text, out temp);
                oData.VendorID = temp;
                oData.ReferenceID = refId.Text;

                oData.SenderName = txtSenderName.Text;
                oData.SenderPhone = txtSenderPhone.Text;                
                oData.TrackingNumber = txtTrackingNumber.Text;
                oData.DeliveryTo = txtDeliveryTo.Text;
                oData.DeliveryBy = txtDeliveryBy.Text;
                oData.DeliveryByPhone = txtCourierPhone.Text;
                oData.DeliveryByCompany = txtCourierCompany.Text;
                oData.Notes = txtNotes.Text;
                oData.StockType = Convert.ToInt32(ddlStockType.SelectedValue);
                if(ddlLocation.SelectedValue != string.Empty)
                oData.LocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                oData.DiscountAmount = Convert.ToSingle(ViewState["ItemDiscount"]);
                oData.TaxAmount = Convert.ToSingle(ViewState["ItemTax"]);                
                oData.SubTotal = Convert.ToSingle(ViewState["ItemTotal"]);
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
            return oData;

        }

        #endregion

        #region Handle Items

        public void PopulateItemsGrid()
        {
            string StockID = null;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    StockID = Request.QueryString["ID"].ToString();
                }

                StockItem OI = new StockItem();
                DataSet ds = OI.GetStockItems(StockID,Convert.ToString((int)EnumClass.StockType.Receive));
                ViewState["Items"] = ds;
                ItemsGridView.DataSource = ds;
                ItemsGridView.DataBind();
                CalculateBillTotals();
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }
        
        public void RefreshGrid()
        {

            try
            {
                if (ViewState["Items"] != null)
                {
                    DataSet ds = (DataSet)ViewState["Items"];
                    ItemsGridView.DataSource = ds;
                    ItemsGridView.DataBind();
                    ITUP.Update();
                }

            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        protected void ActionItemBtnVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                imgSave.Visible = true;
                imgCancel.Visible = true;
                imgAdd.Visible = false;
                //imgEdit.Visible = false;
                imgDelete.Visible = false;
            }
            else
            {
                imgSave.Visible = false;
                imgCancel.Visible = false;
                imgAdd.Visible = true;
                //imgEdit.Visible = true;
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
            MembershipUser user = Membership.GetUser();
            int currIndex, index = 0;
            try
            {
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted && dr.RowState != DataRowState.Detached)
                            {
                                currIndex = Convert.ToInt32(dr["ID"]);
                                if (currIndex > index)
                                {
                                    index = currIndex;
                                }
                            }
                        }
                    }
                    index += 1;
                    DataRow drNewRow = ds.Tables[0].NewRow();
                    drNewRow["ID"] = index;
                    drNewRow["CreatedBy"] = Convert.ToInt32(user.ProviderUserKey.ToString());
                    drNewRow["ModifiedBy"] = Convert.ToInt32(user.ProviderUserKey.ToString());
                    drNewRow["ModifiedOn"] = DateTime.Now;
                    drNewRow["CreatedOn"] = DateTime.Now;
                    ds.Tables[0].Rows.Add(drNewRow);
                    //ds.Tables[0].AcceptChanges();

                    ViewState["Items"] = ds;
                    ItemsGridView.DataSource = ds;
                    ItemsGridView.DataBind();
                    CalculateBillTotals();
                    ITUP.Update();
                }

            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }

        }
        protected void imgEditItem_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            int index = -1;
            try
            {
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
                    CalculateBillTotals();
                    ITUP.Update();
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }
        protected void imgDeleteItem_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            Label lbl = null;
            ArrayList aList = new ArrayList();
            try
            {
                foreach (GridViewRow dr in ItemsGridView.Rows)
                {
                    cb = (CheckBox)dr.FindControl("cbSelect");
                    lbl = (Label)dr.FindControl("lblID");
                    if (cb.Checked)
                    {
                        aList.Add(lbl.Text); //(int)tpoGrid.DataKeys[].Value;                   

                    }
                }
                if (aList.Count >= 0)
                {
                    DataSet ds = (DataSet)ViewState["Items"];

                    for (int index = 0; index < aList.Count; index++)
                    {
                        for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            //DataRow dr in dsItems.Tables[0].Rows
                            DataRow dr = ds.Tables[0].Rows[i];
                            if (dr.RowState != DataRowState.Deleted && dr.RowState != DataRowState.Detached)
                            {
                                if (dr["ID"].ToString() == aList[index].ToString())
                                {
                                    dr.Delete();
                                }
                            }
                        }
                    }
                    ViewState["Items"] = ds;
                    ItemsGridView.DataSource = ds;
                    ItemsGridView.DataBind();
                    CalculateBillTotals();
                    ITUP.Update();
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }
        protected void imgUpdateItem_Click(object sender, EventArgs e)
        {

            //if (ViewState["ItemKey"] != null)
            //{
            //    int index = Convert.ToInt32(ViewState["ItemKey"]);
            //    Single totalValue = 0.0F;
            //    Single parseValue = 0.0F;
            //    int parseValueInt = 0;
            //    try
            //    {
            //        if ((ItemsGridView.Rows[index].RowState & DataControlRowState.Edit) > 0)
            //        {
            //            Label id = (Label)ItemsGridView.Rows[index].FindControl("ID");
            //            TextBox txtBillID = (TextBox)ItemsGridView.Rows[index].FindControl("txtBillID");
            //            TextBox txtCode = (TextBox)ItemsGridView.Rows[index].FindControl("txtCode");
            //            TextBox txtItemDescription = (TextBox)ItemsGridView.Rows[index].FindControl("txtItemDescription");
            //            TextBox txtQuantity = (TextBox)ItemsGridView.Rows[index].FindControl("txtQuantity");
            //            TextBox txtRejectedQuantity = (TextBox)ItemsGridView.Rows[index].FindControl("txtQuantity");
            //            TextBox txtUnit = (TextBox)ItemsGridView.Rows[index].FindControl("txtUnit");
            //            TextBox txtRate = (TextBox)ItemsGridView.Rows[index].FindControl("txtRate");
            //            TextBox txtDiscount = (TextBox)ItemsGridView.Rows[index].FindControl("txtDiscount");
            //            TextBox txtTax = (TextBox)ItemsGridView.Rows[index].FindControl("txtTax");
            //            Label txtTaxAmount = (Label)ItemsGridView.Rows[index].FindControl("lblTaxAmount");
            //            Label txtTotalAmount = (Label)ItemsGridView.Rows[index].FindControl("lblTotalAmount");

            //            DataSet ds = (DataSet)ViewState["Items"];

            //            foreach (DataRow dr in ds.Tables[0].Rows)
            //            {
            //                if (dr["ID"].ToString() == id.Text)
            //                {
            //                    Int32.TryParse(txtBillID.Text, out parseValueInt);
            //                    if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
            //                    {
            //                        parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
            //                    }
            //                    dr["StockID"] = parseValueInt;
            //                    dr["Code"] = txtCode.Text;
            //                    dr["Description"] = txtItemDescription.Text;
            //                    float.TryParse(txtQuantity.Text, out parseValue);
            //                    dr["Quantity"] = parseValue;
            //                    float.TryParse(txtRejectedQuantity.Text, out parseValue);
            //                    dr["RejectedQuantity"] = parseValue;
            //                    dr["Balance"] = parseValue;
            //                    totalValue = parseValue;
            //                    dr["Unit"] = txtUnit.Text;
            //                    float.TryParse(txtRate.Text, out parseValue);
            //                    dr["Rate"] = parseValue;
            //                    totalValue = totalValue * parseValue;
            //                    float.TryParse(txtDiscount.Text, out parseValue);
            //                    dr["Discount"] = parseValue;
            //                    if (totalValue > 0)
            //                        totalValue = totalValue - ((totalValue * parseValue) / 100);
            //                    float.TryParse(txtTax.Text, out parseValue);
            //                    dr["Tax"] = parseValue;
            //                    if (totalValue > 0)
            //                    {
            //                        totalValue = totalValue - ((totalValue * parseValue) / 100);
            //                        dr["TaxAmount"] = (totalValue * parseValue) / 100;
            //                    }

            //                    dr["TotalAmount"] = totalValue;

            //                    break;
            //                }
            //            }
            //            ViewState["Items"] = ds;
            //            // change the editindex and rebind
            //            ItemsGridView.EditIndex = -1;
            //            //ItemsBtnPanel.Visible = false;
            //            //TermsBtnPanel.Visible = false;
            //            ActionItemBtnVisible(false);
            //            ItemsGridView.DataSource = (DataSet)ViewState["Items"];
            //            ItemsGridView.DataBind();
            //            CalculateBillTotals();
            //            ITUP.Update();
            //        }
            //    }
            //    catch (Exception exc)
            //    {
            //        Errors.LogError(exc);
            //    }
            //}

        }
        protected void imgCancelItem_Click(object sender, EventArgs e)
        {
            try
            {
                ItemsGridView.EditIndex = -1;
                ActionItemBtnVisible(false);
                ItemsGridView.DataSource = (DataSet)ViewState["Items"];
                ItemsGridView.DataBind();
                ITUP.Update();
                CalculateBillTotals();
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        protected void imgRefresh_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        private void CalculateBillTotals()
        {

            Single parseValue = 0.0F;
            Single discountSum = 0.0F;
            Single taxSum = 0.0F;

            Single totalSum = 0.0F;
            Single subTotalSum = 0.0F;
            try
            {
                DataSet itemDS = (DataSet)ViewState["Items"];
                foreach (DataRow dRow in itemDS.Tables[0].Rows)
                {
                    if (dRow.RowState != DataRowState.Deleted)
                    {
                        float.TryParse(dRow["SubTotal"].ToString(), out parseValue);
                        subTotalSum += parseValue;
                        //float.TryParse(dRow["Discount"].ToString(), out parseValue);
                        //discountSum += parseValue;
                        float.TryParse(dRow["DiscountAmount"].ToString(), out parseValue);
                        discountSum += parseValue;
                        //float.TryParse(dRow["Tax"].ToString(), out parseValue);
                        //taxSum += parseValue;                   
                        float.TryParse(dRow["TaxAmount"].ToString(), out parseValue);
                        taxSum += parseValue;
                        float.TryParse(dRow["Total"].ToString(), out parseValue);
                        totalSum += parseValue;
                    }
                }
                ViewState["ItemDiscount"] = discountSum;
                ViewState["ItemTax"] = taxSum;
                ViewState["ItemTotal"] = totalSum;
                GridViewRow row = ItemsGridView.FooterRow;

                if (row != null)
                {
                    ((Label)row.FindControl("lblSubTotal")).Text = subTotalSum.ToString();
                    ((Label)row.FindControl("lblTotalDiscountAmount")).Text = discountSum.ToString();
                    ((Label)row.FindControl("lblTotalTaxAmount")).Text = taxSum.ToString();
                    ((Label)row.FindControl("lblTotalAmount")).Text = totalSum.ToString();
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        public void UpdateGridViewDetailsToDataTable()
        {
            Single totalValue = 0.0F;
            Single parseValue = 0.0F;
            Single quantity = 0.0F;
            Single rate = 0.0F;
            Single discount = 0.0F;
            Single tax = 0.0F;
            Single orderedQuantity = 0.0F;
            Single receivedQuantity = 0.0F;
            
            int parseValueInt = 0;
            DataSet dsItems = (DataSet)ViewState["Items"];
            MembershipUser user = Membership.GetUser();
            foreach (GridViewRow GRow in ItemsGridView.Rows)
            {
                
                    Label ID = (Label)GRow.FindControl("lblID");
                    string StockID = RequestBillID;
                    TextBox txtCode = (TextBox)GRow.FindControl("txtCode");
                    TextBox txtItemDescription = (TextBox)GRow.FindControl("txtItemDescription");
                    TextBox txtQuantity = (TextBox)GRow.FindControl("txtQuantity");
                    Label lblReceivedQuantity = (Label)GRow.FindControl("lblReceivedQuantity");
                    Label lblOrderedQuantity = (Label)GRow.FindControl("lblOrderedQuantity");
                    TextBox txtUnit = (TextBox)GRow.FindControl("txtUnit");
                    TextBox txtRate = (TextBox)GRow.FindControl("txtRate");
                    TextBox txtDiscount = (TextBox)GRow.FindControl("txtDiscount");
                    TextBox txtTax = (TextBox)GRow.FindControl("txtTax");
                    Label txtTaxAmount = (Label)GRow.FindControl("lblTaxAmount");
                    Label txtTotal = (Label)GRow.FindControl("lblTotal");
                    
                    for (int i = dsItems.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dsItems.Tables[0].Rows[i];
                        if (dr.RowState != DataRowState.Deleted && dr.RowState != DataRowState.Detached)
                        {
                            if (dr["ID"].ToString() == ID.Text)
                            {
                                //drow["Code"] = code.Text;
                                //if (BillID != string.Empty)
                                //    drow["BillID"] = Convert.ToInt32(BillID);
                                //drow["Description"] = txtItemDescription.Text;
                                //drow["Quantity"] = txtQuantity.Text;
                                //drow["Unit"] = txtUnit.Text;
                                //drow["Rate"] = txtRate.Text;
                                //drow["Discount"] = txtDiscount.Text;
                                //drow["Tax"] = txtTax.Text;
                                Int32.TryParse(StockID, out parseValueInt);
                                if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
                                {
                                    parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
                                }
                                if (txtCode.Text != string.Empty)
                                {
                                    dr["StockType"] = EnumClass.StockType.Receive;
                                    dr["StockID"] = parseValueInt;
                                    dr["Code"] = txtCode.Text;
                                    dr["Description"] = txtItemDescription.Text;
                                    dr["Unit"] = txtUnit.Text;

                                    float.TryParse(txtQuantity.Text, out parseValue);
                                    quantity = parseValue;
                                    dr["Quantity"] = parseValue;

                                    float.TryParse(lblOrderedQuantity.Text, out parseValue);
                                    dr["OrderedQuantity"] = parseValue;

                                    float.TryParse(lblReceivedQuantity.Text, out parseValue);
                                    dr["ReceivedQuantity"] = parseValue;


                                    float.TryParse(txtRate.Text, out parseValue);
                                    rate = parseValue;
                                    dr["Rate"] = parseValue;

                                    float.TryParse(txtDiscount.Text, out parseValue);
                                    discount = parseValue;
                                    dr["Discount"] = parseValue;

                                    float.TryParse(txtTax.Text, out parseValue);
                                    tax = parseValue;
                                    dr["Tax"] = parseValue;

                                    totalValue = quantity * rate;
                                    dr["SubTotal"] = totalValue;
                                    dr["TaxAmount"] = (totalValue * tax) / 100;
                                    dr["DiscountAmount"] = (totalValue * discount) / 100;
                                    totalValue = totalValue - ((totalValue * discount) / 100);
                                    totalValue = totalValue + ((totalValue * tax) / 100);
                                    dr["Total"] = totalValue;

                                    dr["ModifiedBy"] = Convert.ToInt32(user.ProviderUserKey.ToString());
                                    dr["ModifiedOn"] = DateTime.Now;


                                }
                                else
                                {
                                    //Remove row as the itemCode is empty
                                    dsItems.Tables[0].Rows[i].Delete();
                                }

                                break;
                            }
                        }
                    }
                
            }
            ViewState["Items"] = dsItems;
        }

        protected void ItemsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label refId = e.Row.FindControl("lblReferenceID") as Label;
                TextBox txtCode = (TextBox)e.Row.FindControl("txtCode");
                TextBox txtItemDescription = (TextBox)e.Row.FindControl("txtItemDescription");
                TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");               
                TextBox txtUnit = (TextBox)e.Row.FindControl("txtUnit");
                TextBox txtRate = (TextBox)e.Row.FindControl("txtRate");
                TextBox txtDiscount = (TextBox)e.Row.FindControl("txtDiscount");
                TextBox txtTax = (TextBox)e.Row.FindControl("txtTax");
                //If Salary is less than 10000 than set the row Background Color to Cyan  
                if (refId != null && !string.IsNullOrEmpty(refId.Text))
                {
                    txtCode.ReadOnly = true;
                    txtItemDescription.ReadOnly = true;
                    txtUnit.ReadOnly = true;
                    txtRate.ReadOnly = true;
                    txtDiscount.ReadOnly = true;
                    txtTax.ReadOnly = true;                     
                }
            }
        }  

        #endregion

        #region Handle Terms
        public void PopulateTermsGrid()
        {
            string stockID = null;
            try
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    stockID = Request.QueryString["ID"].ToString();
                }


                StockTerm OI = new StockTerm();
                DataSet ds = OI.GetStockTerms(stockID);
                ViewState["Terms"] = ds;
                TermsGridView.DataSource = ds;
                TermsGridView.DataBind();
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }

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
            try
            {
                DataSet ds = (DataSet)ViewState["Terms"];
                int currIndex, index = 0;
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted && dr.RowState != DataRowState.Detached)
                            {
                                currIndex = Convert.ToInt32(dr["ID"]);
                                if (currIndex > index)
                                {
                                    index = currIndex;
                                }
                            }
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
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }

        }
        protected void imgEditTerm_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            int index = -1;
            try
            {
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
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }
        protected void imgDeleteTerm_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            ArrayList aList = new ArrayList();
            try
            {
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
                    for (int index = 0; index < aList.Count; index++)
                    {
                        for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            //DataRow dr in dsItems.Tables[0].Rows
                            DataRow dr = ds.Tables[0].Rows[i];
                            if (dr.RowState != DataRowState.Deleted && dr.RowState != DataRowState.Detached)
                            {
                                if (dr["ID"].ToString() == aList[index].ToString())
                                {
                                    dr.Delete();
                                }
                            }
                        }
                    }
                    ViewState["Terms"] = ds;
                    TermsGridView.DataSource = ds;
                    TermsGridView.DataBind();
                    ITUP.Update();
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }
        protected void imgUpdateTerm_Click(object sender, EventArgs e)
        {
            int parseValueInt = 0;
            if (ViewState["TermKey"] != null)
            {
                try
                {
                    int index = Convert.ToInt32(ViewState["TermKey"]);

                    if ((TermsGridView.Rows[index].RowState & DataControlRowState.Edit) > 0)
                    {
                        Label id = (Label)TermsGridView.Rows[index].FindControl("ID");
                        TextBox txtStockID = (TextBox)TermsGridView.Rows[index].FindControl("txtStockID");
                        TextBox txtTermBox = (TextBox)TermsGridView.Rows[index].FindControl("TermBox");
                        TextBox txtConditionBox = (TextBox)TermsGridView.Rows[index].FindControl("ConditionBox");

                        DataSet ds = (DataSet)ViewState["Terms"];

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["ID"].ToString() == id.Text)
                            {
                                Int32.TryParse(txtStockID.Text, out parseValueInt);
                                if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
                                {
                                    parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
                                }
                                dr["StockID"] = parseValueInt;
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
                catch (Exception exc)
                {
                    Errors.LogError(exc);
                }
            }

        }
        protected void imgCancelTerm_Click(object sender, EventArgs e)
        {
            try
            {
                TermsGridView.EditIndex = -1;
                ActionTermBtnVisible(false);
                TermsGridView.DataSource = (DataSet)ViewState["Terms"];
                TermsGridView.DataBind();
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
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
                Errors.LogError(exc);
            }
        }


        #endregion

        public FilesData CopyUIToFilesData()
        {
            FilesData fs = new FilesData();
            
            if (fupload.HasFile)
            {
                fs.FileName = fupload.FileName;
                fs.FileSize = fupload.FileBytes.Length;
                fs.OleAttach = fupload.FileBytes;
                string ext = System.IO.Path.GetExtension(fs.FileName).ToLower();
                fs.FileType = CoreAssemblies.Core.GetMimeType(ext);
            }
            return fs;
        }
    }
}
