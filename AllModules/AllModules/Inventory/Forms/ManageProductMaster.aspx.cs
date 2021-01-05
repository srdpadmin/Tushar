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
using Inventory.BusLogic;
using System.Collections.Generic;
using Inventory.Data;

namespace Billing.Forms
{
    public partial class ManageProductMaster : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.UrlReferrer != null)
            {
                string url = Request.UrlReferrer.ToString();
                if (url.Contains("Billing"))
                {
                    this.Page.MasterPageFile = "~/Billing/Billing.master";
                    Session["MasterPageFile"] = "~/Billing/Billing.master";
                }

                else if (url.Contains("Inventory"))
                {
                    this.Page.MasterPageFile = "~/Inventory/Inventory.master";
                    Session["MasterPageFile"] = "~/Inventory/Inventory.master";
                }
                else if (url.Contains("Quotation"))
                {
                    this.Page.MasterPageFile = "~/Quotation/Quotation.master";
                    Session["MasterPageFile"] = "~/Quotation/Quotation.master";
                }
            }
            else
            {
                if (Session["MasterPageFile"] != null)
                {
                    this.Page.MasterPageFile = Session["MasterPageFile"].ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!Page.IsPostBack)
            {
                PopulateProducts();
                ActionButtonVisible(false);
            }
            else
            {
                ErrorLabel.Visible = false;
            }
        }

        public void PopulateProducts()
        {
            ProductMaster PM = new ProductMaster();
            ProdMasterGrid.DataSource = PM.GetAllProducts();
            ProdMasterGrid.DataBind();
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            MembershipUser user = Membership.GetUser();
            ProductMaster PM = new ProductMaster();
          
            Dictionary<string, string> dict = PM.GetProductMasterWithId();

            if (!dict.ContainsValue("TestCode"))
            {               
                int i = PM.InsertProductMaster(Convert.ToInt32(user.ProviderUserKey));
                if (i < 0)
                {
                    ErrorLabel.Text = "Error creating new ProductMaster. Please try again later.";
                    ErrorLabel.Visible = true;
                }
                else
                {
                    ErrorLabel.Visible = false;
                    PopulateProducts();
                }
            }
            else
            {
                ErrorLabel.Text = "Cannot add duplicate code to ProductMaster.";
                ErrorLabel.Visible = true;
            }

        }

        protected void Edit_Click(object sender, EventArgs e)
        {
             
            int index = -1;
            index = GetCheckedElement();
            if (index >=0)
            {
                ProductMaster PM = new ProductMaster();
                ProdMasterGrid.EditIndex = index;
                ActionButtonVisible(true);
                PopulateProducts();
                //((CheckBox)ProdMasterGrid.Rows[index].FindControl("select")).Checked = true;
            }

        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            GetCheckedElement();
            if (ViewState["ItemKey"] != null)
            {
                int index = Convert.ToInt32(ViewState["ItemKey"]);
                int Id=Convert.ToInt32(Convert.ToString(ProdMasterGrid.DataKeys[index].Value));
                ProductMaster PM = new ProductMaster();
                PM.DeleteProductMaster(Id);
                PopulateProducts();
                ViewState["ItemKey"] = null;
            }
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            float parseValue;
            if (ViewState["ItemKey"] != null)
            {
                    int index = Convert.ToInt32(ViewState["ItemKey"]);
                    int Id = Convert.ToInt32(Convert.ToString(ProdMasterGrid.DataKeys[index].Value));
                    ProductMaster PM = new ProductMaster();
                    ProductMasterData pmd = new ProductMasterData();
                    TextBox tb = (TextBox)ProdMasterGrid.Rows[index].FindControl("Code");
                    TextBox tb1 = (TextBox)ProdMasterGrid.Rows[index].FindControl("Description");
                    //TextBox tb2 = (TextBox)ProdMasterGrid.Rows[index].FindControl("Balance");
                    TextBox tb3 = (TextBox)ProdMasterGrid.Rows[index].FindControl("Type");
                    TextBox tb4 = (TextBox)ProdMasterGrid.Rows[index].FindControl("Unit");
                    TextBox tb5 = (TextBox)ProdMasterGrid.Rows[index].FindControl("Rate");
                    TextBox tb6 = (TextBox)ProdMasterGrid.Rows[index].FindControl("Location");
                    TextBox tb7 = (TextBox)ProdMasterGrid.Rows[index].FindControl("Tax");
                    TextBox tb8 = (TextBox)ProdMasterGrid.Rows[index].FindControl("Discount");
                    HiddenField tb9 = (HiddenField)ProdMasterGrid.Rows[index].FindControl("hdnCreatedBy");
                    MembershipUser user = Membership.GetUser();
                    pmd.ID = Id;
                    pmd.Code = tb.Text;
                    pmd.Description = tb1.Text;
                    //float.TryParse(tb2.Text, out parseValue);
                    //pmd.Balance = parseValue;
                   
                    float.TryParse(tb5.Text, out parseValue);
                    pmd.Rate = parseValue;

                    float.TryParse(tb7.Text, out parseValue);
                    pmd.Tax = parseValue;

                    float.TryParse(tb8.Text, out parseValue);
                    pmd.Discount = parseValue;

                    pmd.Type = tb3.Text;
                    pmd.Unit = tb4.Text;                    
                    //pmd.Location = tb6.Text;
                    pmd.ModifiedOn = DateTime.Now;
                    pmd.CreatedBy = Convert.ToInt32(tb9.Value);


                    //todo: tryparse above
                    if (ValidateProduct(pmd))
                    {
                        int update = PM.UpdateProductMaster(pmd);
                        //int update = PM.UpdateProductMaster(Id, tb.Text,tb1.Text, Convert.ToInt32 (tb2.Text), tb3.Text, Convert.ToInt32 (tb4.Text), Convert.ToInt32 (tb5.Text), tb6.Text, "");
                        if (update > 0)
                        {
                            ProdMasterGrid.EditIndex = -1;
                            ActionButtonVisible(false);
                            PopulateProducts();
                        }
                        else
                        {                            
                            ErrorLabel.Text = "Error Updating the ProductMaster";
                            ErrorLabel.Visible = true;
                        }
                    }
                    else
                    {
                        ErrorLabel.Text = "Cannot add duplicate Code to ProductMaster.Please use different Code.";
                        ErrorLabel.Visible = true;
                    }
                     
                    
              
            }

        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            ProdMasterGrid.EditIndex = -1;
            ActionButtonVisible(false);
            PopulateProducts();
        }

        protected void ActionButtonVisible(Boolean bMode)
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

        public int GetCheckedElement()
        {
            RadioButton cb = null;
            int Id=-1;
            foreach (GridViewRow gvr in ProdMasterGrid.Rows)
            {
                cb = (RadioButton)gvr.FindControl("cbSelect");
             
                if (cb != null && cb.Checked)
                {
                    Id = gvr.RowIndex;
                    ViewState["ItemKey"] = gvr.RowIndex;
                    break;
                }                
            }
            return Id;
        }

        public bool ValidateProduct(ProductMasterData pmd)
        {
            bool returnValue = true;
            MembershipUser user = Membership.GetUser();
            ProductMaster PM = new ProductMaster();
            Dictionary<string, string> dict = PM.GetProductMasterWithId();

            //case to check if code = testcode is found twice
            if (pmd.Code.Trim() != string.Empty && dict.ContainsValue(pmd.Code.Trim())) // case of duplicate code
            {   
                //check  if Id is same
                foreach (KeyValuePair<string, string> pair in dict)
                {	                   
                   if(Convert.ToInt32(pair.Key) != pmd.ID && pair.Value == pmd.Code)
                   {
                     returnValue = false;
                     break;
                   }
                }               
            }
            return returnValue;
        }

        protected void ProdMasterGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ProdMasterGrid.PageIndex = e.NewPageIndex;
            PopulateProducts();
        }
    }
}
