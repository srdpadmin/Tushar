using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Billing.Data;
using WC = Billing.Common.WebCommon;
using System.Collections;
using Billing.BusLogic ;
using System.Data;
using ASM = CoreAssemblies;
using System.Web.Security;
using AllModules;
using CoreAssemblies;

namespace Billing.Forms
{
    public partial class ManageBilling : System.Web.UI.Page
    {
        #region Private Variables

        BillData oData = null;
        private bool isEditMode = false;
        protected bool IsInEditMode
        {
            get
            {
                 
                this.isEditMode =ViewState["IsEditMode"] !=null ? (bool)ViewState["IsEditMode"]:false;
                return this.isEditMode; }
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
           
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
            

        }

        protected void CheckStatus()
        { 
            Billing.BusLogic.Bill bill = new Billing.BusLogic.Bill();
            EnumClass.Status status = (EnumClass.Status)bill.GetBillStatus(RequestBillID);
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
               
                btnPDFPrint.Visible = false;
                btnExcelPrint.Visible = false;
                btnAmend.Visible = false;
                btnUpdate.Visible = false;
                btnSubmit.Visible = false;
                btnInsert.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                btnPDFPrint.Visible = true;
                btnExcelPrint.Visible = true;

                btnAmend.Visible = true;               
                btnCancel.Visible = false;
                btnSubmit.Visible = true;

                btnInsert.Visible = false;
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
              
                BillData oData = CopyUIToBillDataObject();
                DataSet dsItems = (DataSet)ViewState["Items"];
                DataSet dsTerms = (DataSet)ViewState["Terms"];
                ArrayList sqlCommands = new ArrayList();
                string updateSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "UPDATE", "Bill");
                sqlCommands.Add(updateSql);
                if (dsItems != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new BillItemData(), dsItems.Tables[0], "BillItem"));
                if (dsTerms != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new BillTermData(), dsTerms.Tables[0], "BillTerm"));

                int[] x = WC.UpdateBillToDbUsingTransaction(sqlCommands);
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
             Billing.BusLogic.Bill bill = new Billing.BusLogic.Bill();
            EnumClass.Status status = (EnumClass.Status)bill.GetBillStatus(RequestBillID);
            if (status == EnumClass.Status.Draft)
            {
                bill.UpdateBillStatus(RequestBillID);
                // Do not delete this code, its working fine
                // When required use it
                // Get all the items from the items list and 
                // create new ProductMasterTransactions for each item
                // Save those transactions in ProductMasterTransactions
                //if (Inventory.Common.WebCommon.InsertBillItemsToProductMasterTransactions(RequestBillID))
                //{
                   
                //    CheckStatus();
                //}
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
                Response.Redirect("~/Billing/Forms/SearchBilling.aspx",false);
            }
            else if (ODView.CurrentMode == DetailsViewMode.ReadOnly)
            {
                Response.Redirect("~/Billing/Forms/SearchBilling.aspx",false);
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
             
                BillData oData = CopyUIToBillDataObject();
                //todo set createdby for oData to userid
               
                DataSet dsItems = (DataSet)ViewState["Items"];
                DataSet dsTerms = (DataSet)ViewState["Terms"];
                string insertSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "INSERT", "Bill");
                string identitySql = "Select @@Identity";
                ArrayList sqlCommands = new ArrayList();
                sqlCommands.Add(insertSql);
                sqlCommands.Add(identitySql);
                if(dsItems !=null)
                sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new BillItemData(), dsItems.Tables[0], "BillItem"));
                if (dsTerms != null)
                sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new BillTermData(), dsTerms.Tables[0], "BillTerm"));

                int x = WC.InsertBillToDbUsingTransaction(sqlCommands);
                ViewState["Items"] = null;
                ViewState["Terms"] = null;
                ViewState["ItemKey"] = null;

                if (x < 0)
                {
                    //Error
                }
                else
                {
                    Response.Redirect("~/Billing/Forms/ManageBilling.aspx?ID=" + x.ToString(),false);
                    ODView.DataBind();
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

        public BillData CopyUIToBillDataObject()
        {
            int temp = 0;
            oData = new BillData();
            try
            {
            Label CustomerId = ODView.Rows[0].FindControl("hdnCustomerID") as Label;
            TextBox refId = ODView.Rows[0].FindControl("txtReferencePO") as TextBox;
          //  TextBox requestedFor = ODView.Rows[0].FindControl("txtrequestedFor") as TextBox;
            TextBox impNote = ODView.Rows[0].FindControl("txtImportantNote") as TextBox;
            TextBox billDate = ODView.Rows[0].FindControl("txtBillDate") as TextBox;
            
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
                oData.Revision  = temp + 1;
                oData.CreatedBy = Convert.ToInt32(hdnCreatedBy.Value);
                oData.AmendReason  = amendReason.Text;
                //oData.RequestedFor = RequestedFor.Text;
                oData.ModifiedOn = DateTime.Now;
                oData.AmendedBy = Convert.ToInt32(user.ProviderUserKey.ToString());
                oData.CreatedOn = DateTime.ParseExact(hdnCreatedOn.Value, "dd/MM/yyyy", null);
                 
            }
            else
            {
                oData.CreatedOn = DateTime.Now;
                oData.CreatedBy = Convert.ToInt32(user.ProviderUserKey.ToString()); 
            }
            if (billDate.Text != string.Empty)
            {
                oData.BillDate = DateTime.ParseExact(billDate.Text, "dd/MM/yyyy", null);
            }
            else
            {
                oData.BillDate = DateTime.Now;
            }
            Int32.TryParse(CustomerId.Text, out temp);
            oData.CustID = temp;
            oData.ReferenceID = refId.Text;
           // oData.RequestedFor = requestedFor.Text;
           // oData. = impNote.Text;
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
            string BillID = null;
            try
            {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                BillID = Request.QueryString["ID"].ToString();
            }

            BillItem OI = new BillItem();
            DataSet ds = OI.GetBillItems(BillID);
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
            int currIndex,index = 0;
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
                    lbl=(Label)dr.FindControl("lblID"); 
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
            //    if ((ItemsGridView.Rows[index].RowState & DataControlRowState.Edit) > 0)
            //    {
            //        Label id = (Label)ItemsGridView.Rows[index].FindControl("ID");
            //        TextBox txtBillID = (TextBox)ItemsGridView.Rows[index].FindControl("txtBillID");
            //        TextBox txtCode = (TextBox)ItemsGridView.Rows[index].FindControl("txtCode");
            //        TextBox txtItemDescription = (TextBox)ItemsGridView.Rows[index].FindControl("txtItemDescription");
            //        TextBox txtQuantity = (TextBox)ItemsGridView.Rows[index].FindControl("txtQuantity");
            //        TextBox txtUnit = (TextBox)ItemsGridView.Rows[index].FindControl("txtUnit");
            //        TextBox txtRate = (TextBox)ItemsGridView.Rows[index].FindControl("txtRate");
            //        TextBox txtDiscount = (TextBox)ItemsGridView.Rows[index].FindControl("txtDiscount");
            //        TextBox txtTax = (TextBox)ItemsGridView.Rows[index].FindControl("txtTax");
            //        Label txtTaxAmount = (Label)ItemsGridView.Rows[index].FindControl("lblTaxAmount");
            //        Label txtTotalAmount = (Label)ItemsGridView.Rows[index].FindControl("lblTotalAmount");

            //        DataSet ds = (DataSet)ViewState["Items"];

            //        foreach (DataRow dr in ds.Tables[0].Rows)
            //        {
            //            if (dr["ID"].ToString() == id.Text)
            //            {
            //                Int32.TryParse(txtBillID.Text, out parseValueInt);
            //                if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
            //                {
            //                    parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
            //                }
            //                dr["BillID"] = parseValueInt;
            //                dr["Code"] = txtCode.Text;
            //                dr["Description"] = txtItemDescription.Text;
            //                float.TryParse(txtQuantity.Text, out parseValue);
            //                dr["Quantity"] = parseValue;
            //                dr["Balance"] = parseValue;
            //                totalValue = parseValue;
            //                dr["Unit"] = txtUnit.Text;
            //                float.TryParse(txtRate.Text, out parseValue);
            //                dr["Rate"] = parseValue;
            //                totalValue = totalValue * parseValue;
            //                float.TryParse(txtDiscount.Text, out parseValue);
            //                dr["Discount"] = parseValue;
            //                if (totalValue > 0)
            //                    totalValue = totalValue - ((totalValue * parseValue) / 100);
            //                float.TryParse(txtTax.Text, out parseValue);
            //                dr["Tax"] = parseValue;
            //                if (totalValue > 0)
            //                {
            //                    totalValue = totalValue - ((totalValue * parseValue) / 100);
            //                    dr["TaxAmount"] = (totalValue * parseValue) / 100;
            //                }
                            
            //                dr["TotalAmount"] = totalValue;

            //                break;
            //            }
            //        }
            //        ViewState["Items"] = ds;
            //        // change the editindex and rebind
            //        ItemsGridView.EditIndex = -1;
            //        //ItemsBtnPanel.Visible = false;
            //        //TermsBtnPanel.Visible = false;
            //        ActionItemBtnVisible(false);
            //        ItemsGridView.DataSource = (DataSet)ViewState["Items"];
            //        ItemsGridView.DataBind();
            //        CalculateBillTotals();
            //        ITUP.Update();
            //    }
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
            int parseValueInt = 0;
            DataSet dsItems = (DataSet)ViewState["Items"];
            MembershipUser user = Membership.GetUser();
            foreach (GridViewRow GRow in ItemsGridView.Rows)
            {
                Label ID = (Label)GRow.FindControl("lblID");
                string BillID = RequestBillID;
                TextBox txtCode = (TextBox)GRow.FindControl("txtCode");
                TextBox txtItemDescription = (TextBox)GRow.FindControl("txtItemDescription");
                TextBox txtQuantity = (TextBox)GRow.FindControl("txtQuantity");
                TextBox txtUnit = (TextBox)GRow.FindControl("txtUnit");
                TextBox txtRate = (TextBox)GRow.FindControl("txtRate");
                TextBox txtDiscount = (TextBox)GRow.FindControl("txtDiscount");
                TextBox txtTax = (TextBox)GRow.FindControl("txtTax");
                Label lblTotalTaxAmount = (Label)GRow.FindControl("lblTotalTaxAmount");
                Label lblTotalAmount = (Label)GRow.FindControl("lblTotalAmount");
                Label lblTotalDiscountAmount = (Label)GRow.FindControl("lblTotalDiscountAmount");

                for (int i = dsItems.Tables[0].Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dsItems.Tables[0].Rows[i];
                    if (dr.RowState != DataRowState.Deleted && dr.RowState != DataRowState.Detached)
                    {                        
                        if (dr["ID"].ToString() == ID.Text)
                        {

                            Int32.TryParse(BillID, out parseValueInt);
                            if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
                            {
                                parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
                            }
                            if (txtCode.Text != string.Empty)
                            {
                                dr["BillID"] = parseValueInt;
                                dr["Code"] = txtCode.Text;
                                dr["Description"] = txtItemDescription.Text;
                                dr["Unit"] = txtUnit.Text;

                                float.TryParse(txtQuantity.Text, out parseValue);
                                quantity = parseValue;
                                dr["Quantity"] = parseValue;


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

        #endregion

        #region Handle Terms
        public void PopulateTermsGrid()
        {
            string billID = null;
            try
            {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                billID = Request.QueryString["ID"].ToString();
            }
            

            BillTerm OI = new BillTerm();
            DataSet ds = OI.GetBillTerms(billID);
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
            int currIndex,index = 0;
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
            try{
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
            try{
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
                try{
                int index = Convert.ToInt32(ViewState["TermKey"]);

                if ((TermsGridView.Rows[index].RowState & DataControlRowState.Edit) > 0)
                {
                    Label id = (Label)TermsGridView.Rows[index].FindControl("ID");
                    TextBox txtBillID = (TextBox)TermsGridView.Rows[index].FindControl("txtBillID");
                    TextBox txtTermBox = (TextBox)TermsGridView.Rows[index].FindControl("TermBox");
                    TextBox txtConditionBox = (TextBox)TermsGridView.Rows[index].FindControl("ConditionBox");

                    DataSet ds = (DataSet)ViewState["Terms"];

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["ID"].ToString() == id.Text)
                        {
                            Int32.TryParse(txtBillID.Text, out parseValueInt);
                            if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
                            {
                                parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
                            }
                            dr["BillID"] = parseValueInt;
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
            try{
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

    }
}