using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Inventory.Forms
{
    public partial class StockLedger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SearchBtnClick(object sender, EventArgs e)
        {
            InventoryGridView.DataBind();

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

        protected void InventoryGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjectDataSource ctrl = e.Row.FindControl("OIDODS") as ObjectDataSource;

                if (ctrl != null && e.Row.DataItem != null)
                {
                    ctrl.SelectParameters["StockID"].DefaultValue = ((DataRowView)e.Row.DataItem)["ID"].ToString();
                    ctrl.SelectParameters["StockType"].DefaultValue = ((DataRowView)e.Row.DataItem)["StockType"].ToString();
                    //GridView igv = e.Row.FindControl("ItemsGridView") as GridView;
                    //igv.DataBind();
                }
            }
        }

        protected void InventoryGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            //CustomerID is stored as event Commend Argument

            if (e.CommandName == "View")
            {
                string Id = (string)e.CommandArgument;
                Response.Redirect("~/Inventory/Forms/ManageStock.aspx?ID=" + Id);
            }

        }

        protected void ItemGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            string Id = (string)e.CommandArgument;
            //CustomerID is stored as event Commend Argument
            //if (e.CommandName == "Receipt")
            //{
            //    Response.Redirect("~/Inventory/Forms/ManageItemTransaction.aspx?Receipt=" + Id);
            //}
            //else
            //{
            //    Response.Redirect("~/Inventory/Forms/ManageItemTransaction.aspx?Dispatch=" + Id);
            //}
        }

        protected void InventoryGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            InventoryGridView.PageIndex = e.NewPageIndex;
            InventoryGridView.DataBind();
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {

            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            InventoryGridView.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            InventoryGridView.SelectedIndex = row.RowIndex;

        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            GridViewRow row = InventoryGridView.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string ID = Convert.ToString(InventoryGridView.DataKeys[row.RowIndex].Value);
                Inventory.BusLogic.Stock stock= new Inventory.BusLogic.Stock();
                int[] ids = stock.DeleteStock(ID);
                if (ids[0] > 0)
                {
                    InventoryGridView.DataBind();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Stock Entry deleted successfully" + "');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Problem deleting Stock Entry" + "');", true);

                }
            }
        }
    }
}
