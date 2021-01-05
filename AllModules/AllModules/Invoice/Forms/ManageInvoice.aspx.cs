using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Inventory.BusLogic;
using System.Collections;
using ASM = CoreAssemblies;
using Billing.Data;
using Invoice.Data;
using AllModules;

namespace Invoice.Forms
{
    public partial class ManageInvoice : System.Web.UI.Page
    {
        private int itemfocusRow = 0;
        private int paymentfocusRow = 0;
        private bool FirstTimePopulated = false;
        private string billID = string.Empty;

        public string BillID
        {
            get { return billID; }
            set { billID = value; }
        }
        private string customerID = string.Empty;
        private Single total = 0,balance =0;

        public Single Total
        {
            get { return Convert.ToSingle(ViewState["total"]); ; }
            set { total = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, "");            
            billID=Request.QueryString["BillID"];
            customerID=Request.QueryString["CustomerID"];
            if (Page.IsPostBack)
            {
                // Bill
                if (Request["__EVENTTARGET"] == "itemNew")
                {
                    int index = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    index = index / 5;
                    UpdateGridViewDetailsToDataTable();
                    AddItems(index);
                    UpdateTotals();
                    RePopulateBothGrids();
                     
                }
                if (Request["__EVENTTARGET"] == "ResultRow")
                {
                    int index = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    index = index / 5;
                    UpdateGridViewDetailsToDataTable();
                    HiddenField id = (HiddenField)ItemsGridView.Rows[index].FindControl("lblID");
                    DeleteItems(id.Value,index);
                    UpdateTotals();
                    RePopulateBothGrids();

                }
                if (Request["__EVENTTARGET"] == "payNew")
                {
                    int index = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    index = index / 3;
                    UpdateGridViewDetailsToDataTable();
                    AddPaymentItems(index);
                    UpdateTotals();
                    RePopulateBothGrids();
                }
                if (Request["__EVENTTARGET"] == "payDel")
                {
                    int index = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    index = index / 3;
                    UpdateGridViewDetailsToDataTable();
                    DeletePaymentItems(index);
                    UpdateTotals();
                    RePopulateBothGrids();

                }
                if (Request["__EVENTTARGET"] == "Close")
                {            
                        ClearViewState();
                        Response.Redirect("~/Invoice/Forms/SearchInvoice.aspx");
                       
                }
                if (Request["__EVENTTARGET"] == "Save")
                {
                    UpdateGridViewDetailsToDataTable();
                    UpdateTotals();
                    if (string.IsNullOrEmpty(billID))
                        InsertData();
                    else
                        UpdateData();
                }
                if (Request["__EVENTTARGET"] == "Refresh")
                {
                    if (ViewState["datatable"] != null)
                    {
                        UpdateGridViewDetailsToDataTable();
                        UpdateTotals();
                        RePopulateBothGrids();
                    }
                }
                 
            }
            else
            {
                // First Time load
                // Get customer ID, and bill id if available
                ClearViewState();
                CreateNewBillItems(billID);
                CreateNewPaymentItems(billID);
                RePopulateBothGrids();
                FirstTimePopulated = true;
                PopulateCustomerDetails();
                ddlCustomer.Focus();
      
            }
        }
        public void ClearViewState()
        {
            ViewState["datatable"] = null;
            ViewState["pdatatable"] = null;
            ViewState["BillID"] = null;
            ViewState["total"] = null;
            ViewState["balance"] = null;
        }

        public void PopulateCustomerDetails()
        {
             
            Contact.BusLogic.Contacts contact = new Contact.BusLogic.Contacts();
            ddlCustomer.DataSource = contact.GetAllContacts();
            ddlCustomer.DataTextField = "Value";
            ddlCustomer.DataValueField = "Key";
            ddlCustomer.DataBind();
            if (!string.IsNullOrEmpty(customerID))
            {
                ddlCustomer.ClearSelection(); //making sure the previous selection has been cleared
                ddlCustomer.Items.FindByValue(customerID).Selected = true;
                ddlCustomer.Enabled = false;
            }
            if (!string.IsNullOrEmpty(BillID))
            {
                lblInvoice.Text = billID;
            }
            
        }

        public void UpdateTotals()
        {
            Single total = 0, PayTotal = 0, result=0;
            DataTable ItemsTable = (DataTable)ViewState["datatable"];
            DataTable PayTable = (DataTable)ViewState["pdatatable"];
            foreach (DataRow drow in ItemsTable.Rows)
            {
                if (drow.RowState != DataRowState.Deleted && drow.RowState != DataRowState.Detached)
                {
                    Single.TryParse(Convert.ToString(drow["SubTotal"]), out result);
                    total += result;
                }
            }
            result = 0;
            foreach (DataRow drow in PayTable.Rows)
            {
                if (drow.RowState != DataRowState.Deleted && drow.RowState != DataRowState.Detached)
                {
                    Single.TryParse(Convert.ToString(drow["Amount"]), out result);
                    PayTotal += result;
                    balance = total - PayTotal;
                    drow["Balance"] = balance;
                }
            }
            ViewState["total"] = total;
            ViewState["balance"] = balance;
        }

        public void UpdateGridViewDetailsToDataTable()
        {
            GetItemsGridViewDetails();
            GetPaymentGridViewDetails();
        }

        public void GetItemsGridViewDetails()
        {
            Single totalValue = 0.0F;
            Single parseValue = 0.0F;
            Single quantity = 0.0F;
            Single rate = 0.0F;
            DataTable dsItems = (DataTable)ViewState["datatable"];

            int billID = 0;
            string BillID = Convert.ToString(ViewState["BillID"]);
            Int32.TryParse(BillID, out billID);

            //todo: here the billid may not be there
            foreach (GridViewRow GRow in ItemsGridView.Rows)
            {
                HiddenField ID = (HiddenField)GRow.FindControl("lblID");
                TextBox txtCode = (TextBox)GRow.FindControl("txtCode");
                DropDownList ddlDescription = (DropDownList)GRow.FindControl("ddlDescription");
                TextBox txtQuantity = (TextBox)GRow.FindControl("txtQuantity");
                TextBox txtUnit = (TextBox)GRow.FindControl("txtUnit");
                TextBox txtRate = (TextBox)GRow.FindControl("txtRate");
                Label lblSubTotal = (Label)GRow.FindControl("lblSubTotal");

                for (int i = dsItems.Rows.Count - 1; i >= 0; i--)
                {

                    DataRow dr = dsItems.Rows[i];
                    if (dr["ID"].ToString() == ID.Value)
                    {

                        dr["BillID"] = billID;
                        dr["Code"] = ddlDescription.SelectedItem.Value;
                        dr["Description"] = ddlDescription.SelectedItem.Text;
                        dr["Unit"] = txtUnit.Text;

                        float.TryParse(txtQuantity.Text, out parseValue);
                        quantity = parseValue;
                        dr["Quantity"] = parseValue;

                        float.TryParse(txtRate.Text, out parseValue);
                        rate = parseValue;
                        dr["Rate"] = parseValue;
                        totalValue = quantity * rate;
                        dr["SubTotal"] = totalValue;
                    }
                }
            }


            ViewState["datatable"] = dsItems;
        }

        public void GetPaymentGridViewDetails()
        {
            Single parseValue = 0.0F;
            int billID = 0;
            DataTable dsItems = (DataTable)ViewState["pdatatable"];
            string BillID = Convert.ToString(ViewState["BillID"]);
            Int32.TryParse(BillID, out billID);

            foreach (GridViewRow GRow in PaymentGridView.Rows)
            {
                HiddenField ID = (HiddenField)GRow.FindControl("lblID");
                TextBox txtAmount = (TextBox)GRow.FindControl("txtAmount");
                Label lblBalance = (Label)GRow.FindControl("lblBalance");
                DropDownList payType = (DropDownList)GRow.FindControl("payType");
                for (int i = dsItems.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dsItems.Rows[i];
                    if (dr["ID"].ToString() == ID.Value)
                    {
                        if (txtAmount.Text != string.Empty)
                        {
                            dr["BillID"] = billID;
                            float.TryParse(txtAmount.Text, out parseValue);
                            dr["Amount"] = parseValue;
                            dr["Type"] = payType.SelectedValue;
                        }

                        break;
                    }
                }
            }


            ViewState["pdatatable"] = dsItems;
        }

        public void RePopulateBothGrids()
        {
            ItemsGridView.DataSource = (DataTable)ViewState["datatable"];
            ItemsGridView.DataBind();
            PaymentGridView.DataSource = (DataTable)ViewState["pdatatable"];
            PaymentGridView.DataBind();
        }

        public void InsertData()
        {
            try
            {

                BillData oData = new BillData();
                oData.ID = 0;
                oData.CustID = Convert.ToInt32(ddlCustomer.SelectedValue);
                oData.SubTotal = total;
                oData.BillDate = DateTime.Now;
                DataTable dsItems = (DataTable)ViewState["datatable"];
                DataTable dsPayments = (DataTable)ViewState["pdatatable"];
                string insertSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "INSERT", "Bill");
                string identitySql = "Select @@Identity";
                ArrayList sqlCommands = new ArrayList();
                sqlCommands.Add(insertSql);
                sqlCommands.Add(identitySql);
                if (dsItems != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new BillItemData(), dsItems, "BillItem"));
                if (dsPayments != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new PaymentData(), dsPayments, "Payment"));

                int x = Billing.Common.WebCommon.InsertBillToDbUsingTransaction(sqlCommands);
                 

                if (x < 0)
                {
                    //Error
                }
                else
                {
                    Response.Redirect("~/Invoice/Forms/ManageInvoice.aspx?BillID=" + x.ToString() + "&CustomerID=" + ddlCustomer.SelectedValue, false);
                    
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }

        public void UpdateData()
        {
            try
            {

                BillData oData = new BillData();
                oData.ID = Convert.ToInt32(billID);
                oData.BillDate = DateTime.Now;
                oData.CustID = Convert.ToInt32(customerID);
                oData.SubTotal = total;
                DataTable dsItems = (DataTable)ViewState["datatable"];
                DataTable dsPayments = (DataTable)ViewState["pdatatable"];
                ArrayList sqlCommands = new ArrayList();
                string updateSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "UPDATE", "Bill");
                sqlCommands.Add(updateSql);
                if (dsItems != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new BillItemData(), dsItems, "BillItem"));
                if (dsPayments != null)
                    sqlCommands.AddRange(ASM.Core.GetSQLFromDataTable(new PaymentData(), dsPayments, "Payment"));

                int[] x = Billing.Common.WebCommon.UpdateBillToDbUsingTransaction(sqlCommands);
                ClearViewState();
                if (x[0] < 0)
                {
                    //Error

                }
                else
                {
                    Response.Redirect("~/Invoice/Forms/ManageInvoice.aspx?BillID=" + billID +"&CustomerID="+customerID, false);
                }
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
            }
        }



        #region Items
        public void CreateNewBillItems(string billID)
        {
            Billing.BusLogic.BillItem bill = new Billing.BusLogic.BillItem();
            DataSet billds = bill.GetBillItems(billID);
            if(billds.Tables[0].Rows.Count==0)  
            ViewState["datatable"] = AddNewItemRow(billds.Tables[0]);
            else
            ViewState["datatable"] = billds.Tables[0];
            
        }
        public void AddItems(int rowcount)
        {
            // Check the new ID follow Stock
            DataTable dt = new DataTable();
            if (ViewState["datatable"] != null)
            {
                dt = (DataTable)ViewState["datatable"];
            }
            else
            {
                CreateNewBillItems(null);
                dt = (DataTable)ViewState["datatable"];
            }

            ViewState["datatable"] = AddNewItemRow(dt);
            itemfocusRow = rowcount + 1;
        }

        public DataTable AddNewItemRow(DataTable ds)
        {
            int currIndex, index = 0;
            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
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
            DataRow drNewRow = ds.NewRow();
            drNewRow["ID"] = index;
            drNewRow["Quantity"] = 0;
            drNewRow["Rate"] = 0;
            ds.Rows.Add(drNewRow);
            return ds;
        }

        public void DeleteItems(string ID, int rowIndex)
        {
            // Check the new ID follow Stock
            DataTable dt = new DataTable();

            if (ViewState["datatable"] != null)
            {
                dt = (DataTable)ViewState["datatable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        //DataRow dr in dsItems.Tables[0].Rows
                        DataRow dr = dt.Rows[i];
                        if (dr.RowState != DataRowState.Deleted && dr.RowState != DataRowState.Detached)
                        {
                            if (dr["ID"].ToString() == ID)
                            {
                                dr.Delete();
                                break;
                            }
                        }
                    }
                    ViewState["datatable"] = dt;
                    if (dt.Rows.Count == 0)
                    {
                        ViewState["datatable"] = AddNewItemRow(dt);
                    }
                    if (rowIndex > 0)
                    {
                        itemfocusRow = rowIndex - 1;
                    }
                    
                }
                
              
            }

        }

        protected void ItemsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCode = (HiddenField)e.Row.FindControl("hdnCode");
                if (e.Row.RowIndex == itemfocusRow && !FirstTimePopulated)
                {
                    DropDownList ddlList = (DropDownList)e.Row.FindControl("ddlDescription");
                    if (ddlList != null)
                    {
                        ddlList.Focus();
                    }
                }
                DropDownList ddlDescription = (DropDownList)e.Row.FindControl("ddlDescription");              
                ProductMaster oi = new ProductMaster();
                DataSet ds = (DataSet)oi.GetProductMasterForCache(null);
                Hashtable hTable = new Hashtable();
                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dRow in ds.Tables[0].Rows)
                    {
                        hTable.Add(dRow["Code"].ToString(), dRow["Description"].ToString());
                    }
                }
                ddlDescription.DataSource = hTable;
                ddlDescription.DataTextField = "Value";
                ddlDescription.DataValueField = "Key";
                ddlDescription.DataBind();
                if (!string.IsNullOrEmpty(hdnCode.Value))
                {
                    //ddlDescription.SelectedIndex = ddlDescription.Items.IndexOf(ddlDescription.Items.FindByText(hdnCode.Value));
                    //ddlDescription.SelectedItem.Value = hdnCode.Value;
                    ddlDescription.ClearSelection(); //making sure the previous selection has been cleared
                    ddlDescription.Items.FindByValue(hdnCode.Value).Selected = true;
                }
            }
        }
        #endregion

        #region Payment
        public void CreateNewPaymentItems(string billID)
        {
            Invoice.BusLogic.Payment payment = new Invoice.BusLogic.Payment();
            DataSet billds = payment.GetPayments(billID);
            if (billds.Tables[0].Rows.Count == 0)
                ViewState["pdatatable"] = AddNewPaytmentItemRow(billds.Tables[0]);
            else
                ViewState["pdatatable"] = billds.Tables[0];
             
        }
        public DataTable AddPaymentItems(int rowcount)
        {
            DataTable dt = new DataTable();
            if (ViewState["pdatatable"] != null)
            {
                dt = (DataTable)ViewState["pdatatable"];
            }
            else
            {
                CreateNewPaymentItems(null);
                dt = (DataTable)ViewState["pdatatable"];
            }

            ViewState["pdatatable"] = AddNewPaytmentItemRow(dt);
            paymentfocusRow = rowcount + 1; 
            return dt;
        }
        public DataTable AddNewPaytmentItemRow(DataTable ds)
        {
            int currIndex, index = 0;
            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
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
            DataRow drNewRow = ds.NewRow();
            drNewRow["ID"] = index;
            drNewRow["Amount"] = 0;
            drNewRow["Balance"] = 0;
            ds.Rows.Add(drNewRow);
            return ds;
        }

        public void DeletePaymentItems(int rowIndex)
        {
            DataTable dt = new DataTable();

            if (ViewState["pdatatable"] != null)
            {
                dt = (DataTable)ViewState["pdatatable"];

                if (dt.Rows.Count > 0)
                {
                    dt.Rows[rowIndex].Delete();

                    ViewState["pdatatable"] = dt;
                    if (rowIndex > 0)
                    {
                        paymentfocusRow = rowIndex - 1;
                    }
                    else if (dt.Rows.Count == 0)
                    {
                        ViewState["pdatatable"] = AddNewPaytmentItemRow(dt);
                    }
                   
                }

            }

        }

        protected void PaymentGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                 
                HiddenField hdnPayType = (HiddenField)e.Row.FindControl("hdnPayType");
                DropDownList ddl = (DropDownList)e.Row.FindControl("payType");
                if (!string.IsNullOrEmpty(hdnPayType.Value))
                {
                    ddl.ClearSelection(); //making sure the previous selection has been cleared
                    ddl.Items.FindByValue(hdnPayType.Value).Selected = true;
                }
                if (e.Row.RowIndex == paymentfocusRow && !FirstTimePopulated)
                { ddl.Focus(); }

            }
        }
        #endregion
    }
}
