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
using System.IO;
using AllModules.Payroll.BusLogic;
using AllModules.Payroll.Data;
using AllModules.Common;

namespace Payroll.Forms
{
    public partial class ManageEmployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";
           
            
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

                    //DUP.Update();
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

                    //DUP.Update();

                }
            }
            else
            {
               //string ControlName = SalaryDetailsView.CurrentMode == DetailsViewMode.Edit ? "Econtacts" : "contacts";

                Payroll.Controls.ucLeaveTransactions uc = SalaryDetailsView.Rows[0].FindControl("UCLeavesTransaction") as Payroll.Controls.ucLeaveTransactions;
                Payroll.Controls.ucAdvanceTransactions ucA = SalaryDetailsView.Rows[0].FindControl("UCAdvanceTransaction") as Payroll.Controls.ucAdvanceTransactions;
                if (uc != null)
                {
                    uc.LeavesUpdated += new EventHandler(uc_LeavesUpdated);
                    ManipulateSalaryDetailsViewThroughScript(SalaryDetailsView.Rows[0], true);
                }
                if (ucA != null)
                {
                    ucA.AdvanceUpdated += new EventHandler(ucA_AdvanceUpdated);
                }
            }
        }

        void ucA_AdvanceUpdated(object sender, EventArgs e)
        {
            Payroll.Controls.ucAdvanceTransactions uc = sender as Payroll.Controls.ucAdvanceTransactions;
            LinkButton lnkAdvancePending = SalaryDetailsView.Rows[0].FindControl("lnkAdvancePending") as LinkButton;
            lnkAdvancePending.Text = uc.CurrentBalance.ToString("00");
        }

        void uc_LeavesUpdated(object sender, EventArgs e)
        {
            Payroll.Controls.ucLeaveTransactions uc = sender as Payroll.Controls.ucLeaveTransactions;                          
            LinkButton lnkBalanceLeaves = SalaryDetailsView.Rows[0].FindControl("lnkBalanceLeaves") as LinkButton;             
            lnkBalanceLeaves.Text = uc.CurrentBalance.ToString("00");
            //DUP.Update();
            
        }
        //Store FileUpload in session
        public void StoreFileUploadInSession()
        {
            FileUpload fup = EmployeeView.FindControl("fup") as FileUpload;
            if (fup != null)
            {
                if (Session["FileUpload1"] == null )
                {
                    Session["FileUpload1"] = fup;

                }
                // Next time submit and Session has values but FileUpload is Blank 
                // Return the values from session to FileUpload 
                else if (Session["FileUpload1"] != null && (!fup.HasFile))
                {
                    fup = (FileUpload)Session["FileUpload1"];

                }
                // Now there could be another sictution when Session has File but user want to change the file 
                // In this case we have to change the file in session object 
                else if (fup.HasFile)
                {
                    Session["FileUpload1"] = fup;
                }
            }
        }
      
        protected void EmployeeView_ItemCreated(object sender, EventArgs e)
        {
            if (EmployeeDetailsView.CurrentMode == DetailsViewMode.Edit)
            {
                //StoreFileUploadInSession();
            }
        }
        protected void FileUpload_Click(object sender, EventArgs e)
        {   // IMPORTANT: DO NOT REMOVE THIS CODE
            // Fix for FileUpload - losing data on postback
            // http://www.codeproject.com/Articles/16945/Simple-AJAX-File-Upload
            //this.Page.Form.Enctype = "multipart/form-data";
        }
        protected void ToggleAmendInsertVisible(Boolean bMode)
        {

            if (bMode == true)
            {
                btnAmend.Visible = false;
                //btnPrint.Visible = false;
                btnUpdate.Visible = false;
                btnInsert.Visible = true;
                btnCancel.Visible = false;
            }
            else
            {
                //btnPrint.Visible = true;
                btnAmend.Visible = true;
                btnInsert.Visible = false;
                btnCancel.Visible = false;
                btnUpdate.Visible = false;
                
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    //btnPrint.Attributes.Add("onclick", "PrintReportInNewWindow(" + Request.QueryString["ID"] + "); return false;");
                }
            }

        }

        protected void ToggleAmendUpdateVisible(Boolean bMode)
        {
            
            if (bMode == true)
            {
                btnAmend.Visible = true;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            else
            {
                btnAmend.Visible = false;
                btnUpdate.Visible = true;
                btnCancel.Visible = true;
            }

        }

        protected void btnAmend_Click(object sender, EventArgs e)
        {
            ToggleAmendUpdateVisible(false);
            EmployeeView.ChangeMode(DetailsViewMode.Edit);
            EmployeeDetailsView.ChangeMode(DetailsViewMode.Edit);
            SalaryDetailsView.ChangeMode(DetailsViewMode.Edit);

            //DUP.Update();

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
             
            //DUP.Update();
        }
        
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            EmployeeData        oData  = CopyUIToEmployeeData();
            EmployeeDetailsData oeData = CopyUIToEmployeeDetailsData();
            SalaryDetailsData osData = CopyUIToSalaryDetailsData();
            FilesData fsData = CopyUIToFilesData();
            Files files = new Files();
            if (oData.FileID > 0 && fsData != null && fsData.OleAttach != null)
            {
                files.ReplaceFile(fsData, oData.FileID);
            }
            else if (fsData != null && fsData.FileName != null && fsData.OleAttach != null)
            {
                oData.FileID = files.InsertNewFile(fsData);
            }
             
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
                    //DUP.Update();
                     

                }

        }
        
        protected void btnInsert_Click(object sender, EventArgs e)
        {

                EmployeeData oData = CopyUIToEmployeeData();
                EmployeeDetailsData oeData = CopyUIToEmployeeDetailsData();
                SalaryDetailsData osData = CopyUIToSalaryDetailsData();
                //todo set createdby for oData to userid
                FilesData fsData = CopyUIToFilesData();
                Files files = new Files();
                if (oData.FileID > 0 && fsData != null && fsData.OleAttach != null)
                {
                    files.ReplaceFile(fsData, oData.FileID);
                }
                else if (fsData != null && fsData.FileName != null && fsData.OleAttach != null)
                {
                    oData.FileID = files.InsertNewFile(fsData);
                }
                
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
            HiddenField fileID  = EmployeeView.Rows[0].FindControl("FileID") as HiddenField;            
            TextBox txtDesig    = EmployeeView.Rows[0].FindControl("txtDesig") as TextBox;            
            TextBox txtJoinDate = EmployeeView.Rows[0].FindControl("txtJoinDate") as TextBox;
            Label txtCreatedOn  = EmployeeView.Rows[0].FindControl("txtCreatedOn") as Label;
            Label txtModifiedOn = EmployeeView.Rows[0].FindControl("txtModifiedOn") as Label;
            CheckBox isActive   = EmployeeView.Rows[0].FindControl("IsActive") as CheckBox;
            TextBox txtPF       = EmployeeView.Rows[0].FindControl("txtPF") as TextBox;
            TextBox txtESIC     = EmployeeView.Rows[0].FindControl("txtESIC") as TextBox;
            TextBox txtPAN      = EmployeeView.Rows[0].FindControl("txtPAN") as TextBox;
            TextBox txtfname    = EmployeeView.Rows[0].FindControl("txtfname") as TextBox;
            TextBox txtmname    = EmployeeView.Rows[0].FindControl("txtmname") as TextBox;
            TextBox txtlname    = EmployeeView.Rows[0].FindControl("txtlname") as TextBox;
            TextBox txtdob      = EmployeeView.Rows[0].FindControl("txtdob") as TextBox;
            RadioButtonList rblGender = EmployeeView.Rows[0].FindControl("rblGender") as RadioButtonList;
            CheckBox cbMarried  = EmployeeView.Rows[0].FindControl("IsMarried") as CheckBox;
            

            EmployeeData oData = new EmployeeData();
            //Assign data
            oData.FirstName = txtfname.Text;
            oData.MiddleName = txtmname.Text;
            oData.LastName = txtlname.Text;
            oData.Gender = Convert.ToBoolean(rblGender.SelectedValue);
            oData.Married = cbMarried.Checked;
            oData.BirthDate = ASM.Core.ResolveDateToUS(txtdob.Text);
            oData.Designation = txtDesig.Text;
            CultureInfo cultEnGb = new CultureInfo("en-GB");
            oData.Active = isActive.Checked;
            oData.ModifiedOn = DateTime.Now;
            oData.PF    = txtPF.Text;
            oData.ESIC  = txtESIC.Text;
            oData.PAN   = txtPAN.Text;
            if (fileID != null && fileID.Value != string.Empty)
            {
                oData.FileID = Convert.ToInt32(fileID.Value);
            }
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
            HiddenField lblID = EmployeeDetailsView.Rows[0].FindControl("hdnID") as HiddenField;
            
            TextBox txtAddress1 = EmployeeDetailsView.Rows[0].FindControl("txtAddressLine1") as TextBox;
            TextBox txtAddress2 = EmployeeDetailsView.Rows[0].FindControl("txtAddressLine2") as TextBox;
            TextBox txtCity = EmployeeDetailsView.Rows[0].FindControl("txtCity") as TextBox;
            TextBox txtState = EmployeeDetailsView.Rows[0].FindControl("txtState") as TextBox;
            TextBox txtCountry = EmployeeDetailsView.Rows[0].FindControl("txtCountry") as TextBox;
            TextBox txtPinCode = EmployeeDetailsView.Rows[0].FindControl("txtPinCode") as TextBox;
            TextBox txtHomePhone = EmployeeDetailsView.Rows[0].FindControl("txtHomePhone") as TextBox;
            TextBox txtMobile = EmployeeDetailsView.Rows[0].FindControl("txtMobile") as TextBox;
            TextBox txtworkPhone = EmployeeDetailsView.Rows[0].FindControl("txtworkPhone") as TextBox;
           
            EmployeeDetailsData oData = new EmployeeDetailsData();
            //Assign data
            
            oData.AddressLine1 = txtAddress1.Text;
            oData.AddressLine2 = txtAddress2.Text;
            oData.City = txtCity.Text;
            oData.State = txtState.Text;
            oData.Country = txtCountry.Text;
            oData.PinCode = txtPinCode.Text;
            oData.HomePhone = txtHomePhone.Text;
            oData.Mobile = txtMobile.Text;
            oData.WorkPhone = txtworkPhone.Text;
            oData.ModifiedOn = DateTime.Now;

            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {                 
                oData.EmpID = Convert.ToInt32(Request.QueryString["ID"]);
                oData.ID =  Convert.ToInt32(lblID.Value);
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
            LinkButton lnkBalanceLeaves = SalaryDetailsView.Rows[0].FindControl("lnkBalanceLeaves") as LinkButton;
            //LinkButton lnkAdvancePending = SalaryDetailsView.Rows[0].FindControl("lnkAdvancePending") as LinkButton; // Removing from Db Not Required
            TextBox txtYearlyPaidLeaves = SalaryDetailsView.Rows[0].FindControl("txtYearlyPaidLeaves") as TextBox;

            //New fields
            Label lblAdvancePending1 = SalaryDetailsView.Rows[0].FindControl("lblAdvancePending1") as Label;
            TextBox lblProfessionalTaxPercent1 = SalaryDetailsView.Rows[0].FindControl("txtProfessionalTaxPercent1") as TextBox;
            TextBox lblESICTaxPercent1 = SalaryDetailsView.Rows[0].FindControl("txtESICTaxPercent1") as TextBox;
            TextBox lblProvidentFundPercent1 = SalaryDetailsView.Rows[0].FindControl("txtProvidentFundPercent1") as TextBox;
            TextBox lblTravelAllowance1 = SalaryDetailsView.Rows[0].FindControl("txtTravelAllowance1") as TextBox;
            TextBox lblDearnessAllowance1 = SalaryDetailsView.Rows[0].FindControl("txtDearnessAllowance1") as TextBox;
            //New fields
            //CheckBox cbHasAllowance = SalaryDetailsView.Rows[0].FindControl("cbHasAllowance") as CheckBox;
            //CheckBox cbHasLeaves = SalaryDetailsView.Rows[0].FindControl("cbHasLeaves") as CheckBox;
            //CheckBox cbHasOverTime = SalaryDetailsView.Rows[0].FindControl("cbHasOverTime") as CheckBox;
            CheckBox cbDeductESIC = SalaryDetailsView.Rows[0].FindControl("cbDeductESIC") as CheckBox;
            CheckBox cbDeductProfTax = SalaryDetailsView.Rows[0].FindControl("cbDeductProfTax") as CheckBox;
            CheckBox cbDeductPF = SalaryDetailsView.Rows[0].FindControl("cbDeductPF") as CheckBox;
            CheckBox cbCreditTA = SalaryDetailsView.Rows[0].FindControl("cbCreditTA") as CheckBox;
            CheckBox cbCreditDA = SalaryDetailsView.Rows[0].FindControl("cbCreditDA") as CheckBox;

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

             //if (lnkBalanceLeaves != null)
             //{
             //    float.TryParse(lnkBalanceLeaves.Text, out parseValue);
             //    oData.BalanceLeaves = parseValue;
             //}
             //if (lnkAdvancePending != null)
             //{
             //    float.TryParse(lnkAdvancePending.Text, out parseValue);
             //    oData.AdvancePending = parseValue;
             //}

             float.TryParse(txtYearlyPaidLeaves.Text, out parseValue);
             oData.YearlyPaidLeaves = parseValue;

             //New fields
             float.TryParse(lblProfessionalTaxPercent1.Text, out parseValue);
             oData.ProfessionalTaxPercent = parseValue;
             float.TryParse(lblESICTaxPercent1.Text, out parseValue);
             oData.ESICTaxPercent = parseValue;
             float.TryParse(lblProvidentFundPercent1.Text, out parseValue);
             oData.ProvidentFundPercent = parseValue;
             float.TryParse(lblTravelAllowance1.Text, out parseValue);
             oData.TravelAllowance = parseValue;
             float.TryParse(lblDearnessAllowance1.Text, out parseValue);
             oData.DearnessAllowance = parseValue;
            

             //New fields           
             //oData.HasAllowance = cbHasAllowance.Checked;
             //oData.HasLeaves = cbHasLeaves.Checked;
             //oData.HasOverTime = cbHasOverTime.Checked;
             oData.DeductESIC = cbDeductESIC.Checked;
             oData.DeductProfTax = cbDeductProfTax.Checked;
             oData.DeductPF = cbDeductPF.Checked;
             oData.CreditTA = cbCreditTA.Checked;
             oData.CreditDA = cbCreditDA.Checked;

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

        public FilesData CopyUIToFilesData()
        {
            FilesData fs = new FilesData();
            FileUpload fup = EmployeeView.FindControl("fup") as FileUpload;
            if (fup.HasFile)
            {
                fs.FileName = fup.FileName;
                fs.FileSize = fup.FileBytes.Length;
                fs.OleAttach = fup.FileBytes;
                string ext = System.IO.Path.GetExtension(fs.FileName).ToLower();
                fs.FileType = CoreAssemblies.Core.GetMimeType(ext);
            }
            return fs;
        }
        protected void DetailsView_OnItemUpdating(object sender,DetailsViewUpdateEventArgs e) 
        {
            //Do Stuff 
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
                    //Image img = row.FindControl("imgfile") as Image;
                    //HiddenField fileID = row.FindControl("FileID") as HiddenField;
                    //if (fileID.Value != string.Empty && Convert.ToInt32(fileID.Value) > 0)
                    //{
                    //    Files files =  new Files();
                    //    FilesData fs = files.GetFilesData(fileID.Value);
                    //    img.ImageUrl = "data:" + fs.FileType + ";base64," + Convert.ToBase64String(fs.OleAttach, 0, fs.OleAttach.Length);
                    //} 

                    Label hdnActive = row.FindControl("hdnActive") as Label;                    
                    CheckBox cb = row.FindControl("IsActive") as CheckBox; 
                    if (hdnActive.Text.ToLower() == "true")
                        cb.Checked = true;
                    else
                        cb.Checked = false;

                    RadioButtonList ddlEmpType = (RadioButtonList)row.FindControl("rblGender");
                    Label hdnGender = row.FindControl("hdnGender") as Label;
                    Label hdnMarried = row.FindControl("hdnMarried") as Label;
                     cb = row.FindControl("IsMarried") as CheckBox;
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

                Label hdnGender = row.FindControl("hdnGender") as Label;
                Label hdnMarried = row.FindControl("hdnMarried") as Label;
                cb = row.FindControl("IsMarried") as CheckBox;
                //using ismale
                if (hdnGender.Text.ToLower() == "true")
                    hdnGender.Text = "Male";
                else
                    hdnGender.Text = "Female";

                if (hdnMarried.Text.ToLower() == "true")
                    cb.Checked = true;
                else
                    cb.Checked = false;

                //if (fileID.Value != string.Empty && Convert.ToInt32(fileID.Value) > 0)
                //{
                //    Files files = new Files();
                //    FilesData fs = files.GetFilesData(fileID.Value);
                   
                //    //img.ImageUrl = "data:" + fs.FileType + ";base64," + Convert.ToBase64String(fs.OleAttach,0,fs.OleAttach.Length);
                //    ////img.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])files.GetFileStream(fileID.Value));
                //    //img.Visible = true;
                //    //img.DataBind();
                //}

            }
        }

        protected void EmployeeDetailsView_DataBound(object sender, EventArgs e)
        {
            //if (EmployeeDetailsView.CurrentMode == DetailsViewMode.Insert ||
            //    EmployeeDetailsView.CurrentMode == DetailsViewMode.Edit)
            //{
            //    DetailsView view = (DetailsView)sender;
            //    DetailsViewRowCollection rows = view.Rows;
            //    DetailsViewRow row = rows[0];
                
            //    if (EmployeeView.CurrentMode == DetailsViewMode.Edit)
            //    {
                    
            //    }

            //}
            //else if (EmployeeView.CurrentMode == DetailsViewMode.ReadOnly)
            //{
            //    DetailsView view = (DetailsView)sender;
            //    DetailsViewRowCollection rows = view.Rows;
            //    DetailsViewRow row = rows[0];
                

            //}
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
                    HiddenField hdnEmptype = row.FindControl("hdnEmptype") as HiddenField;
                    ddlEmpType.SelectedValue = hdnEmptype.Value;
                    

                    ManipulateSalaryDetailsViewThroughScript(row, true);
                    
                    
                    //ClientScript.RegisterStartupScript(GetType(), "load", cstext1.ToString());
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "load", cstext1.ToString());
                    
                }
                else if (SalaryDetailsView.CurrentMode == DetailsViewMode.Insert)
                {
                    ManipulateSalaryDetailsViewThroughScript(row, false);
                    
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
            //DUP.Update();
        }

        public void ManipulateSalaryDetailsViewThroughScript(DetailsViewRow row,bool IsEditMode)
        {
            //TODO: create an enum to identify
            StringBuilder cstext1 = new StringBuilder();
            if (IsEditMode)
            {
                HiddenField hdnEmptype = row.FindControl("hdnEmptype") as HiddenField;
                DropDownList ddlEmpType = row.FindControl("ddlEmpType") as DropDownList;
                ddlEmpType.Attributes.Add("onchange", "ShowHidetrEdit( this.options[this.selectedIndex].value ,'" + hdnEmptype.ClientID + "');");
                CheckBox isBasicDaily = row.FindControl("cbIsBasicDaily") as CheckBox;
                CheckBox isAllowanceDaily = row.FindControl("cbIsAllowanceDaily") as CheckBox; //"<script type=text/javascript>
                cstext1.Append("ShowHideBasic(" + isBasicDaily.Checked.ToString().ToLower() + ");");
                cstext1.Append("ShowHideAllowance(" + isAllowanceDaily.Checked.ToString().ToLower() + ");"); //</script>");
                cstext1.Append("ShowHidetrEdit(" + hdnEmptype.Value + ",'" + hdnEmptype.ClientID + "');"); //</script>");
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", cstext1.ToString(), true);
            }
            else //insert
            {
                // Only for the case of insert we are loading the values from configuration
                TextBox txtProfessionalTaxPercent1 = row.FindControl("txtProfessionalTaxPercent1") as TextBox;
                TextBox txtESICTaxPercent1 = row.FindControl("txtESICTaxPercent1") as TextBox;
                TextBox txtProvidentFundPercent1 = row.FindControl("txtProvidentFundPercent1") as TextBox;
                TextBox txtTravelAllowance1 = row.FindControl("txtTravelAllowance1") as TextBox;
                TextBox txtDearnessAllowance1 = row.FindControl("txtDearnessAllowance1") as TextBox;
                clsConfiguration config = new clsConfiguration();
                clsConfigurationData cData = config.GetConfiguration();
                txtProfessionalTaxPercent1.Text = cData.ProfessionalTax.ToString();
                txtESICTaxPercent1.Text = cData.ESIC.ToString();
                txtProvidentFundPercent1.Text = cData.ProvidentFund.ToString();
                txtTravelAllowance1.Text = cData.TravelAllowance.ToString();
                txtDearnessAllowance1.Text = cData.DearnessAllowance.ToString();
                cstext1.Append("ShowHideBasic(false);ShowHideAllowance(false);");
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", cstext1.ToString(), true);
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
            TextBox lblProfessionalTaxPercent1 = (TextBox)SalaryDetailsView.FindControl("txtProfessionalTaxPercent1");
            RetainBDDMProperties();
            clsConfiguration config = new clsConfiguration();
            clsConfigurationData cData = config.GetConfiguration();

            if (cb1.Checked)
            {
                lblProfessionalTaxPercent1.Text = cData.ProfessionalTax.ToString();
            }
            else
            {
                lblProfessionalTaxPercent1.Text = "0.0";
            }
        }
        protected void ESICchecked(object sender, EventArgs e)
        {
            CheckBox cb1 = sender as CheckBox;
            TextBox lblESICTaxPercent1 = (TextBox)SalaryDetailsView.FindControl("txtESICTaxPercent1");
            RetainBDDMProperties();
            clsConfiguration config = new clsConfiguration();
            clsConfigurationData cData = config.GetConfiguration();
            if (cb1.Checked)
            {
                lblESICTaxPercent1.Text = cData.ESIC.ToString();
            }
            else
            {
                lblESICTaxPercent1.Text = "0.0";
            }
        }
        protected void PFchecked(object sender, EventArgs e)
        {
            CheckBox cb1 = sender as CheckBox;
            TextBox lblProvidentFundPercent1 = (TextBox)SalaryDetailsView.FindControl("txtProvidentFundPercent1");
            RetainBDDMProperties();
            clsConfiguration config = new clsConfiguration();
            clsConfigurationData cData = config.GetConfiguration();
            if (cb1.Checked)
            {
                lblProvidentFundPercent1.Text = cData.ProvidentFund.ToString();
            }
            else
            {
                lblProvidentFundPercent1.Text = "0.0";
            }

        }
        protected void TAchecked(object sender, EventArgs e)
        {
            CheckBox cb1 = sender as CheckBox;
            TextBox lblTravelAllowance = (TextBox)SalaryDetailsView.FindControl("txtTravelAllowance1");
            RetainBDDMProperties();
            if (cb1.Checked)
            {
                lblTravelAllowance.Text = "10";
            }
            else
            {
                lblTravelAllowance.Text = "0.0";
            }

        }
        protected void DAchecked(object sender, EventArgs e)
        {
            CheckBox cb1 = sender as CheckBox;
            TextBox lblTravelAllowance = (TextBox)SalaryDetailsView.FindControl("txtDearnessAllowance1");
            RetainBDDMProperties();
            if (cb1.Checked)
            {
                lblTravelAllowance.Text = "10";
            }
            else
            {
                lblTravelAllowance.Text = "0.0";
            }

        }

        public void RetainBDDMProperties()        
        {
            bool isBasicChecked =false;
             bool isAllowanceChecked =false;
            if(BasicChecked.Value != null)
            {
                isBasicChecked=Convert.ToBoolean(BasicChecked.Value);
            }
             if(BasicChecked.Value != null)
            {
                isAllowanceChecked = Convert.ToBoolean(AllowanceChecked.Value);
            }
              
             StringBuilder cstext1 = new StringBuilder();
             cstext1.Append("ShowHideBasic(" + isBasicChecked.ToString().ToLower() + ");");
             cstext1.Append("ShowHideAllowance(" + isAllowanceChecked.ToString().ToLower() + ");");
             ScriptManager.RegisterStartupScript(Page, this.GetType(), "load", cstext1.ToString(), true);
        }

        protected void Trigger_Click(object sender, EventArgs e)
        {
            //euPop,up2
            if (SalaryDetailsView.CurrentMode == DetailsViewMode.Edit)
            {
                 
                LinkButton lnkBalanceLeaves = SalaryDetailsView.Rows[0].FindControl("lnkBalanceLeaves") as LinkButton;
                LinkButton lnkAdvancePending = SalaryDetailsView.Rows[0].FindControl("lnkAdvancePending") as LinkButton;
                Leaves lv = new Leaves();
                Advance adv = new Advance();
                string empID = Request.QueryString["ID"];
                float? leavesBalance = lv.GetCurrentBalance(Convert.ToInt32(empID));
                float? advBalance = adv.GetCurrentBalance(Convert.ToInt32(empID));
                lnkBalanceLeaves.Text = leavesBalance.Value.ToString("00");
                lnkAdvancePending.Text = advBalance.Value.ToString("00");
                UpdatePanel up1 = SalaryDetailsView.Rows[0].FindControl("up2") as UpdatePanel;
                UpdatePanel up2 = SalaryDetailsView.Rows[0].FindControl("EuPop") as UpdatePanel;
                up1.Update();
                up2.Update();
            }

        }
    }
}



