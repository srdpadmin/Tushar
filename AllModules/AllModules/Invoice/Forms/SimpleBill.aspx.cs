using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Inventory.BusLogic;
using Billing.BusLogic;
using System.Collections;

namespace Invoice.Forms
{
    public partial class SimpleBill : System.Web.UI.Page
    {
        private int itemfocusRow = 0;
        private int paymentfocusRow = 0;
        private bool bringFocusCustomer = false;
        private bool bringFocusOnResults = false;
        private string focusClientID = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            ClientScript.GetPostBackEventReference(this, "");
            if (Page.IsPostBack)
            {
                // Bill
                if (Request["__EVENTTARGET"] == "itemNew")
                {
                    int index = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    index = index / 5;
                    UpdateGridViewDetailsToDataTable();
                    AddItems(index);
                     
                }
                if (Request["__EVENTTARGET"] == "itemDel")
                {
                    int index = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    index = index / 5;
                    UpdateGridViewDetailsToDataTable();
                    HiddenField id = (HiddenField)ItemsGridView.Rows[index].FindControl("lblID");
                    DeleteItems(id.Value,index);

                }
                if (Request["__EVENTTARGET"] == "payNew")
                {
                    int index = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    index = index / 3;
                    UpdateGridViewDetailsToDataTable();
                    AddPaymentItems(index);
                    
                }
                if (Request["__EVENTTARGET"] == "payDel")
                {
                    int index = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    index = index / 3;
                    UpdateGridViewDetailsToDataTable();
                    DeletePaymentItems(index);

                }
                /// Search
                if (Request["__EVENTTARGET"] == "LookupBox")
                {
                    int customerID = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    if (customerID != 0)
                    {
                        ClearViewState();
                        PopulateBills(customerID);
                        pnlShowHide(true);
                        bringFocusOnResults = true;
                        NewBill.Visible = true;
                    }
                }
                if (Request["__EVENTTARGET"] == "ResultRow")
                {
                    int rowIndex = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                    rowIndex = rowIndex /5;
                    GridViewRow gRow = navigate.Rows[rowIndex];
                    HiddenField hdn = (HiddenField)gRow.FindControl("ID");
                    string billID = hdn.Value;
                    pnlShowHide(false);
                    CreateNewBillItems(billID);
                    CreateNewPaymentItems(billID);
                    ViewState["BillID"] = billID;
                }
                if (Request["__EVENTTARGET"] == "CloseBill")
                {
                    if (ViewState["datatable"] != null)
                    {
                        UpdateGridViewDetailsToDataTable();

                        ClearViewState();
                        pnlShowHide(true);
                        bringFocusOnResults = true;
                    }
                }
            }
            else
            {
                ClearViewState();
                pnlShowHide(true);
                NewBill.Visible = false;
            }
        }

        public void ClearViewState()
        {
            ViewState["datatable"] = null;
            ViewState["pdatatable"] = null;
            ViewState["BillID"] = null;
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
                            dr["Code"] = txtCode.Text;
                            dr["Description"] = ddlDescription.SelectedValue;
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
                             dr["ReferenceID"] = billID;
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

        public void pnlShowHide(bool val)
        {
            if (val)
            {
                pnlSearch.Visible = true;
                pnlCreateBill.Visible = false;
            }
            else
            {
                pnlSearch.Visible = false;
                pnlCreateBill.Visible = true;
            }
        }

        protected void NewBill_Click(object sender, EventArgs e)
        {
            CreateNewBillItems(null);
            CreateNewPaymentItems(null);
            pnlShowHide(false);
        }

        #region Search
        protected void navigate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == itemfocusRow)
                {
                    Label txtBox = (Label)e.Row.FindControl("company");
                    if (txtBox != null)
                    {
                        focusClientID = txtBox.ClientID;
                    }
                }
            }
        }
        protected void LookupBox_TextChanged(object sender, EventArgs e)
        {
            string Text = LookupBox.Text;
        }
        public void PopulateBills(int customerID)
        {
            Billing.BusLogic.Bill bill = new Billing.BusLogic.Bill();
            navigate.DataSource = bill.GetBills(null, null, null, LookupBox.Text, null, null);
            navigate.DataBind();
        }

        #endregion


        #region Items
        public void CreateNewBillItems(string billID)
        {
            Billing.BusLogic.BillItem bill = new Billing.BusLogic.BillItem();
            DataSet billds = bill.GetBillItems(billID);
            DataRow drow = billds.Tables[0].NewRow();
            ViewState["datatable"]  = AddNewItemRow(billds.Tables[0]);
            ItemsGridView.DataSource = (DataTable)ViewState["datatable"];
            ItemsGridView.DataBind();  
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

            ItemsGridView.DataSource = (DataTable)ViewState["datatable"];
            ItemsGridView.DataBind();
           

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
                    if (rowIndex > 0)
                    {
                        itemfocusRow = rowIndex - 1;
                    }           
                }
                if (dt.Rows.Count == 0)
                {
                    DataRow drow = dt.NewRow();
                    dt.Rows.Add(drow);
                    ViewState["pdatatable"] = dt;
                }
                ItemsGridView.DataSource = dt;
                ItemsGridView.DataBind();
            }

        }

        protected void ItemsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow )
            {
                if (e.Row.RowIndex == itemfocusRow)
                {
                    TextBox txtBox = (TextBox)e.Row.FindControl("txtCode");
                    if (txtBox != null)
                    {
                        txtBox.Focus();
                    }
                }
                DropDownList ddlDescription = (DropDownList)e.Row.FindControl("ddlDescription");
                HiddenField hdnDescription = (HiddenField)e.Row.FindControl("hdnDescription");
                ProductMaster oi = new ProductMaster();
                DataSet ds = (DataSet)oi.GetProductMasterForCache(null);
                Hashtable hTable = new Hashtable();
                if (ds.Tables[0].Rows.Count>0)
                {
                    
                    foreach(DataRow dRow in ds.Tables[0].Rows)
                    {
                        hTable.Add(dRow["Code"].ToString(), dRow["Description"].ToString());
                    }
                }
                ddlDescription.DataSource = hTable;
                ddlDescription.DataTextField = "Value";
                ddlDescription.DataValueField = "Key";
                ddlDescription.DataBind();

            }
        }
        #endregion

        #region Payment
        public void CreateNewPaymentItems(string billID)
        {
            Invoice.BusLogic.Payment payment = new Invoice.BusLogic.Payment();
            DataSet billds = payment.GetPayments(billID);
            DataRow drow = billds.Tables[0].NewRow();
            billds.Tables[0].Rows.Add(drow);
            ViewState["pdatatable"] = billds.Tables[0];
           
            PaymentGridView.DataSource = billds.Tables[0];
            PaymentGridView.DataBind();             
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
            DataRow drow = dt.NewRow();
            dt.Rows.Add(drow);
            ViewState["pdatatable"] = dt;
            paymentfocusRow = rowcount + 1;

            PaymentGridView.DataSource = dt;
            PaymentGridView.DataBind();
            return dt;
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
                        DataRow drow = dt.NewRow();
                        dt.Rows.Add(drow);
                        ViewState["pdatatable"] = dt;
                    }
                    PaymentGridView.DataSource = dt;
                    PaymentGridView.DataBind();
                }

            }

        }

        protected void PaymentGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == paymentfocusRow)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("payType");
                if (ddl != null)
                {
                    ddl.Focus();
                }

            }
        }
        #endregion

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (bringFocusCustomer)
            {
                LookupBox.Focus();
            }
            if (bringFocusOnResults && focusClientID != null)
            {
                string url = "FocusNavigate(" + focusClientID + ");";
                ScriptManager.RegisterStartupScript(Page, typeof(string), "", url, true);
            }
        }
    }
}
