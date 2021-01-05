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
using Payroll.BusLogic;

namespace Payroll.Forms
{
    public partial class ManageProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // case of insert or edit
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {   //case of amend                    
                    ActionItemBtnVisible(false);                   
                                                  
                    PopulateItemsGrid();                   
                    ITUP.Update();
                }
                else
                {
                    //case of insert                   
                    ActionItemBtnVisible(false);      
                    PopulateItemsGrid();                   
                                     
                    ITUP.Update();

                }
            }
        }

        #region Handle Items
        public void PopulateItemsGrid()
        {
            string orderID = null;
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                orderID = Request.QueryString["ID"].ToString();
            }

            EmployeeProfile ET = new EmployeeProfile();
            DataSet ds = ET.GetAllItems();
            ViewState["Items"] = ds;
            ItemsGridView.DataSource = ds;
            ItemsGridView.DataBind();
            

        }
        protected void ActionItemBtnVisible(Boolean bMode)
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
        protected void imgAddItem_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)ViewState["Items"];
            int index = 0;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        index = Convert.ToInt32(dr["ID"]);
                    }
                }
                index += 1;
                DataRow drNewRow = ds.Tables[0].NewRow();
                drNewRow["ID"] = index;
                ds.Tables[0].Rows.Add(drNewRow);
                //ds.Tables[0].AcceptChanges();

                ViewState["Items"] = ds;
                ItemsGridView.DataSource = ds;
                ItemsGridView.DataBind();
              
                ITUP.Update();
            }

        }
        protected void imgEditItem_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            int index = -1;
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
               
                ITUP.Update();
            }
        }
        protected void imgDeleteItem_Click(object sender, EventArgs e)
        {
            CheckBox cb = null;
            ArrayList aList = new ArrayList();
            foreach (GridViewRow dr in ItemsGridView.Rows)
            {
                cb = (CheckBox)dr.FindControl("cbSelect");
                if (cb.Checked)
                {
                    aList.Add(dr.RowIndex); //(int)tpoGrid.DataKeys[].Value;                   

                }
            }
            if (aList.Count >= 0)
            {
                DataSet ds = (DataSet)ViewState["Items"];

                for (int i = aList.Count; i > 0; i--)
                {
                    ds.Tables[0].Rows[(int)aList[i - 1]].Delete();
                }
                ItemsGridView.DataSource = (DataSet)ViewState["Items"];
                ItemsGridView.DataBind();
               
                ITUP.Update();
            }
        }
        protected void imgUpdateItem_Click(object sender, EventArgs e)
        {

            if (ViewState["ItemKey"] != null)
            {
                int index = Convert.ToInt32(ViewState["ItemKey"]);
                Single totalValue = 0.0F;
                Single parseValue = 0.0F;
                int parseValueInt = 0;
                if ((ItemsGridView.Rows[index].RowState & DataControlRowState.Edit) > 0)
                {
                    Label id = (Label)ItemsGridView.Rows[index].FindControl("ID");
                    TextBox txtOrderID = (TextBox)ItemsGridView.Rows[index].FindControl("txtOrderID");
                    TextBox txtCode = (TextBox)ItemsGridView.Rows[index].FindControl("txtCode");
                    TextBox txtItemDescription = (TextBox)ItemsGridView.Rows[index].FindControl("txtItemDescription");
                    TextBox txtQuantity = (TextBox)ItemsGridView.Rows[index].FindControl("txtQuantity");
                    TextBox txtUnit = (TextBox)ItemsGridView.Rows[index].FindControl("txtUnit");
                    TextBox txtRate = (TextBox)ItemsGridView.Rows[index].FindControl("txtRate");
                    TextBox txtDiscount = (TextBox)ItemsGridView.Rows[index].FindControl("txtDiscount");
                    TextBox txtTax = (TextBox)ItemsGridView.Rows[index].FindControl("txtTax");
                    Label txtTaxAmount = (Label)ItemsGridView.Rows[index].FindControl("lblTaxAmount");
                    Label txtTotalAmount = (Label)ItemsGridView.Rows[index].FindControl("lblTotalAmount");

                    DataSet ds = (DataSet)ViewState["Items"];

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["ID"].ToString() == id.Text)
                        {
                            Int32.TryParse(txtOrderID.Text, out parseValueInt);
                            if (parseValueInt == 0 && !string.IsNullOrEmpty(Request.QueryString["ID"]))
                            {
                                parseValueInt = Convert.ToInt32(Request.QueryString["ID"].ToString());
                            }
                            dr["OrderID"] = parseValueInt;
                            dr["Code"] = txtCode.Text;
                            dr["Description"] = txtItemDescription.Text;
                            float.TryParse(txtQuantity.Text, out parseValue);
                            dr["Quantity"] = parseValue;
                            dr["Balance"] = parseValue;
                            totalValue = parseValue;
                            dr["Unit"] = txtUnit.Text;
                            float.TryParse(txtRate.Text, out parseValue);
                            dr["Rate"] = parseValue;
                            totalValue = totalValue * parseValue;
                            float.TryParse(txtDiscount.Text, out parseValue);
                            dr["Discount"] = parseValue;
                            if (totalValue > 0)
                                totalValue = totalValue - ((totalValue * parseValue) / 100);
                            float.TryParse(txtTax.Text, out parseValue);
                            dr["Tax"] = parseValue;
                            if (totalValue > 0)
                                totalValue = totalValue - ((totalValue * parseValue) / 100);
                            float.TryParse(txtTotalAmount.Text, out parseValue);
                            dr["TotalAmount"] = totalValue;

                            break;
                        }
                    }
                    ViewState["Items"] = ds;
                    // change the editindex and rebind
                    ItemsGridView.EditIndex = -1;
                    //ItemsBtnPanel.Visible = false;
                    //TermsBtnPanel.Visible = false;
                    ActionItemBtnVisible(false);
                    ItemsGridView.DataSource = (DataSet)ViewState["Items"];
                    ItemsGridView.DataBind();
                   
                    ITUP.Update();
                }
            }

        }
        protected void imgCancelItem_Click(object sender, EventArgs e)
        {
            ItemsGridView.EditIndex = -1;
            ActionItemBtnVisible(false);
            ItemsGridView.DataSource = (DataSet)ViewState["Items"];
            ItemsGridView.DataBind();
            ITUP.Update();
           
        }
        protected void ItemsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Edit)
            {
                DropDownList ddlEmpType = (DropDownList)e.Row.FindControl("ddlEmpType");
                EmployeeType ET = new EmployeeType();
                ddlEmpType.DataSource = ET.GetEmployeeType();
                ddlEmpType.DataTextField = "Value";
                ddlEmpType.DataValueField = "Key";
                ddlEmpType.DataBind();
               
            }
        }
        #endregion
    }
}
