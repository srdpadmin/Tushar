
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
using Authorization.BusLogic;
using ENM = CoreAssemblies.EnumClass;
using System.Collections.Generic;
using Authorization.Data;
using CoreAssemblies;

namespace Authorization
{
    public partial class UserModules : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.UrlReferrer != null && !Request.UrlReferrer.ToString().Contains("Authorization"))
            {
                string url = Request.UrlReferrer.ToString();
                if (url.Contains("Payroll"))
                {
                    this.Page.MasterPageFile = "~/Payroll/Payroll.master";
                    Session["MasterPageFile"] = "~/Payroll/Payroll.master";
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
            MembershipUser user = Membership.GetUser();

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Authorization/Login.aspx");
            }
            else if (!AllModules.Validate.UserRoleAccess(Convert.ToInt32(user.ProviderUserKey), EnumClass.Roles.Admin))
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Page.IsPostBack)
            {
                ActionItemBtnVisible(false);
                PopulateUserModules();
            }

        }
        protected void Add_Click(object sender, EventArgs e)
        {
            MembershipUser user = Membership.GetUser();

            DataSet ds = (DataSet)ViewState["Items"];

            bool canInsert = true;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["ID"]) == -1)
                        {
                            canInsert = false;
                        }
                    }
                }
                if (canInsert)
                {
                    DataRow drNewRow = ds.Tables[0].NewRow();
                    drNewRow["ID"] = -1;
                    ds.Tables[0].Rows.Add(drNewRow);
                    //ds.Tables[0].AcceptChanges();

                    ViewState["Items"] = ds;
                    umGrid.DataSource = ds;
                    umGrid.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", "javascript:alert('Only one new row can be added at a time')", true);
                }

            }

        }

        private void PopulateUserModules()
        {
            if (ViewState["Items"] == null)
            {
                UserModule um = new UserModule();
                DataSet ds = um.GetAllUserModules();
                ViewState["Items"] = ds;
                umGrid.DataSource = ds;
                umGrid.DataBind();
            }
            else
            {
                DataSet ds = ViewState["Items"] as DataSet;
                umGrid.DataSource = ds;
                umGrid.DataBind();
            }

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

        protected void Edit_Click(object sender, EventArgs e)
        {
            ActionItemBtnVisible(true);
            GridViewRow row = umGrid.SelectedRow;
            if (row != null)
            {
                umGrid.EditIndex = row.RowIndex;
                PopulateUserModules();
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            GridViewRow row = umGrid.SelectedRow;
            if (row != null)
            {
                HiddenField hdnUserID = row.FindControl("hdnUserID") as HiddenField;
                Label uName = row.FindControl("UserName") as Label;
                string ID = hdnUserID.Value;
                string userName = uName.Text;

                UserModule um = new UserModule();
                um.DeleteUserModules(ID, userName);
                ViewState["Items"] = null;
                PopulateUserModules();
                umGrid.SelectedIndex = -1;
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            ActionItemBtnVisible(false);
            umGrid.EditIndex = -1;
            PopulateUserModules();
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
        protected void SelectButton_Click(object sender, EventArgs e)
        {
            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            umGrid.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = false);
            umGrid.SelectedIndex = row.RowIndex;

        }
        protected void Update_Click(object sender, EventArgs e)
        {
            long mByte = 0;
            long rByte = 0;
            int index = 0;
            bool userExists = false;
            UsersData user = new UsersData();
            //bool insertRowFound = false;
            //int insertRowcount = 0;
            GridViewRow eRow = umGrid.SelectedRow;
            DropDownList ddluser = (DropDownList)eRow.FindControl("userddl");
            Saplin.Controls.DropDownCheckBoxes modCheckBoxes = eRow.FindControl("modCheckBoxes") as Saplin.Controls.DropDownCheckBoxes;
            Saplin.Controls.DropDownCheckBoxes rodCheckBoxes = eRow.FindControl("rodCheckBoxes") as Saplin.Controls.DropDownCheckBoxes;
            string ID = Convert.ToString(umGrid.DataKeys[eRow.RowIndex].Value);

            foreach (ListItem item in (modCheckBoxes as ListControl).Items)
            {
                if (item.Selected)
                {
                    mByte = mByte | Convert.ToInt64(item.Value);
                }
            }
            foreach (ListItem item in (rodCheckBoxes as ListControl).Items)
            {
                if (item.Selected)
                {
                    rByte = rByte | Convert.ToByte(item.Value);
                }
            }
            user.ModuleBit = mByte;
            user.RolesBit = rByte;
            
            //if (index <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", "javascript:alert('Please assign a module to user');", true);
            //}
            //else
            if (Convert.ToInt32(ID) == -1)
            {
                //Case of Insert
                UserModule um = new UserModule();
                userExists = um.CheckIfUserAlredyExisit(ddluser.SelectedValue);
                if (userExists)
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", "javascript:alert('The User already has assigned modules,Please select different user');", true);
                }
                else
                {

                    if (!userExists)
                    {
                        //Make a insert Call  
                        user.UserId = Convert.ToInt32(ddluser.SelectedValue);
                        CopyUIToObject(user, eRow);
                        um.InsertUserModules(user);
                        ActionItemBtnVisible(false);
                        umGrid.EditIndex = -1;
                        ViewState["Items"] = null;
                        PopulateUserModules();

                        AllModules.Validate.ClearRoleModuleAccess(Convert.ToInt32(ddluser.SelectedValue));

                    }
                }
            }
            else
            {
                //Case of Update
                UserModule um = new UserModule();
                user.UserId = Convert.ToInt32(ddluser.SelectedValue);
                user.ModuleBit = mByte;
                CopyUIToObject(user, eRow);
                if (um.UpdateUserModules(user) > 0)
                {
                    ActionItemBtnVisible(false);
                    umGrid.EditIndex = -1;
                    ViewState["Items"] = null;
                    PopulateUserModules();
                    AllModules.Validate.ClearRoleModuleAccess(Convert.ToInt32(ddluser.SelectedValue));
                }

            }
        }
        protected void ResetPassword_Click(object sender, EventArgs e)
        {

            GridViewRow row = umGrid.SelectedRow;
            if (row != null)
            {
                HiddenField hdnUserID = row.FindControl("hdnUserID") as HiddenField;
                Label uName = row.FindControl("UserName") as Label;
                string userName = uName.Text;
                MembershipProvider currentProvider = System.Web.Security.Membership.Provider;
                string newPassword = currentProvider.ResetPassword(userName, "Change");
                if (!string.IsNullOrEmpty(newPassword))
                {
                    if (currentProvider.ChangePassword(userName, newPassword, "password!"))
                    {
                        ErrorLabel.Text = "Password successfully changed. your new password is: password! , please change the password.";
                    }
                    else
                    {
                        ErrorLabel.Text = "Password cannot be changed.Please contact administrator";
                    }
                    ErrorLabel.Visible = true;
                }

            }

        }
        protected void Unlock_Click(object sender, EventArgs e)
        {

            GridViewRow row = umGrid.SelectedRow;
            if (row != null)
            {
                MembershipProvider currentProvider = System.Web.Security.Membership.Provider;
                HiddenField hdnUserID = row.FindControl("hdnUserID") as HiddenField;
                Label uName = row.FindControl("UserName") as Label;
                string userName = uName.Text;
                //if (amp.UnlockUser(userName))
                //{
                //    ErrorLabel.Text = "User successfully unlocked";
                //}
                //else
                //{
                //    ErrorLabel.Text = "User cannot be unlocked.Please contact administrator";
                //}
            }

        }
        private void CopyUIToObject(UsersData um, GridViewRow eRow)
        {
            TextBox txtFirstName = (TextBox)eRow.FindControl("txtFirstName");
            TextBox txtMiddleName = (TextBox)eRow.FindControl("txtMiddleName");
            TextBox txtLastName = (TextBox)eRow.FindControl("txtLastName");
            HiddenField hdnID = (HiddenField)eRow.FindControl("hdnID");
            um.FirstName = txtFirstName.Text;
            um.MiddleName = txtMiddleName.Text;
            um.LastName = txtLastName.Text;
            if (hdnID.Value != string.Empty)
            {
                um.ID = Convert.ToInt32(hdnID.Value);
            }

        }

        protected void umGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow eRow = e.Row as GridViewRow;
            long bByte = 0;

            if (eRow.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState & DataControlRowState.Edit) <= 0)
            {
                HiddenField hdnMBit = eRow.FindControl("hdnModuleBit") as HiddenField;
                HiddenField hdnRBit = eRow.FindControl("hdnRoleBit") as HiddenField;
                Label mName = eRow.FindControl("ModuleName") as Label;
                Label rName = eRow.FindControl("RoleName") as Label;
                if (hdnMBit.Value != string.Empty)
                {
                    bByte = Convert.ToInt64(hdnMBit.Value);
                    //Dictionary<int, string> dict = UserModule.ConvertModuleNamesToDictionary();
                    //int maxValue = Enum.GetValues(typeof(ENM.Modules)).Cast<int>().Max();
                    //if (maxValue == bByte)
                    //{
                    //    // All of the enumerations present
                    //    foreach (ENM.Modules item in Enum.GetValues(typeof(ENM.Modules)))
                    //    { 
                    //       mName.Text += item.ToString() +"," ;     
                    //    }
                    //    mName.Text = mName.Text.Remove(mName.Text.Length - 1, 1);
                    //}
                    //else
                    //{
                    //Loop through the dictionary to add item names
                    foreach (ENM.Modules item in Enum.GetValues(typeof(ENM.Modules)))
                    {
                        if (((long)item & bByte) == (long)item)
                        {
                            mName.Text += item.ToString() + ",";
                        }
                    }
                    if (mName.Text.Length > 0)
                    {
                        mName.Text = mName.Text.Remove(mName.Text.Length - 1, 1);
                    }
                    //}                  
                }
                if (hdnRBit.Value != string.Empty)
                {

                    bByte = Convert.ToInt64(hdnRBit.Value);
                    //Dictionary<int, string> dict = UserModule.ConvertModuleNamesToDictionary();
                    //int maxValue = Enum.GetValues(typeof(ENM.Roles)).Cast<int>().Max();
                    //if (maxValue == bByte)
                    //{
                    //    // All of the enumerations present
                    //    foreach (ENM.Roles item in Enum.GetValues(typeof(ENM.Roles)))
                    //    {
                    //        rName.Text += item.ToString() + ",";
                    //    }
                    //    rName.Text = rName.Text.Remove(rName.Text.Length - 1, 1);
                    //}
                    //else
                    //{
                    //Loop through the dictionary to add item names
                    foreach (ENM.Roles item in Enum.GetValues(typeof(ENM.Roles)))
                    {
                        if (((long)item & bByte) == (long)item)
                        {
                            rName.Text += item.ToString() + ",";
                        }
                    }
                    if (rName.Text.Length > 0)
                    {
                        rName.Text = rName.Text.Remove(rName.Text.Length - 1, 1);
                    }
                    //}
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddluser = (DropDownList)e.Row.FindControl("userddl");
                HiddenField hdnUserID = (HiddenField)e.Row.FindControl("hdnUserID");
                UserModule um = new UserModule();
                ddluser.DataSource = um.GetUsers();
                ddluser.DataTextField = "Value";
                ddluser.DataValueField = "Key";
                if (hdnUserID.Value != string.Empty)
                {
                    ddluser.SelectedValue = hdnUserID.Value;
                    ddluser.Enabled = false;
                }
                ddluser.DataBind();

                Saplin.Controls.DropDownCheckBoxes modCheckBoxes = eRow.FindControl("modCheckBoxes") as Saplin.Controls.DropDownCheckBoxes;
                modCheckBoxes.AddJQueryReference = true;
                modCheckBoxes.DataSource = ConvertModulesEnumToDictionary();
                modCheckBoxes.DataTextField = "Key";
                modCheckBoxes.DataValueField = "Value";
                modCheckBoxes.DataBind();
                HiddenField hdnMBit = eRow.FindControl("hdnModuleBit") as HiddenField;
                if (hdnMBit.Value != string.Empty)
                {
                    bByte = Convert.ToInt64(hdnMBit.Value);
                    if (bByte > 0)
                    {
                        foreach (ListItem item in (modCheckBoxes as ListControl).Items)
                        {
                            if ((Convert.ToInt64(item.Value) & bByte) == Convert.ToInt64(item.Value))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }


                Saplin.Controls.DropDownCheckBoxes rodCheckBoxes = eRow.FindControl("rodCheckBoxes") as Saplin.Controls.DropDownCheckBoxes;
                rodCheckBoxes.AddJQueryReference = true;
                rodCheckBoxes.DataSource = ConvertRolesEnumToDictionary();
                rodCheckBoxes.DataTextField = "Key";
                rodCheckBoxes.DataValueField = "Value";
                rodCheckBoxes.DataBind();
                HiddenField hdnRBit = eRow.FindControl("hdnRoleBit") as HiddenField;
                if (hdnRBit.Value != string.Empty)
                {
                    bByte = Convert.ToInt64(hdnRBit.Value);
                    if (bByte > 0)
                    {

                        foreach (ListItem item in (rodCheckBoxes as ListControl).Items)
                        {
                            if ((Convert.ToInt64(item.Value) & bByte) == Convert.ToInt64(item.Value))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }
            }


        }

        protected void mod_checkBoxes_SelcetedIndexChanged(object sender, EventArgs e)
        {
            Saplin.Controls.DropDownCheckBoxes modCheckBoxes = sender as Saplin.Controls.DropDownCheckBoxes;
            modCheckBoxes.Texts.SelectBoxCaption = string.Empty;
            foreach (ListItem item in (modCheckBoxes as ListControl).Items)
            {
                if (item.Selected)
                    modCheckBoxes.Texts.SelectBoxCaption += item.Text + ",";
            }
        }
        protected void rod_checkBoxes_SelcetedIndexChanged(object sender, EventArgs e)
        {
            Saplin.Controls.DropDownCheckBoxes rodCheckBoxes = sender as Saplin.Controls.DropDownCheckBoxes;
            rodCheckBoxes.Texts.SelectBoxCaption = string.Empty;
            foreach (ListItem item in (rodCheckBoxes as ListControl).Items)
            {
                if (item.Selected)
                    rodCheckBoxes.Texts.SelectBoxCaption += item.Text + ",";
            }
        }

        public static IDictionary<String, Int32> ConvertModulesEnumToDictionary()
        {
            if (typeof(ENM.Modules).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            return Enum.GetValues(typeof(ENM.Modules)).Cast<int>().ToDictionary(currentItem => Enum.GetName(typeof(ENM.Modules), currentItem));
        }
        public static IDictionary<String, Int32> ConvertRolesEnumToDictionary()
        {
            if (typeof(ENM.Roles).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            return Enum.GetValues(typeof(ENM.Roles)).Cast<int>().ToDictionary(currentItem => Enum.GetName(typeof(ENM.Roles), currentItem));
        }

    }
}

