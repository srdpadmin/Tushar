using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Invoice.Forms
{
    public partial class SearchInvoice : System.Web.UI.Page
    {
        private bool bringFocusCustomer = false;
        private bool bringFocusOnResults = false;
        private string focusClientID = null;
        protected void Page_Load(object sender, EventArgs e)
        {
              ClientScript.GetPostBackEventReference(this, "");
              if (Page.IsPostBack)
              {
                  /// Search
                  if (Request["__EVENTTARGET"] == "LookupBox")
                  {
                      string customerID = Request["__EVENTARGUMENT"];
                      if (!string.IsNullOrEmpty(customerID))
                      {
                          ClearViewState();
                          PopulateBills(customerID);
                          CustID.Value = Convert.ToString(customerID);
                          bringFocusOnResults = true;
                           
                      }
                  }
                  if (Request["__EVENTTARGET"] == "ResultRow")
                  {
                      int billId = Convert.ToInt32(Request["__EVENTARGUMENT"]);
                      if (billId != 0)
                      {
                          ClearViewState();
                          string customerID = CustID.Value;
                          Response.Redirect("~/Invoice/Forms/ManageInvoice.aspx?BillID=" + billId + "&CustomerID=" + customerID, false);
                   
                      }
                  }
                  if (Request["__EVENTTARGET"] == "New")
                  {
                      Response.Redirect("~/Invoice/Forms/ManageInvoice.aspx");
                  }
              }
              else
              {
                  ClearViewState();
                  //PopulateBills(string.Empty);
              }
        }
        public void ClearViewState()
        {
            ViewState["datatable"] = null;
            ViewState["pdatatable"] = null;
            ViewState["BillID"] = null;
        }
        protected void NewBill_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Invoice/Forms/ManageInvoice.aspx");
        }

        protected void navigate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             
        }
        protected void LookupBox_TextChanged(object sender, EventArgs e)
        {
            string Text = LookupBox.Text;
        }
        public void PopulateBills(string customerID)
        {
            Billing.BusLogic.Bill bill = new Billing.BusLogic.Bill();
            navigate.DataSource = bill.GetBillByCustomerID(customerID);
            //navigate.DataSource = bill.GetBills(null, null, null, LookupBox.Text, null, null);
            navigate.DataBind();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (bringFocusCustomer)
            {
                LookupBox.Focus();
            }
            if (bringFocusOnResults)
            {
                if (navigate.Rows.Count > 0)
                {
                    FocussedChild.Value = navigate.Rows[0].Controls[0].FindControl("billID").ClientID;
                    string url = "FocusNavigate(" + navigate.Rows[0].Controls[0].Controls[0].ClientID + ");";
                    //navigate.Rows[1].Controls[0].Focus();
                    //LookupBox.Focus();
                    //if (!Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType(), "myscript"))                      
                    //ScriptManager.RegisterStartupScript(Page, typeof(string), "myscript", url, true);
                }
            }
        }
        

    }
}
