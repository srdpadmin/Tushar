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

namespace Quotation.Forms
{
    public partial class SearchQuotes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // check if any id has any text set in filter, and show pdf icon for download
        }


        protected void SearchBtnClick(object sender, EventArgs e)
        {
            QuotationGridView.DataBind();
           
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

        protected void QuotationGridView_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjectDataSource ctrl = e.Row.FindControl("OIDODS") as ObjectDataSource;

                if (ctrl != null && e.Row.DataItem != null)
                {
                    ctrl.SelectParameters["QuoteID"].DefaultValue = ((DataRowView)e.Row.DataItem)["ID"].ToString();
                    //GridView igv = e.Row.FindControl("ItemsGridView") as GridView;
                    //igv.DataBind();
                }
            }
        }

        protected void QuotationGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            //CustomerID is stored as event Commend Argument
            
            if (e.CommandName == "View")
            {
                string Id = (string)e.CommandArgument;
                Response.Redirect("~/Quotation/Forms/ManageQuote.aspx?ID=" + Id);
            }
           
        }

        protected void ItemGridView_RowCommand(object s, GridViewCommandEventArgs e)
        {
            string Id = (string)e.CommandArgument;
            //CustomerID is stored as event Commend Argument
            if (e.CommandName == "Receipt")
            {
                Response.Redirect("~/Quotation/Forms/ManageItemTransaction.aspx?Receipt=" + Id);
            }
            else
            {
                Response.Redirect("~/Quotation/Forms/ManageItemTransaction.aspx?Dispatch=" + Id);
            }
        }

        protected void QuotationGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            QuotationGridView.PageIndex = e.NewPageIndex;
            QuotationGridView.DataBind();
        }
        protected void btnSelect_Click(object sender, EventArgs e)
        {

            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            QuotationGridView.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            QuotationGridView.SelectedIndex = row.RowIndex;

        }
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            GridViewRow row = QuotationGridView.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("rbtnSelect") != null) && ((RadioButton)row.FindControl("rbtnSelect")).Checked)
            {
                string ID = Convert.ToString(QuotationGridView.DataKeys[row.RowIndex].Value);
                Quotation.BusLogic.Quote quote = new Quotation.BusLogic.Quote();
                int[] ids=quote.DeleteQuote(ID);
                if (ids[0] >0)
                {
                    QuotationGridView.DataBind();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Quotation deleted successfully" + "');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + "Problem deleting Quotation" + "');", true);

                }
            }
        }
    }
}
