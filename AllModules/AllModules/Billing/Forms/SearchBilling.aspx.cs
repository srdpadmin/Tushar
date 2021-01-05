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

namespace Billing.Forms
{
    public partial class SearchBilling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SearchBtnClick(object sender, EventArgs e)
        {
            BillingGridView.DataBind();

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

        protected void ClearBtnClick(object sender, EventArgs e)
        {
            ClearAllText();
        }

        protected void BillingGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjectDataSource ctrl = e.Row.FindControl("OIDODS") as ObjectDataSource;

                if (ctrl != null && e.Row.DataItem != null)
                {
                    ctrl.SelectParameters["BillID"].DefaultValue = ((DataRowView)e.Row.DataItem)["ID"].ToString();
                    //GridView igv = e.Row.FindControl("ItemsGridView") as GridView;
                    //igv.DataBind();
                }
            }
        }

        protected void BillingGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            //CustomerID is stored as event Commend Argument
            
            if (e.CommandName == "View")
            {
                string Id = (string)e.CommandArgument;
                Response.Redirect("~/Billing/Forms/ManageBilling.aspx?ID=" + Id);
            }
           
        }

        protected void ItemGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            string Id = (string)e.CommandArgument;
            //CustomerID is stored as event Commend Argument
            if (e.CommandName == "Receipt")
            {
                Response.Redirect("~/Billing/Forms/ManageItemTransaction.aspx?Receipt=" + Id);
            }
            else
            {
                Response.Redirect("~/Billing/Forms/ManageItemTransaction.aspx?Dispatch=" + Id);
            }
        }

        protected void BillingGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BillingGridView.PageIndex = e.NewPageIndex;
            BillingGridView.DataBind();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {

            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            BillingGridView.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            BillingGridView.SelectedIndex = row.RowIndex;

        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            GridViewRow row = BillingGridView.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string ID = Convert.ToString(BillingGridView.DataKeys[row.RowIndex].Value);
                Billing.BusLogic.Bill bill = new Billing.BusLogic.Bill();
                int[] ids = bill.DeleteBill(ID);
                if (ids[0] > 0)
                {
                    BillingGridView.DataBind();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Bill deleted successfully" + "');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Problem deleting Bill" + "');", true);

                }
            }
        }
    }
}
