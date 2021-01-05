using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Inventory.BusLogic;
using System.Data;
using Inventory.Data;

namespace AllModules.Inventory.Controls
{
    public partial class LocationControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                 
                TermsPanel.Visible = true;                
                PopulateGrids();
                ActionTermBtnVisible(false); 
            }
        }

        public void PopulateGrids()
        {
            Location mtc = new Location();
            DataSet ds = mtc.GetAllLocations(txtSearchTerm.Text);
            ViewState["Terms"] = ds;
            TermsGridView.DataSource = ds;
            TermsGridView.DataBind();           
        }
        protected void ActionTermBtnVisible(Boolean bMode)
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

        protected void SelectTerm_Click(object sender, EventArgs e)
        {
            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            TermsGridView.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            TermsGridView.SelectedIndex = row.RowIndex;
        }

        protected void imgAdd_Click(object sender, EventArgs e)
        {
            Location mtc = new Location();
            DataSet ds = (DataSet)ViewState["Terms"];
            int index = 0;
            int lastIndex = 0;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        index = Convert.ToInt32(dr["ID"]);
                        if (index < 0 && index < lastIndex)
                        {
                            lastIndex = index;
                        }
                    }
                }
                if (lastIndex == 0)
                {
                    lastIndex = -1;
                }
                DataRow drNewRow = ds.Tables[0].NewRow();
                drNewRow["ID"] = lastIndex;
                ds.Tables[0].Rows.Add(drNewRow);

                ViewState["Terms"] = ds;
                TermsGridView.DataSource = ds;
                TermsGridView.DataBind();
            }

        }

        protected void imgEdit_Click(object sender, EventArgs e)
        {
            ActionTermBtnVisible(true);
            GridViewRow row = TermsGridView.SelectedRow;
            TermsGridView.EditIndex = row.RowIndex;
            TermsGridView.DataSource = (DataSet)ViewState["Terms"];
            TermsGridView.DataBind();
        }
        protected void imgDelete_Click(object sender, EventArgs e)
        {
            GridViewRow row = TermsGridView.SelectedRow;
            string ID = Convert.ToString(TermsGridView.DataKeys[row.RowIndex].Value);
            if (ID != string.Empty)
            {
                Location mtc = new Location();
                mtc.DeleteLocation(ID);
                PopulateGrids();
            }
        }
        protected void imgUpdate_Click(object sender, EventArgs e)
        {
            ActionTermBtnVisible(false);
            GridViewRow row = TermsGridView.SelectedRow;
            TextBox tb = (TextBox)row.FindControl("TermName");
            string ID = Convert.ToString(TermsGridView.DataKeys[row.RowIndex].Value);
            Location mtc = new Location();
            LocationData mt = new LocationData();
            mt.ID = Convert.ToInt32(ID);
            mt.LocationName = tb.Text;
            mt.ModifiedOn = DateTime.Now;
            if (Convert.ToInt32(ID) < 0)
            {
                mtc.InsertLocation(mt);
            }
            else
            {
                mtc.UpdateLocation(mt);
            }
            TermsGridView.EditIndex = -1;
            PopulateGrids();

        }
        protected void imgCancel_Click(object sender, EventArgs e)
        {
            TermsGridView.EditIndex = -1;
            TermsGridView.DataSource = (DataSet)ViewState["Terms"];
            TermsGridView.DataBind();
            ActionTermBtnVisible(false);
        }
        protected void btnTermSearch_Click(object sender, EventArgs e)
        {
            PopulateGrids();
        }

    }
}