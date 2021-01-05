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
using System.Collections.Generic;
using Payroll.BusLogic;
using AjaxControlToolkit;
using ENM = CoreAssemblies.EnumClass;

namespace Payroll.Forms
{
    public partial class SalaryInformation : System.Web.UI.Page
    {
        private int empID;
        public int EmpID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["EmpID"]))
                    empID = Convert.ToInt32(Request.QueryString["EmpID"]);
                return empID;
            }
            set { empID = value; }
        }    
        protected void Page_Init(object sender, EventArgs e)
        {
            empLookup.empChanged += new EventHandler(empLookup_empChanged);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (EmpID > 0)
            {
                empLookup.EmployeeID = EmpID.ToString();
                ActionItemBtnPanelVisible(true);
                ActionItemBtnVisible(false);
            }
            else if(!string.IsNullOrEmpty(empLookup.EmployeeID))
            {
                ActionItemBtnPanelVisible(true);
                ActionItemBtnVisible(false);
            }
            else
            {                
                ActionItemBtnPanelVisible(false);
                ActionItemBtnVisible(false);
            }
            upS.Update();
        }

        void empLookup_empChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(empLookup.EmployeeID) && Convert.ToInt32(empLookup.EmployeeID)>0)
            {
                ActionItemBtnPanelVisible(true);                                
            }
            else
            {
                ActionItemBtnPanelVisible(false);                               
            }
            SalaryGrid.DataBind();
            upS.Update();
            upL.Update();
            
        }
        protected void ActionItemBtnPanelVisible(Boolean mode)
        {
            if (mode)
            {
               BtnPanel.Visible = true;
            }
            else
            {
                BtnPanel.Visible = false;
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

        protected void SalaryGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int empID = 0;
            int dmID = 0;
            //bool locked = false;
            if (e.CommandName == "View" && !string.IsNullOrEmpty(empLookup.EmployeeID))
            {
                string argAll = (string)e.CommandArgument;
                string[] args = argAll.Split(';');
                Int32.TryParse(args[0], out empID);
                Int32.TryParse(args[1], out dmID);
                Response.Redirect("~/Payroll/Forms/ManageSalary.aspx?EmpID=" + empID + "&DMID=" + dmID);

            }
            //    string argAll = (string)e.CommandArgument;
            //    string[] args = argAll.Split(';');
            //    Int32.TryParse(args[0], out monthnumber);
            //    Int32.TryParse(args[1], out yearnumber);
            //    bool.TryParse(args[2], out locked);
            //    if ((monthnumber > 0 && monthnumber < 13))
            //    {
            //        AddNewSalary.Visible = false;
            //        DeleteSalary.Visible = false;
            //        SaveNewSalary.Visible = false;
            //        Cancel.Visible = true;
            //        if (workflow.MD.Active)
            //        {
            //            EditSalary.Visible = locked == true ? false : true;
            //        }

            //        SalaryDetails.Visible = true;
            //        SalaryGrid.Visible = false;
            //        workflow.SerialNumbers.ActiveSalaryMonth = monthnumber;
            //        workflow.SerialNumbers.ActiveSalaryYear = yearnumber;
            //        UpdateWorkFlow(false);
            //        workflow.MD.SelectedMonth = monthnumber;
            //        workflow.MD.SelectedYear = yearnumber;
            //        SalaryDetails.DataBind();

            //    }
            //}
        }

        protected void imgAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(empLookup.EmployeeID))
            {
                Response.Redirect("~/Payroll/Forms/ManageSalary.aspx?EmpID=" + empLookup.EmployeeID);
            }
        } 
       
        protected void imgCancel_Click(object sender, EventArgs e)
        {
            ActionItemBtnVisible(false);
            SalaryGrid.EditIndex = -1;
            SalaryGrid.DataBind();
        }

        protected void imgEdit_Click(object sender, EventArgs e)
        {
            ActionItemBtnVisible(true);
            GridViewRow row = SalaryGrid.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("cbSelect") != null) && ((RadioButton)row.FindControl("cbSelect")).Checked)
            {
                SalaryGrid.EditIndex = row.RowIndex;
                SalaryGrid.DataBind();
            }
            //row = SalaryGrid.SelectedRow;
            //SalaryGrid.Rows.OfType<GridViewRow>().ToList().Where(a => a == row).ToList().
            //                 ForEach(a => ((RadioButton)a.FindControl("rbtnSelect")).Checked = true);
            //SalaryGrid.SelectedIndex = row.RowIndex;
             
        }

        protected void imgUpdate_Click(object sender, EventArgs e)
        {
            ActionItemBtnVisible(false);
            GridViewRow row = SalaryGrid.SelectedRow;
            CheckBox cb = row.FindControl("LockedCheck") as CheckBox;
            if (cb != null)
            {
                 string dmID= Convert.ToString(SalaryGrid.DataKeys[row.RowIndex].Value);
                 Salary sal = new Salary();
                 sal.LockUnlockSalary(empLookup.EmployeeID, dmID, cb.Checked);
            }
            SalaryGrid.EditIndex = -1;
            SalaryGrid.DataBind();
        }

        protected void imgDelete_Click(object sender, EventArgs e)
        {
            GridViewRow row = SalaryGrid.SelectedRow;
            if (row != null && ((RadioButton)row.FindControl("cbSelect") != null) && ((RadioButton)row.FindControl("cbSelect")).Checked)
            {
                string dmID = Convert.ToString(SalaryGrid.DataKeys[row.RowIndex].Value);
                Salary sal = new Salary();
                int x = sal.DeleteSalary(empLookup.EmployeeID, dmID);
                SalaryGrid.DataBind();
            }
        }

        protected void SalaryGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow eRow = e.Row as GridViewRow;
            bool IsAdmin = false;
              if (eRow.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState & DataControlRowState.Edit) <= 0)
              {
                  //Readonly Rows
                RadioButton cbs = e.Row.FindControl("cbSelect") as RadioButton;
                CheckBox locked = e.Row.FindControl("LockedCheck") as CheckBox;
                LinkButton print = e.Row.FindControl("PrintButton") as LinkButton;
                Label emptype = e.Row.FindControl("EmpType") as Label;                
                MembershipUser user = Membership.GetUser();
                IsAdmin = AllModules.Validate.UserRoleAccess(Convert.ToInt32(user.ProviderUserKey), CoreAssemblies.EnumClass.Roles.Admin);
                cbs.Enabled = IsAdmin;
                locked.Enabled = false;
                //Page.User.IsInRole("admin") ? true : false;                
                if (print != null)
                {
                    string[] info = print.CommandArgument.Split(';');
                    string appPath = Page.Request.Url.GetLeftPart(UriPartial.Authority);
                    //Showheader only when empType is monthlyWithBenfits
                    bool showHeader = Convert.ToInt32(emptype.Text) == (int)ENM.EmployeeType.MonthlyWithBenefits ? true : false;
                    string jscriptline = "javascript:ShowPaySlipInNewWindow('" + info[0] + "','" + info[1] + "','" + showHeader.ToString() + "'); return false;";
                    print.OnClientClick = jscriptline;
                    //print.Attributes.Add("OnClientclick", jscriptline);
                    //print.OnClientClick = "javascript:window.open('" + appPath + "/Reports/GeneratePaySlip.aspx?EmpID=" + info[0] + "&month=" + info[1] + "&year=" + info[2] + "&showHeader=" + showHeader.ToString() + "'); return false;";
                    //print.OnClientClick = "javascript:window.open('" + appPath + "/SMModule/Reports/GeneratePaySlip.aspx?EmpID=" + info[0] + "&month=" + info[1] + "&year=" + info[2] + "'); return false;";
                }
                
            }
              else if ((e.Row.RowState & DataControlRowState.Edit) > 0)
              {
                  CheckBox locked = e.Row.FindControl("LockedCheck") as CheckBox;
                  MembershipUser user = Membership.GetUser();
                  IsAdmin = AllModules.Validate.UserRoleAccess(Convert.ToInt32(user.ProviderUserKey), CoreAssemblies.EnumClass.Roles.Admin);
                  locked.Enabled = IsAdmin;                   
              }
        }

        protected void SelectButton_Click(object sender, EventArgs e)
        {
            RadioButton rbSelect = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rbSelect.NamingContainer;
            SalaryGrid.Rows.OfType<GridViewRow>().ToList().Where(a => a != row).ToList().
                           ForEach(a => ((RadioButton)a.FindControl("cbSelect")).Checked = false);
            SalaryGrid.SelectedIndex = row.RowIndex;
        }

        protected void LockedCheck_CheckedChanged(object sender, EventArgs e)
        {
            //Locked checkbox handled here
            //create a query to execute into datbase
            //CheckBox x = sender as CheckBox;
            //string[] args = x.ToolTip.Split(';');
            //int monthnumber = 0, yearnumber = 0;
            //Int32.TryParse(args[1], out monthnumber);
            //Int32.TryParse(args[2], out yearnumber);
            //SalaryBL salB = new SalaryBL();
            //if (salB.UnlockSalary(args[0], monthnumber, yearnumber) == 0)
            //{
            //    ErrorLog.addError("LockedCheck_CheckedChanged", "Unable to unlock the salary ");
            //}
            //else
            //{
            //    SalaryGrid.DataBind();
            //}

        }

        // unused
        //public bool MonthlySalaryExistInSystem()
        //{
        //    SalaryBL salB = new SalaryBL();
        //    return !salB.MonthlySalaryExistInSystem(workflow.MD.EmpID, workflow.MD.SelectedMonth, workflow.MD.SelectedYear);
        //}

        
    }
}
