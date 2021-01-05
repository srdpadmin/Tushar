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

namespace PurchaseOrder.Forms
{
    public partial class SearchPurchases : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // check if any id has any text set in filter, and show pdf icon for download
        }


        protected void SearchBtnClick(object sender, EventArgs e)
        {
            PurchaseOrderGridView.DataBind();
           
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

        protected void PurchaseOrderGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjectDataSource ctrl = e.Row.FindControl("OIDODS") as ObjectDataSource;

                if (ctrl != null && e.Row.DataItem != null)
                {
                    ctrl.SelectParameters["PurchaseID"].DefaultValue = ((DataRowView)e.Row.DataItem)["ID"].ToString();
                    //GridView igv = e.Row.FindControl("ItemsGridView") as GridView;
                    //igv.DataBind();
                }
            }
        }

        protected void PurchaseOrderGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            //CustomerID is stored as event Commend Argument
            
            if (e.CommandName == "View")
            {
                string Id = (string)e.CommandArgument;
                Response.Redirect("~/PurchaseOrder/Forms/ManageOrder.aspx?ID=" + Id);
            }
           
        }

        protected void ItemGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            string Id = (string)e.CommandArgument;
            //CustomerID is stored as event Commend Argument
            if (e.CommandName == "Receipt")
            {
                Response.Redirect("~/PurchaseOrder/Forms/ManageItemTransaction.aspx?Receipt=" + Id);
            }
            else
            {
                Response.Redirect("~/PurchaseOrder/Forms/ManageItemTransaction.aspx?Dispatch=" + Id);
            }
        }

        protected void PurchaseOrderGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PurchaseOrderGridView.PageIndex = e.NewPageIndex;
            PurchaseOrderGridView.DataBind();
        }
        protected void btnSelect_Click(object sender, EventArgs e)
        {

            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            PurchaseOrderGridView.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            PurchaseOrderGridView.SelectedIndex = row.RowIndex;

        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            GridViewRow row = PurchaseOrderGridView.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string ID = Convert.ToString(PurchaseOrderGridView.DataKeys[row.RowIndex].Value);
                PurchaseOrder.BusLogic.Purchase Purchase = new PurchaseOrder.BusLogic.Purchase();
                int[] ids=Purchase.DeletePurchase(ID);
                if (ids[0] >0)
                {
                    PurchaseOrderGridView.DataBind();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "PurchaseOrder deleted successfully" + "');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Problem deleting PurchaseOrder" + "');", true);

                }
            }
        }
    }
}
