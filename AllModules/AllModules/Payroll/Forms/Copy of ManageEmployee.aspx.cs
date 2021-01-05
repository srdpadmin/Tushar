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
using Payroll.Data;
using ASM = CoreAssemblies;
using WC = Payroll.Common.WebCommon;
using System.Globalization;
using System.Text;

namespace Payroll.Forms
{
    public partial class ManageEmployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO: Add AmendedBy,CreatedBy,RowVersion & Active
            if (!Page.IsPostBack)
            {
                // case of insert or edit
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {   //case of amend
                    ToggleAmendInsertVisible(false);
                    EmployeeView.DataBind();
                    EmployeeDetailsView.DataBind();
                    SalaryDetailsView.DataBind();
                    MUP.Update();
                    DUP.Update();
                }

                else
                {
                    //case of insert
                    ToggleAmendInsertVisible(true);
                    EmployeeView.ChangeMode(DetailsViewMode.Insert);
                    EmployeeDetailsView.ChangeMode(DetailsViewMode.Insert);
                    SalaryDetailsView.ChangeMode(DetailsViewMode.Insert);
                    EmployeeView.DataBind();
                    EmployeeDetailsView.DataBind();
                    SalaryDetailsView.DataBind();
                    MUP.Update();
                    DUP.Update();

                }
            }
        }

        protected void ToggleAmendInsertVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Visible = false;
                btnPrint.Visible = false;
                btnUpdate.Visible = false;
                btnInsert.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                btnPrint.Visible = true;
                btnAmend.Visible = true;
                btnInsert.Visible = false;
                btnCancel.Visible = true;
                btnUpdate.Visible = false;
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    btnPrint.Attributes.Add("onclick", "PrintReportInNewWindow(" + Request.QueryString["ID"] + "); return false;");
                }
            }

        }

        protected void ToggleAmendUpdateVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Visible = true;
                btnUpdate.Visible = false;

            }
            else
            {
                btnAmend.Visible = false;
                btnUpdate.Visible = true;
            }

        }

        protected void btnAmend_Click(object sender, EventArgs e)
        {
            ToggleAmendUpdateVisible(false);
            EmployeeView.ChangeMode(DetailsViewMode.Edit);
            EmployeeDetailsView.ChangeMode(DetailsViewMode.Edit);
            SalaryDetailsView.ChangeMode(DetailsViewMode.Edit);
            MUP.Update();
            DUP.Update();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (EmployeeView.CurrentMode == DetailsViewMode.Edit)
            {
                ToggleAmendUpdateVisible(true);
            }
            else if (EmployeeView.CurrentMode == DetailsViewMode.Insert)
            {
                Response.Redirect("~/Payroll/Forms/SearchEmployees.aspx");
            }
            else if (EmployeeView.CurrentMode == DetailsViewMode.ReadOnly)
            {
                Response.Redirect("~/Payroll/Forms/SearchEmployees.aspx");
            }

            EmployeeView.DataBind();
            EmployeeView.ChangeMode(DetailsViewMode.ReadOnly);
            EmployeeDetailsView.ChangeMode(DetailsViewMode.ReadOnly);
            SalaryDetailsView.ChangeMode(DetailsViewMode.ReadOnly);
            MUP.Update();
            DUP.Update();
        }
        
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            EmployeeData        oData  = CopyUIToEmployeeData();
            EmployeeDetailsData oeData = CopyUIToEmployeeDetailsData();
            SalaryDetailsData osData = CopyUIToSalaryDetailsData();
            DataSet dsItems = (DataSet)ViewState["EmpDetails"];
                //DataSet dsTerms = (DataSet)ViewState["Terms"];
                ArrayList sqlCommands = new ArrayList();
                string updateSql = ASM.Core.SetClassPropertiesValuesToSql(oData, "UPDATE", "Employee");
                sqlCommands.Add(updateSql);
                sqlCommands.Add(ASM.Core.SetClassPropertiesValuesToSqlUpdated(oeData, "UPDATE", "EmployeeDetails", null, null));
                sqlCommands.Add(ASM.Core.SetClassPropertiesValuesToSqlUpdated(osData, "UPDATE", "SalaryDetails", null, null));  
                int[] x = WC.UpdateEmployeeToDbUsingTransaction(sqlCommands);

                if (x[0] < 0)
                {
                    //Error

                }
                else
                {
                    ToggleAmendUpdateVisible(true);
                    EmployeeView.ChangeMode(DetailsViewMode.ReadOnly);
                    EmployeeDetailsView.ChangeMode(DetailsViewMode.ReadOnly);
                    SalaryDetailsView.ChangeMode(DetailsViewMode.ReadOnly);
                    DUP.Update();
                    MUP.Update();

                }

        }
        
        protected void btnInsert_Click(object sender, EventArgs e)
        {

                EmployeeData oData = CopyUIToEmployeeData();
                EmployeeDetailsData oeData = CopyUIToEmployeeDetailsData();
                SalaryDetailsData osData = CopyUIToSalaryDetailsData();
                //todo set createdby for oData to userid
            
                DataSet dsItems = (DataSet)ViewState["EmpDetails"];
                //DataSet dsTerms = (DataSet)ViewState["Terms"];
                string insertSql =ASM.Core.SetClassPropertiesValuesToSql(oData, "INSERT", "Employee");
                string identitySql = "Select @@Identity";
                ArrayList sqlCommands = new ArrayList();
                sqlCommands.Add(insertSql);
                sqlCommands.Add(identitySql);
                ArrayList arx = new ArrayList();
                arx.Add("ID");
                arx.Add("EmpID");
                string ar = ASM.Core.SetClassPropertiesValuesToSqlUpdated(oeData, "INSERT", "EmployeeDetails",arx,"EmpID");
                sqlCommands.Add(ar);
                ar = ASM.Core.SetClassPropertiesValuesToSqlUpdated(osData, "INSERT", "SalaryDetails", arx, "EmpID");
                sqlCommands.Add(ar);
                int x = WC.InsertEmployeeToDbUsingTransaction(sqlCommands);

                if (x < 0)
                {
                    //Error
                }
                else
                {
                    Response.Redirect("~/Payroll/Forms/ManageEmployee.aspx?ID=" + x.ToString());
                    EmployeeView.DataBind();
                }
 
        }

        public EmployeeData CopyUIToEmployeeData()
        {

            TextBox txtDesig = EmployeeView.Rows[0].FindControl("txtDesig") as TextBox;            
            TextBox txtJoinDate = EmployeeView.Rows[0].FindControl("txtJoinDate") as TextBox;
            Label txtCreatedOn = EmployeeView.Rows[0].FindControl("txtCreatedOn") as Label;
            Label txtModifiedOn = EmployeeView.Rows[0].FindControl("txtModifiedOn") as Label;
            CheckBox isActive = EmployeeView.Rows[0].FindControl("IsActive") as CheckBox;
            EmployeeData oData = new EmployeeData();
            //Assign data
            oData.Designation = txtDesig.Text;
            CultureInfo cultEnGb = new CultureInfo("en-GB");
            oData.Active = isActive.Checked;
            oData.ModifiedOn = DateTime.Now;
           
            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                Label id = EmployeeView.Rows[0].FindControl("lblID") as Label;
                oData.ID = Convert.ToInt32(id.Text);                               
                
                oData.JoiningDate= ASM.Core.ResolveDateToUS(txtJoinDate.Text);
                IFormatProvider format = new System.Globalization.CultureInfo("en-GB", true);
                
                oData.CreatedOn = ASM.Core.ResolveDateToUS(txtCreatedOn.Text);                              
                //oData.AmendedBy = 1; TODO            
            }
            else
            {
                oData.CreatedOn = DateTime.Now;
                //oData.CreatedBy = 1;  //todo: user should go here             
                oData.JoiningDate = DateTime.Now;
              
                 
            }
            return oData;

        }

        public EmployeeDetailsData CopyUIToEmployeeDetailsData()
        {
            Label lblID = EmployeeDetailsView.Rows[0].FindControl("lblID") as Label;
            TextBox txtfname = EmployeeDetailsView.Rows[0].FindControl("txtfname") as TextBox;
            TextBox txtmname = EmployeeDetailsView.Rows[0].FindControl("txtmname") as TextBox;
            TextBox txtlname = EmployeeDetailsView.Rows[0].FindControl("txtlname") as TextBox;
            TextBox txtdob = EmployeeDetailsView.Rows[0].FindControl("txtdob") as TextBox;
            RadioButtonList rblGender = EmployeeDetailsView.Rows[0].FindControl("rblGender") as RadioButtonList;
            CheckBox cbMarried = EmployeeDetailsView.Rows[0].FindControl("IsMarried") as CheckBox;
            EmployeeDetailsData oData = new EmployeeDetailsData();
            //Assign data
            
            oData.FirstName = txtfname.Text;
            oData.MiddleName = txtmname.Text;
            oData.LastName = txtlname.Text;
            oData.ModifiedOn = DateTime.Now;
            oData.Gender = Convert.ToBoolean(rblGender.SelectedValue);
            oData.Married = cbMarried.Checked;
            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {                 
                oData.EmpID = Convert.ToInt32(Request.QueryString["ID"]);
                oData.ID =  Convert.ToInt32(lblID.Text);
                //oData.CreatedOn = ASM.Core.ResolveDate(txtCreatedOn.Text);
                //oData.AmendedBy = 1; TODO            
            }
            else
            {
                oData.CreatedOn = DateTime.Now;
                //oData.CreatedBy = 1;  //TODO: user should go here               
            }
            return oData;
        }

        public SalaryDetailsData CopyUIToSalaryDetailsData()
        {
            CheckBox cbBD       = SalaryDetailsView.Rows[0].FindControl("cbIsBasicDaily") as CheckBox;
            CheckBox cbAD       = SalaryDetailsView.Rows[0].FindControl("cbIsAllowanceDaily") as CheckBox;
            Label   lblID       = SalaryDetailsView.Rows[0].FindControl("lblID") as Label;
            DropDownList ddlOTR = SalaryDetailsView.Rows[0].FindControl("ddlOverTimeRate") as DropDownList;
            DropDownList ddlEmpType = SalaryDetailsView.Rows[0].FindControl("ddlEmpType") as DropDownList;
            TextBox txtBasicMonthly = SalaryDetailsView.Rows[0].FindControl("txtBasicMonthly") as TextBox;
            TextBox txtAllowanceMonthly = SalaryDetailsView.Rows[0].FindControl("txtAllowanceMonthly") as TextBox;
            TextBox txtBasicDaily = SalaryDetailsView.Rows[0].FindControl("txtBasicDaily") as TextBox;
            TextBox txtAllowanceDaily = SalaryDetailsView.Rows[0].FindControl("txtAllowanceDaily") as TextBox;
            TextBox txtBalanceLeaves = SalaryDetailsView.Rows[0].FindControl("txtBalanceLeaves") as TextBox;
            //TextBox txtAdvancePending = SalaryDetailsView.Rows[0].FindControl("txtAdvancePending") as TextBox;
            TextBox txtYearlyPaidLeaves = SalaryDetailsView.Rows[0].FindControl("txtYearlyPaidLeaves") as TextBox;

            //New fields
            Label lblAdvancePending1 = SalaryDetailsView.Rows[0].FindControl("lblAdvancePending1") as Label;
            Label lblProfessionalTaxPercent1 = SalaryDetailsView.Rows[0].FindControl("lblProfessionalTaxPercent1") as Label;
            Label lblESICTaxPercent1 = SalaryDetailsView.Rows[0].FindControl("lblESICTaxPercent1") as Label;
            Label lblProvidentFundPercent1 = SalaryDetailsView.Rows[0].FindControl("lblProvidentFundPercent1") as Label;
            //New fields
            //CheckBox cbHasAllowance = SalaryDetailsView.Rows[0].FindControl("cbHasAllowance") as CheckBox;
            //CheckBox cbHasLeaves = SalaryDetailsView.Rows[0].FindControl("cbHasLeaves") as CheckBox;
            //CheckBox cbHasOverTime = SalaryDetailsView.Rows[0].FindControl("cbHasOverTime") as CheckBox;
            CheckBox cbDeductESIC = SalaryDetailsView.Rows[0].FindControl("cbDeductESIC") as CheckBox;
            CheckBox cbDeductProfTax = SalaryDetailsView.Rows[0].FindControl("cbDeductProfTax") as CheckBox;
            CheckBox cbDeductPF = SalaryDetailsView.Rows[0].FindControl("cbDeductPF") as CheckBox;

            SalaryDetailsData oData = new SalaryDetailsData();
            //Assign data
            Single parseValue=0.0F;
            oData.EmpType = Convert.ToInt32(ddlEmpType.SelectedItem.Value);
            oData.IsAllowanceDaily = cbAD.Checked;
            oData.IsBasicDaily = cbBD.Checked;
            oData.OverTimeRate = Convert.ToInt32(ddlOTR.SelectedItem.Value);
            oData.ModifiedOn = DateTime.Now;

             float.TryParse(txtBasicMonthly.Text, out parseValue);
             oData.BasicMonthly = parseValue;

             float.TryParse(txtBasicDaily.Text, out parseValue);
             oData.BasicDaily = parseValue;

             float.TryParse(txtAllowanceDaily.Text, out parseValue);
             oData.AllowanceDaily = parseValue;

             float.TryParse(txtAllowanceMonthly.Text, out parseValue);
             oData.AllowanceMonthly = parseValue;

             float.TryParse(txtBalanceLeaves.Text, out parseValue);
             oData.BalanceLeaves = parseValue;             

             float.TryParse(txtYearlyPaidLeaves.Text, out parseValue);
             oData.YearlyPaidLeaves = parseValue;

             //New fields
             float.TryParse(lblAdvancePending1.Text, out parseValue);
             oData.AdvancePending = parseValue;

             float.TryParse(lblProfessionalTaxPercent1.Text, out parseValue);
             oData.ProfessionalTaxPercent = parseValue;

             float.TryParse(lblESICTaxPercent1.Text, out parseValue);
             oData.ESICTaxPercent = parseValue;

             float.TryParse(lblProvidentFundPercent1.Text, out parseValue);
             oData.ProvidentFundPercent = parseValue;

             //New fields           
             //oData.HasAllowance = cbHasAllowance.Checked;
             //oData.HasLeaves = cbHasLeaves.Checked;
             //oData.HasOverTime = cbHasOverTime.Checked;
             oData.DeductESIC = cbDeductESIC.Checked;
             oData.DeductProfTax = cbDeductProfTax.Checked;
             oData.DeductPF = cbDeductPF.Checked;


            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                oData.EmpID = Convert.ToInt32(Request.QueryString["ID"]);
                oData.ID = Convert.ToInt32(lblID.Text);
                //oData.CreatedOn = ASM.Core.ResolveDate(txtCreatedOn.Text);
                //oData.AmendedBy = 1; TODO            
            }
            else
            {
                oData.CreatedOn = DateTime.Now;
                //oData.CreatedBy = 1;  //TODO: user should go here               
            }
            return oData;
        }

        protected void EmployeeView_DataBound(object sender, EventArgs e)
        {
            if (EmployeeView.CurrentMode == DetailsViewMode.Insert ||
                EmployeeView.CurrentMode == DetailsViewMode.Edit)
            {
                DetailsView view = (DetailsView)sender;
                DetailsViewRowCollection rows = view.Rows;
                DetailsViewRow row = rows[0];
                 
                if (EmployeeView.CurrentMode == DetailsViewMode.Edit)
                {
                    Label hdnActive = row.FindControl("hdnActive") as Label;                    
                    CheckBox cb = row.FindControl("IsActive") as CheckBox;

                    if (hdnActive.Text.ToLower() == "true")
                        cb.Checked = true;
                    else
                        cb.Checked = false;
                }

            }
            else if (EmployeeDetailsView.CurrentMode == DetailsViewMode.ReadOnly)
            {
                DetailsView view = (DetailsView)sender;
                DetailsViewRowCollection rows = view.Rows;
                DetailsViewRow row = rows[0];

                Label hdnActive = row.FindControl("hdnActive") as Label;
                CheckBox cb = row.FindControl("IsActive") as CheckBox;

                if (hdnActive.Text.ToLower() == "true")
                    cb.Checked = true;
                else
                    cb.Checked = false;

            }
        }

        protected void EmployeeDetailsView_DataBound(object sender, EventArgs e)
        {
            if (EmployeeDetailsView.CurrentMode == DetailsViewMode.Insert ||
                EmployeeDetailsView.CurrentMode == DetailsViewMode.Edit)
            {
                DetailsView view = (DetailsView)sender;
                DetailsViewRowCollection rows = view.Rows;
                DetailsViewRow row = rows[0];
                RadioButtonList ddlEmpType = (RadioButtonList)row.FindControl("rblGender");
                if (EmployeeView.CurrentMode == DetailsViewMode.Edit)
                {
                    Label hdnGender = row.FindControl("hdnGender") as Label;
                    Label hdnMarried = row.FindControl("hdnMarried") as Label;
                    CheckBox cb = row.FindControl("IsMarried") as CheckBox;
                    ddlEmpType.SelectedValue = Convert.ToBoolean(hdnGender.Text) ? "Male" : "Female";
                    //using ismale
                    if (hdnGender.Text.ToLower() == "true")
                        ddlEmpType.Items[0].Selected = true;

                    else
                        ddlEmpType.Items[1].Selected = true;

                    if (hdnMarried.Text.ToLower() == "true")
                        cb.Checked = true;
                    else
                        cb.Checked = false;
                }

            }
            else if(EmployeeDetailsView.CurrentMode == DetailsViewMode.ReadOnly)
            {
                DetailsView view = (DetailsView)sender;
                DetailsViewRowCollection rows = view.Rows;
                DetailsViewRow row = rows[0];
                Label hdnGender = row.FindControl("hdnGender") as Label;
                Label hdnMarried = row.FindControl("hdnMarried") as Label;
                CheckBox cb = row.FindControl("IsMarried") as CheckBox;
                //using ismale
                if (hdnGender.Text.ToLower() == "true")
                    hdnGender.Text = "Male";
                else
                    hdnGender.Text = "Female";

                if (hdnMarried.Text.ToLower() == "true")
                    cb.Checked = true;
                else
                    cb.Checked = false;

            }
        }

        protected void SalaryDetailsView_DataBound(object sender, EventArgs e)
        {
            if (SalaryDetailsView.CurrentMode == DetailsViewMode.Insert ||
                SalaryDetailsView.CurrentMode == DetailsViewMode.Edit)
            {
                DetailsView view = (DetailsView)sender;
                DetailsViewRowCollection rows = view.Rows;
                DetailsViewRow row = rows[0];
                DropDownList ddlOTR = (DropDownList)row.FindControl("ddlOverTimeRate");
                StringBuilder cstext1 = new StringBuilder();
                ddlOTR.DataSource = WC.GetOverTimeRateFromCache();
                ddlOTR.DataTextField = "Value";
                ddlOTR.DataValueField = "Key";
               
                
                DropDownList ddlEmpType = (DropDownList)row.FindControl("ddlEmpType");
                ddlEmpType.DataSource = WC.GetEmployeeTypeFromCache();
                ddlEmpType.DataTextField = "Value";
                ddlEmpType.DataValueField = "Key";
               

                if (SalaryDetailsView.CurrentMode == DetailsViewMode.Edit)
                {
                    Label hdnOTRate = row.FindControl("hdnOverTimeRate") as Label;
                    ddlOTR.SelectedValue = hdnOTRate.Text;

                    Label hdnEmptype = row.FindControl("hdnEmptype") as Label;
                    ddlEmpType.SelectedValue = hdnEmptype.Text;
                    
                    CheckBox isBasicDaily = row.FindControl("cbIsBasicDaily") as CheckBox;
                    CheckBox isAllowanceDaily = row.FindControl("cbIsAllowanceDaily") as CheckBox; //"<script type=text/javascript>
                    cstext1.Append("ShowHideBasic(" + isBasicDaily.Checked.ToString().ToLower() + ");");
                    cstext1.Append("ShowHideAllowance(" + isAllowanceDaily.Checked.ToString().ToLower() + ");"); //</script>");
                    cstext1.Append("ShowHidetr(" + hdnEmptype.Text + ");"); //</script>");
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", cstext1.ToString(), true);
                    
                    //ClientScript.RegisterStartupScript(GetType(), "load", cstext1.ToString());
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "load", cstext1.ToString());
                    //DUP.Update();
                }
                else if (SalaryDetailsView.CurrentMode == DetailsViewMode.Insert)
                {                    
                    cstext1.Append("ShowHideBasic(false);ShowHideAllowance(false);");
                    
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", cstext1.ToString(), true);
                   
                }
                ddlOTR.DataBind();
                ddlEmpType.DataBind();
               
            }
            else if (SalaryDetailsView.CurrentMode == DetailsViewMode.ReadOnly)
            {
                DetailsView view = (DetailsView)sender;
                DetailsViewRowCollection rows = view.Rows;
                DetailsViewRow row = rows[0];
                StringBuilder cstext1 = new StringBuilder();
                CheckBox isBasicDaily = row.FindControl("cbIsBasicDaily") as CheckBox;
                CheckBox isAllowanceDaily = row.FindControl("cbIsAllowanceDaily") as CheckBox;
                Label lblemptype1 = row.FindControl("hdnlblemptype") as Label;
                //cstext1.Append("<script type=text/javascript>");
                cstext1.Append("ShowHideBasic(" + isBasicDaily.Checked.ToString().ToLower() + ");");
                cstext1.Append("ShowHideAllowance(" + isAllowanceDaily.Checked.ToString().ToLower() + ");");
                cstext1.Append("ShowHidetr(" + lblemptype1.Text + ");");
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", cstext1.ToString(), true);
                //ClientScript.RegisterStartupScript(GetType(), "load", cstext1.ToString());   
            }
        }

        protected void ddlEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        protected void ProfTaxchecked(object sender, EventArgs e)
        {
            CheckBox cb1= sender as CheckBox;
            Label lblProfessionalTaxPercent1 = (Label)SalaryDetailsView.FindControl("lblProfessionalTaxPercent1");
            if (cb1.Checked)
            {
                lblProfessionalTaxPercent1.Text = "250";
            }
            else
            {
                lblProfessionalTaxPercent1.Text = "0.0";
            }
        }
        protected void ESICchecked(object sender, EventArgs e)
        {
            CheckBox cb1 = sender as CheckBox;
            Label lblESICTaxPercent1 = (Label)SalaryDetailsView.FindControl("lblESICTaxPercent1");
            if (cb1.Checked)
            {
                lblESICTaxPercent1.Text = "1.75";
            }
            else
            {
                lblESICTaxPercent1.Text = "0.0";
            }
        }
        protected void PFchecked(object sender, EventArgs e)
        {
            CheckBox cb1 = sender as CheckBox;
            Label lblProvidentFundPercent1 = (Label)SalaryDetailsView.FindControl("lblProvidentFundPercent1");
            if (cb1.Checked)
            {
                lblProvidentFundPercent1.Text = "12.5";
            }
            else
            {
                lblProvidentFundPercent1.Text = "0.0";
            }

        }
    
    }
}


