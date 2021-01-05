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
using System.Collections.Generic;
using Payroll.Common;
using ENM = CoreAssemblies.EnumClass;
using WC = Payroll.Common.WebCommon;
using Payroll.Data;

namespace Payroll.Forms
{
    public partial class ManageSalary : System.Web.UI.Page
    {
        private CurrentMonthDetails cmd = null;
        private int empID = 0;
        private int dmID = 0;
        public string EmpType
        {
            get
            {
                EmployeeType et = new EmployeeType();
                return et.GetEmployeeTypeByName(EmpID);                
            }
        }
        public int EmpID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["EmpID"]))
                    empID = Convert.ToInt32(Request.QueryString["EmpID"]);            
                return empID; }
            set { empID = value; }
        }        
        public int DMID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["DMID"]))
                    dmID = Convert.ToInt32(Request.QueryString["DMID"]); 
                    return dmID; }
            set { dmID = value; }
        }      
        private CurrentMonthDetails getObject()
        {
            if (ViewState["CMD"] != null)
            {
                cmd = (CurrentMonthDetails)ViewState["CMD"];
            }
            else
            {
                cmd = new CurrentMonthDetails();
            }
            return cmd; 
        }
        private  void setObject(CurrentMonthDetails cm)
        {
            ViewState["CMD"] = cm;
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
           
            if (!Page.IsPostBack)
            {
                if (EmpID == 0)
                {
                    // Emp id does not exist
                    Response.Redirect("Error Page");
                }
                else if (DMID == 0)
                {
                    //Insert Mode landing page, User clicked ADD button from Salary Search View
                    //SDView.DataBind();
                    SDView.ChangeMode(DetailsViewMode.Insert);
                    ChangeActionButton(true);
                    
                }
                else
                {
                    // EmpID & DMID Both are fine, 
                    // need to check if salary exist in the db 
                    Salary sal = new Salary();
                    if (!sal.ExistingSalaryCheck(EmpID, DMID))
                    {
                        Response.Redirect("Error Page");
                        //~/Payroll/Forms/SalaryInformation.aspx
                    }
                    else
                    {
                        //Now show in ReadOnly mode
                        ChangeActionButton(false);

                    }

                }

               
            }
        }

        private void ChangeActionButton(bool status)
        {
            if (status)
            {              
                EditSalary.Visible = false;
                UpdateSalary.Visible = false;
                Cancel.Visible = false;
                Back.Visible = true;
                SaveNewSalary.Visible = true;
            }
            else
            {               
                EditSalary.Visible = true;
                UpdateSalary.Visible = false;
                Back.Visible = true;
                Cancel.Visible = false;
                SaveNewSalary.Visible = false;
                Salary sal = new Salary();
                MembershipUser user = Membership.GetUser();
                //bool IsUserAdmin = AllModules.Validate.UserRoleAccess(Convert.ToInt32(user.ProviderUserKey), CoreAssemblies.EnumClass.Roles.Admin);
                bool IsSalaryLocked = sal.SalaryLockCheck(EmpID, DMID);

                if (IsSalaryLocked)
                {
                    EditSalary.Visible = false;
                    Cancel.Visible = false;                    
                }
                else
                {
                    EditSalary.Visible = true;                   
                }
            }
            
        }

        #region Button Events       
        protected void EditSalary_Click(object sender, EventArgs e)
        {
            // Make sure the month and year is selected,
            // Way to handle earlier saved salaries
            EditSalary.Visible = false;
            UpdateSalary.Visible = true;
            Cancel.Visible = true;
            SDView.ChangeMode(DetailsViewMode.Edit);
            SDView.DataBind();
            
        }
        protected void SaveNewSalary_Click(object sender, EventArgs e)
        {
            SalaryData sd = CopyUIToObject();
            PayrollService ps = new PayrollService();
            ps.CalculateSalary(sd, cmd);
            sd.EmpTypeName = EmpType;
            int i = ps.InsertUpdateSalaryToDb(sd, true,cmd);
            if (i > 0)
            {
                Response.Redirect("~/Payroll/Forms/ManageSalary.aspx?EmpID=" + sd.EmpID + "&DMID=" + sd.DMID);
            }
        }
        protected void UpdateSalary_Click(object sender, EventArgs e)
        {
            SalaryData sd = CopyUIToObject();
            PayrollService ps = new PayrollService();
            ps.CalculateSalary(sd, cmd);
            sd.EmpTypeName = EmpType;
            int i = ps.InsertUpdateSalaryToDb(sd, false, cmd);
            if (i > 0)
            {               
                UpdateSalary.Visible = false;
                Cancel.Visible = false;
                SDView.ChangeMode(DetailsViewMode.ReadOnly);
                SDView.DataBind();
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (SDView.CurrentMode == DetailsViewMode.Edit)
            {
                EditSalary.Visible = true;
                UpdateSalary.Visible = false;
                Cancel.Visible = false;
                Back.Visible = true;
                SDView.ChangeMode(DetailsViewMode.ReadOnly);
                SDView.DataBind();
            }
            //else if(SDView.CurrentMode ==DetailsViewMode.ReadOnly)
            //{
            //    Response.Redirect("~/Payroll/Forms/SalaryInformation.aspx?EmpID=" + EmpID);
            //}
            //else if (SDView.CurrentMode == DetailsViewMode.Insert)
            //{
            //    Response.Redirect("~/Payroll/Forms/SalaryInformation.aspx?EmpID=" + EmpID);
            //}

        }
        protected void Back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Payroll/Forms/SalaryInformation.aspx?EmpID=" + EmpID);
        }
        //protected void DeleteSalary_Click(object sender, EventArgs e) {} 
        //protected void AddNewSalary_Click(object sender, EventArgs e){}
        
        protected void Calculate_Click(object sender, EventArgs e)
        {
            //Ideally when we click on Calculate, a web service should be called
            // This will take EMPId, DMID, GovtHolidays,DaysWorked as input
            // and return a SalaryData object back. The whole process should be independent 
            // of this structure
            SalaryData sd = CopyUIToObject();
            PayrollService ps = new PayrollService();
            ps.CalculateSalary(sd, cmd);
            if ((sd.DaysWorked + sd.GovtCompHoliday) > cmd.TotalDaysOfMonth)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ErrorDays", "<script> alert('Days Worked + Govt Holiday exceeds Total Days of Month');</script>", false);
            }
            else
            {
                CopyObjectToUI(sd);
            }

        }
        #endregion

        #region DataBound Events
        protected void SalaryDetails_DataBound(object sender, EventArgs e)
        {
            int scopeMonth = 0;
            int scopeYear = 0;
            //MONTH
            DropDownList ddl = SDView.FindControl("CurrentMonthList") as DropDownList;
           
            //YEAR
            DropDownList ddl2 = SDView.FindControl("CurrentYearList") as DropDownList;
            
            
            cmd = getObject(); //Get object from ViewState   
           
            if (SDView.CurrentMode == DetailsViewMode.Edit)
            {
                // Case of Edit - Use DMID to fetch details of month year & bind
                Label txtMonth = SDView.FindControl("TxtMonth") as Label;
                Label txtYear = SDView.FindControl("TxtYear") as Label;
                SalaryHelper.iGetMonthNameYearFromDMID(DMID, ref scopeMonth, ref scopeYear);
                txtYear.Text = scopeYear.ToString();
                txtMonth.Text = Enum.GetName(typeof(ENM.MonthsOfYear), scopeMonth);
                cmd.SelectedMonth = scopeMonth;
                cmd.SelectedYear = scopeYear;              
            }
            else if (SDView.CurrentMode == DetailsViewMode.Insert)
            {
                // Case of Insert - Use Current Month details for Month and Year
                if (ddl != null)
                {                    
                    ddl.DataTextField = "MonthName";
                    ddl.DataValueField = "MonthValue";
                    ddl.DataSource = Payroll.Common.SalaryHelper.GetMonthsInAYear();
                    scopeMonth = DateTime.Now.Month;
                    ddl.SelectedIndex = scopeMonth - 1;
                    //txtMonth.Text = scopeMonth.ToString();
                    cmd.SelectedMonth = scopeMonth;                    
                    ddl.DataBind();
                }
               
                if (ddl2 != null)
                {
                   
                    List<TYear> years = Payroll.Common.SalaryHelper.GetListOfYear();
                     
                    ddl2.DataTextField = "YearName";
                    ddl2.DataValueField = "YearValue";
                    ddl2.DataSource = years;
                    var cYear = from TYear t in years
                                where t.YearName == DateTime.Now.Year.ToString()
                                select t;
                    ddl2.SelectedIndex = cYear.FirstOrDefault().Index;
                    scopeYear = cYear.FirstOrDefault().YearValue;
                    //txtYear.Text = cYear.FirstOrDefault().YearName;                    
                    cmd.SelectedYear = scopeYear;
                    ddl2.DataBind();
                }               
            }

            ResetMonthdetails(); // this is common to all three modes, Read,Insert,Edit
            

        }

        protected void CurrentYearList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedYear = ((DropDownList)sender).SelectedValue;
            cmd = getObject();
            cmd.SelectedYear = Convert.ToInt16(selectedYear);
            ResetMonthdetails();
            ////Find Drop down list from aspx page            
            //TextBox ddl = SDView.FindControl("TxtYear") as TextBox;
            //if (ddl != null)
            //{
            //    //ddl.Text = selectedYear;
            //    //workflow.MD.SelectedYear = Convert.ToInt16(selectedYear);
            //    //ResetMonthdetails();
            //    //UpdateMonthDetails();
            //}
        }

        protected void CurrentMonthList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedMonth = ((DropDownList)sender).SelectedIndex;
            cmd = getObject();
            selectedMonth += 1;
            cmd.SelectedMonth = selectedMonth;
            ResetMonthdetails();            
        }

        #endregion

        #region Methods

        public void ResetMonthdetails()
        {
            // Need to get the last leave balance . WC> GetLastAdvanceAndLeavesBalance
            if (DMID != 0)
            {             
                Label smonth = SDView.FindControl("TxtMonth") as Label;
                Label iyear = SDView.FindControl("TxtYear") as Label;
                cmd.SelectedMonth = Convert.ToInt32((ENM.MonthsOfYear)Enum.Parse(typeof(ENM.MonthsOfYear), smonth.Text, true));
                cmd.SelectedYear = Convert.ToInt32(iyear.Text);
            }
            SalaryHelper.PopulateMonthDetails(cmd);
            //WC.GetLastAdvanceAndLeavesBalance(EmpID,ref cmd); //todo needs to be reworked
            setObject(cmd);//Put object back to ViewState    
            if (SDView.CurrentMode == DetailsViewMode.Insert ||
                SDView.CurrentMode == DetailsViewMode.Edit)
            {
                // I should get the last leave balance from the previous salary
                TextBox txtWeeklyOff = SDView.FindControl("TxtWeeklyOffs") as TextBox;
                TextBox txtTotalDays = SDView.FindControl("TxtTotalDays") as TextBox;
                TextBox txtLeavesBalancePrevious = SDView.FindControl("TxtLeavesBalancePrevious") as TextBox;
                TextBox txtLeavesBalanceCurrent = SDView.FindControl("TxtLeavesBalanceCurrent") as TextBox;
                TextBox TxtAdvBalancePrevious = SDView.FindControl("TxtAdvBalancePrevious") as TextBox;
                TextBox TxtAdvBalanceCurrent = SDView.FindControl("TxtAdvBalanceCurrent") as TextBox;
                TextBox TxtPaidLeavesTaken = SDView.FindControl("TxtPaidLeavesTaken") as TextBox;
                TextBox TxtAdvBalanceAdd = SDView.FindControl("TxtAdvBalanceAdd") as TextBox;
                TextBox TxtAdvBalanceDeduct = SDView.FindControl("TxtAdvBalanceDeduct") as TextBox;
                                    
                txtTotalDays.Text = cmd.TotalDaysOfMonth.ToString("00");
                txtWeeklyOff.Text = cmd.WeeklyOffs.ToString("00");
                //txtLeavesBalancePrevious.Text = cmd.LeavesBalancePrevious.ToString("00");


                Leaves lv = new Leaves(EmpID,0);
                Advance adv = new Advance(EmpID, 0,0);
                float leavesBalancePrevious,leavesBalanceCurrent=0.0f; //= lv.GetCurrentBalance(EmpID);
                float advanceBalancePrevious,advanceBalanceCurrent=0.0f;// = adv.GetCurrentBalance(EmpID);
                float paidLeavesTaken, advBalanceAdd, advBalanceDeduct;
                float.TryParse(txtLeavesBalancePrevious.Text, out leavesBalancePrevious);
                float.TryParse(TxtAdvBalancePrevious.Text, out advanceBalancePrevious);
                //float.TryParse(TxtAdvBalanceCurrent.Text, out advanceBalanceCurrent);
                float.TryParse(TxtPaidLeavesTaken.Text, out paidLeavesTaken);
                float.TryParse(TxtAdvBalanceAdd.Text, out advBalanceAdd);
                float.TryParse(TxtAdvBalanceDeduct.Text, out advBalanceDeduct);
                //float? lbPrevious = lv.GetCurrentBalance(EmpID);
                //float? abPrevious = adv.GetCurrentBalance(EmpID);

                if (SDView.CurrentMode == DetailsViewMode.Edit)
                {
                    // If system gernerated - i.e. Last updated through Deduction
                    // then the balance is correct, use as it is, nothing changed so far
                    //if (lv.SystemGenerated == 1)
                    //{
                    //    leavesBalancePrevious = lv.PreviousBalance;
                    //}
                    //else
                    //{
                    
                    //if (adv.SystemGenerated == 1)
                    //{
                    //    advanceBalancePrevious = adv.PreviousBalance;
                    //    advanceBalanceCurrent = advanceBalancePrevious - advBalanceAdd + advBalanceDeduct;
                    //}
                    //else
                    //{
                    if (lv.SystemGenerated != 1)
                    {
                        leavesBalancePrevious = lv.CurrentBalance + paidLeavesTaken;
                        leavesBalanceCurrent = leavesBalancePrevious - paidLeavesTaken;
                        txtLeavesBalanceCurrent.Text = leavesBalanceCurrent.ToString();
                        txtLeavesBalancePrevious.Text = leavesBalancePrevious.ToString();
                       
                    }
                        if (adv.SystemGenerated != 1)
                    {
                        advanceBalancePrevious = adv.CurrentBalance - advBalanceAdd + advBalanceDeduct; 
                        advanceBalanceCurrent = advanceBalancePrevious + advBalanceAdd - advBalanceDeduct;
                                     
                        TxtAdvBalancePrevious.Text = advanceBalancePrevious.ToString();
                        TxtAdvBalanceCurrent.Text = advanceBalanceCurrent.ToString();
                    }
                    //else
                    //{
                    //    // This means salary balance is deducted but new balance is added, so 
                    //    // we backtrack the last two and recreate the actual balance
                    //    advanceBalancePrevious = advanceBalancePrevious + lv.CurrentBalance + lv.Debit;
                    //}
                }
                else
                { // INSERT MODE
                   
                    leavesBalancePrevious = lv.CurrentBalance;
                    leavesBalanceCurrent = lv.CurrentBalance;
                    advanceBalancePrevious = adv.CurrentBalance;
                    advanceBalanceCurrent = adv.CurrentBalance;

                    txtLeavesBalanceCurrent.Text = leavesBalanceCurrent.ToString();
                    txtLeavesBalancePrevious.Text = leavesBalancePrevious.ToString();
                    TxtAdvBalancePrevious.Text = advanceBalancePrevious.ToString();
                    TxtAdvBalanceCurrent.Text = advanceBalanceCurrent.ToString();
                }
               
            }
            else if (SDView.CurrentMode == DetailsViewMode.ReadOnly )
            {
                // April 06 2011 update: should display details from current month salary, and not from employee table
                // Otherwise there is lot of confusion, applies for BalLeaves, AdvPending
                Label TxtWeeklyOffs = SDView.FindControl("TxtWeeklyOffs") as Label;
                //Label empType = SDView.FindControl("empType") as Label;
                TxtWeeklyOffs.Text = cmd.WeeklyOffs.ToString("00");              
                //empType.Text = ((ENM.EmployeeType)(Convert.ToInt32(empType.Text))).ToString();
            }
            
        }

        public SalaryData CopyUIToObject()
        {
            SalaryData sd = new SalaryData();
            Label   lblID                       = SDView.FindControl("lblID") as Label;
            //Label lblLeavesCreditTaken          = SDView.FindControl("lblLeavesCreditTaken") as Label;
            TextBox txtWeeklyOff                = SDView.FindControl("TxtWeeklyOffs") as TextBox;
            TextBox txtTotalDays                = SDView.FindControl("TxtTotalDays") as TextBox;
            TextBox txtLeavesBalanceCurrent     = SDView.FindControl("TxtLeavesBalanceCurrent") as TextBox;
            TextBox txtLeavesBalancePrevious    = SDView.FindControl("TxtLeavesBalancePrevious") as TextBox;            
            TextBox txtPaidLeavesTaken          = SDView.FindControl("TxtPaidLeavesTaken") as TextBox;
            TextBox txtUnpaidLeavesTaken        = SDView.FindControl("TxtUnPaidLeavesTaken") as TextBox;
            TextBox txtDaysWorked               = SDView.FindControl("TxtDaysWorked") as TextBox;
            TextBox txtGovtCompHoliday          = SDView.FindControl("TxtGovtCompHoliday") as TextBox;
            TextBox txtTotalPaidDays            = SDView.FindControl("TxtTotalPaidDays") as TextBox;
            //TextBox LblTotalBalanceLeaves       = SDView.FindControl("TotalBalanceLeaves") as TextBox;
            TextBox txtAmountForPaidDays        = SDView.FindControl("TxtAmountForPaidDays") as TextBox;
            TextBox txtOTHours                  = SDView.FindControl("TxtOTHours") as TextBox;
            TextBox txtOTAmount                 = SDView.FindControl("TxtOTAmount") as TextBox;
            TextBox txtAdvBalancePrevious       = SDView.FindControl("TxtAdvBalancePrevious") as TextBox;
            TextBox txtAdvBalanceCurrent        = SDView.FindControl("TxtAdvBalanceCurrent") as TextBox;
            TextBox txtAdvBalanceAdd            = SDView.FindControl("TxtAdvBalanceAdd") as TextBox;
            TextBox txtAdvBalanceDeduct         = SDView.FindControl("TxtAdvBalanceDeduct") as TextBox;
            TextBox txtPF                       = SDView.FindControl("TxtPF1") as TextBox;
            TextBox txtESIC                     = SDView.FindControl("TxtPF2") as TextBox;
            TextBox txtProfTax                  = SDView.FindControl("TxtPF3") as TextBox;
            TextBox txtNetPayable               = SDView.FindControl("TxtNetPayable") as TextBox;
            TextBox txtTotalIncome              = SDView.FindControl("TxtNetIncome") as TextBox;
            CheckBox LeavesOverride             = SDView.FindControl("chkLeavesOverride") as CheckBox;
            #region Hidden Expenses not in this version
            //TextBox txtPaidExpenseCurrent       = SDView.FindControl("PaidExpenseCurrent") as TextBox;
            //TextBox txtPaidExpenseNew           = SDView.FindControl("PaidExpenseNew") as TextBox;
            //TextBox txtPaidExpenseSubtract      = SDView.FindControl("PaidExpenseSubtract") as TextBox;
            //TextBox txtUnPaidExpenseCurrent     = SDView.FindControl("UnPaidExpenseCurrent") as TextBox;
            //TextBox txtUnPaidExpenseNew         = SDView.FindControl("UnPaidExpenseNew") as TextBox;
            //TextBox txtExpenseDeductionCurrent  = SDView.FindControl("ExpenseDeductionCurrent") as TextBox;
            //TextBox txtExpenseDeductionNew      = SDView.FindControl("ExpenseDeductionNew") as TextBox;
            //TextBox txtNetPayableExpense        = SDView.FindControl("TxtNetExpense") as TextBox;
           
            
            //Label lblHiddenPaidExpenseCurrent   = SDView.FindControl("HiddenPaidExpenseCurrent") as Label;
            //Label lblHiddenUnPaidExpenseCurrent = SDView.FindControl("HiddenUnPaidExpenseCurrent") as Label;
            //Label lblHiddenExpenseDeductionCurrent = SDView.FindControl("HiddenExpenseDeductionCurrent") as Label;
            #endregion

            // Populate SalaryData from the UI elements
            //float LeavesCreditTaken = 0;
            float LeavesBalanceCurrent = 0;
            float LeavesBalancePrevious = 0;
            float PaidLeavesTaken = 0;
            float UnpaidLeavesTaken = 0;
            float daysworked = 0;
            float govtCompHoliday = 0;
            float OThours = 0;
            float AdvBalancePrevious = 0;
            float AdvBalanceCurrent = 0;
            float AdvBalanceAdd = 0;
            float AdvBalanceDeduct = 0;
            sd.LeavesOverride = LeavesOverride.Checked;
            float.TryParse(txtLeavesBalanceCurrent.Text, out LeavesBalanceCurrent);
            float.TryParse(txtLeavesBalancePrevious.Text, out LeavesBalancePrevious);
            float.TryParse(txtPaidLeavesTaken.Text, out PaidLeavesTaken);
            float.TryParse(txtUnpaidLeavesTaken.Text, out UnpaidLeavesTaken);
            float.TryParse(txtDaysWorked.Text, out daysworked);
            float.TryParse(txtGovtCompHoliday.Text, out govtCompHoliday);            
            float.TryParse(txtOTHours.Text, out OThours);

            float.TryParse(txtAdvBalancePrevious.Text, out AdvBalancePrevious);
            float.TryParse(txtAdvBalanceCurrent.Text, out AdvBalanceCurrent);
            float.TryParse(txtAdvBalanceAdd.Text, out AdvBalanceAdd);
            float.TryParse(txtAdvBalanceDeduct.Text, out AdvBalanceDeduct);

            //todo: remove everything from month details and keep only 
            cmd = getObject();
            if (lblID.Text != string.Empty) // Works for only edit mode
            {               
                sd.ID = Convert.ToInt32(lblID.Text);           
            }
            sd.GovtCompHoliday = govtCompHoliday;
            sd.PaidLeavesTaken = PaidLeavesTaken;
            sd.LeavesBalanceCurrent = LeavesBalanceCurrent;
            sd.LeavesBalancePrevious = LeavesBalancePrevious;  
            sd.OTHours = OThours;
            sd.AdvBalanceAdd = AdvBalanceAdd;            
            sd.AdvBalanceDeduct = AdvBalanceDeduct;
            sd.AdvBalancePrevious = AdvBalancePrevious;
            sd.AdvBalanceCurrent = AdvBalanceCurrent;
            sd.DaysWorked = daysworked;
            sd.DMID = cmd.DMID;
            sd.EmpID = EmpID;
            sd.Locked = true;
            return sd;
        }

        public void CopyObjectToUI(SalaryData Sd)
        {
            TextBox txtWeeklyOff            = SDView.FindControl("TxtWeeklyOffs") as TextBox;
            TextBox txtTotalDays            = SDView.FindControl("TxtTotalDays") as TextBox;
            TextBox txtLeavesBalanceCurrent = SDView.FindControl("TxtLeavesBalanceCurrent") as TextBox;
            TextBox txtLeavesBalancePrevious = SDView.FindControl("TxtLeavesBalancePrevious") as TextBox;
            TextBox txtPaidLeavesTaken      = SDView.FindControl("TxtPaidLeavesTaken") as TextBox;
            TextBox txtUnpaidLeavesTaken    = SDView.FindControl("TxtUnPaidLeavesTaken") as TextBox;
            TextBox txtDaysWorked           = SDView.FindControl("TxtDaysWorked") as TextBox;
            TextBox txtGovtCompHoliday      = SDView.FindControl("TxtGovtCompHoliday") as TextBox;
            TextBox txtTotalPaidDays        = SDView.FindControl("TxtTotalPaidDays") as TextBox;
            TextBox LblTotalBalanceLeaves   = SDView.FindControl("TotalBalanceLeaves") as TextBox;
            TextBox txtAmountForPaidDays    = SDView.FindControl("TxtAmountForPaidDays") as TextBox;
            TextBox txtOTHours              = SDView.FindControl("TxtOTHours") as TextBox;
            TextBox txtOTAmount             = SDView.FindControl("TxtOTAmount") as TextBox;
            TextBox txtAdvBalancePrevious   = SDView.FindControl("TxtAdvBalancePrevious") as TextBox;
            TextBox txtAdvBalanceCurrent    = SDView.FindControl("TxtAdvBalanceCurrent") as TextBox;
            TextBox txtAdvBalanceAdd        = SDView.FindControl("TxtAdvBalanceAdd") as TextBox;
            TextBox txtAdvBalanceDeduct     = SDView.FindControl("TxtAdvBalanceDeduct") as TextBox;
            TextBox txtPF                   = SDView.FindControl("TxtPF1") as TextBox;
            TextBox txtESIC                 = SDView.FindControl("TxtPF2") as TextBox;
            TextBox txtProfTax              = SDView.FindControl("TxtPF3") as TextBox;
            TextBox txtNetPayable           = SDView.FindControl("TxtNetPayable") as TextBox;
            TextBox txtTotalIncome          = SDView.FindControl("TxtNetIncome") as TextBox;
            CheckBox LeavesOverride         = SDView.FindControl("chkLeavesOverride") as CheckBox;
            txtWeeklyOff.Text               = cmd.WeeklyOffs.ToString("0.0"); ;
            txtTotalDays.Text               = cmd.TotalDaysOfMonth.ToString("0.0");
            txtLeavesBalanceCurrent.Text    = Sd.LeavesBalanceCurrent.ToString("0.0");
            txtLeavesBalancePrevious.Text   = Sd.LeavesBalancePrevious.ToString("0.0"); 
            txtPaidLeavesTaken.Text         = Sd.PaidLeavesTaken.ToString("0.0"); 
            txtUnpaidLeavesTaken.Text       =  Sd.UnPaidLeavesTaken.ToString("0.0"); 
            txtDaysWorked.Text              = Sd.DaysWorked.ToString("0.0"); 
            txtGovtCompHoliday.Text         = Sd.GovtCompHoliday.ToString("0.0"); 
            txtTotalPaidDays.Text           = Sd.TotalPayableDays.ToString("0.0"); 
              
            txtAmountForPaidDays.Text       = Sd.PaidAmount.ToString("0.00"); 
            txtOTHours.Text                 = Sd.OTHours.ToString("0.0"); 
            txtOTAmount.Text                = Sd.OTAmount.ToString("0.00"); 
            txtAdvBalancePrevious.Text      = Sd.AdvBalancePrevious.ToString("0.00"); 
            txtAdvBalanceCurrent.Text       = Sd.AdvBalanceCurrent.ToString("0.00"); 
            txtAdvBalanceAdd.Text           = Sd.AdvBalanceAdd.ToString("0.00"); 
            txtAdvBalanceDeduct.Text        = Sd.AdvBalanceDeduct.ToString("0.00");
            txtPF.Text                      = Sd.PFDeduction.ToString("0.00"); 
            txtESIC.Text                    = Sd.ESICDeduction.ToString("0.00"); 
            txtProfTax.Text                 = Sd.ProfTaxDeduction.ToString("0.00"); 
            txtNetPayable.Text              = Sd.NetPayable.ToString("0.00");
            LeavesOverride.Checked          = Sd.LeavesOverride;
        }
        
        #endregion

    }
}

