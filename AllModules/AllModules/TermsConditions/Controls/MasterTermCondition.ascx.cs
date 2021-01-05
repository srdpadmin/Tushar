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
using TermsConditions.BusLogic;
using TermsConditions.Data;

namespace TermsConditions.Controls
{
    public partial class MasterTermCondition : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (RadioList.SelectedValue == "Terms")
                {
                    TermsPanel.Visible = true;
                    ConditionsPanel.Visible = false;
                }
                else
                {
                    ConditionsPanel.Visible = true;
                    TermsPanel.Visible = false;
                }
            }
            else
            {
                TermsPanel.Visible = true;
                ConditionsPanel.Visible = false;
                PopulateGrids();
                ActionTermBtnVisible(false);
                ActionConditionBtnVisible(false);
                
            }
        }

        public void PopulateGrids()
        {
            MasterTermConditions mtc = new MasterTermConditions();
            DataSet ds = mtc.GetAllTerms(txtSearchTerm.Text);
            ViewState["Terms"] = ds;
            TermsGridView.DataSource = ds;
            TermsGridView.DataBind();
            ds = mtc.GetAllConditions(txtSearchCondition.Text);
            ViewState["Conditions"] = ds;
            ConditionsGridView.DataSource = ds;
            ConditionsGridView.DataBind();
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
        protected void ActionConditionBtnVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                imgSaveC.Visible = true;
                imgCancelC.Visible = true;
                imgAddC.Visible = false;
                imgEditC.Visible = false;
                imgDeleteC.Visible = false;
            }
            else
            {
                imgSaveC.Visible = false;
                imgCancelC.Visible = false;
                imgAddC.Visible = true;
                imgEditC.Visible = true;
                imgDeleteC.Visible = true;
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
            MasterTermConditions mtc = new MasterTermConditions();
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
                MasterTermConditions mtc = new MasterTermConditions();
                mtc.DeleteTermOrCondtion(ID, true);
                PopulateGrids();
            }
        }
        protected void imgUpdate_Click(object sender, EventArgs e)
        {
            ActionTermBtnVisible(false);
            GridViewRow row = TermsGridView.SelectedRow;
            TextBox tb = (TextBox)row.FindControl("TermName");
            string ID = Convert.ToString(TermsGridView.DataKeys[row.RowIndex].Value);
            MasterTermConditions mtc = new MasterTermConditions();
            MasterTerms mt = new MasterTerms();
            mt.ID = Convert.ToInt32(ID);
            mt.Term = tb.Text;
            mt.ModifiedOn = DateTime.Now;
            if (Convert.ToInt32(ID) < 0)
            {
                mtc.InsertTermOrCondition(mt, null);
            }
            else
            {
                mtc.UpdateTermOrCondition(mt, null);
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

        protected void SelectCondition_Click(object sender, EventArgs e)
        {
            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            ConditionsGridView.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            ConditionsGridView.SelectedIndex = row.RowIndex;
        }
        protected void imgAddC_Click(object sender, EventArgs e)
        {
            MasterTermConditions mtc = new MasterTermConditions();
            DataSet ds = (DataSet)ViewState["Conditions"];
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

                ViewState["Conditions"] = ds;
                ConditionsGridView.DataSource = ds;
                ConditionsGridView.DataBind();
            }

        }

        protected void imgEditC_Click(object sender, EventArgs e)
        {
            ActionConditionBtnVisible(true);
            GridViewRow row = ConditionsGridView.SelectedRow;
            ConditionsGridView.EditIndex = row.RowIndex;
            ConditionsGridView.DataSource = (DataSet)ViewState["Conditions"];
            ConditionsGridView.DataBind();
        }
        protected void imgDeleteC_Click(object sender, EventArgs e)
        {
            GridViewRow row = ConditionsGridView.SelectedRow;
            string ID = Convert.ToString(ConditionsGridView.DataKeys[row.RowIndex].Value);
            if (ID != string.Empty)
            {
                MasterTermConditions mtc = new MasterTermConditions();
                mtc.DeleteTermOrCondtion(ID, false);
                PopulateGrids();
            }
        }
        protected void imgUpdateC_Click(object sender, EventArgs e)
        {
            ActionConditionBtnVisible(false);
            GridViewRow row = ConditionsGridView.SelectedRow;
            TextBox tb = (TextBox)row.FindControl("ConditionName");
            string ID = Convert.ToString(ConditionsGridView.DataKeys[row.RowIndex].Value);
            MasterTermConditions mtc = new MasterTermConditions();
            MasterConditions mc = new MasterConditions();
            mc.ID = Convert.ToInt32(ID);
            mc.Condition = tb.Text;
            mc.ModifiedOn = DateTime.Now;
            if (Convert.ToInt32(ID) < 0)
            {
                mtc.InsertTermOrCondition(null, mc);
            }
            else
            {
                mtc.UpdateTermOrCondition( null,mc);
            }
            ConditionsGridView.EditIndex = -1;
            PopulateGrids();

        }
        protected void imgCancelC_Click(object sender, EventArgs e)
        {
            ConditionsGridView.EditIndex = -1;
            ConditionsGridView.DataSource = (DataSet)ViewState["Conditions"];
            ConditionsGridView.DataBind();
            ActionConditionBtnVisible(false);
        }

        protected void btnTermSearch_Click(object sender, EventArgs e)
        {
            PopulateGrids();
        }

        protected void btnConditionSearch_Click(object sender, EventArgs e)
        {
            PopulateGrids();
        }
    }
}