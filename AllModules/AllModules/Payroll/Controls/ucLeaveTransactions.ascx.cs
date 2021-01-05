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
using Payroll.Common;

namespace Payroll.Controls
{//http://www.codeproject.com/Articles/134614/Way-To-Know-Which-Control-Has-Raised-PostBack
    public partial class ucLeaveTransactions : System.Web.UI.UserControl
    {
        private int empID;
        public int EmpID
        {
            get { return empID; }
            set { empID = value; }
        }

        private float currentBalance;
        public float CurrentBalance
        {
            get { return currentBalance; }
        }

        public event EventHandler LeavesUpdated;
        protected void Page_Load(object sender, EventArgs e)
        {
             
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (empID == 0)
            {
                empID = Convert.ToInt32(Request.QueryString["ID"]);
            }
            
            string ControlID = string.Empty;
            if (!String.IsNullOrEmpty(Request.Form[LeaveTransactionsField.UniqueID]))
            {
                ControlID = Request.Form[LeaveTransactionsField.UniqueID];
            }
            if (!ControlID.Contains("Update"))
            {
                PopulateData();
            }
            else
            {
                LeaveTransactionsField.Value = string.Empty;
            }
        }

        public DataTable PopulateData()
        {
            Leaves l = new Leaves();
            DataTable dt=null;
            //if (ViewState["leaves"] != null)
            //{
            //    dt = (DataTable)ViewState["leaves"];
            //}
            //else
            //{
                dt = l.GetAllLeavesTransactions(empID);
            //    ViewState["leaves"] = dt;
            //}
           
            gView.DataSource = dt;
            gView.DataBind();
            return dt;
        }
        protected void AddNewButton_Click(object sender, EventArgs e)
        {
            Leaves l = new Leaves();
            float? previousBalance = l.GetCurrentBalance(empID);
            
            //DataTable dt = (DataTable)ViewState["leaves"];
            DataTable dt = l.GetAllLeavesTransactions(empID);
            
            dt.Rows.InsertAt(dt.NewRow(),0);
            dt.Rows[0]["ID"] = -1;
            dt.Rows[0]["PreviousBalance"] = (previousBalance == null) ? 0 : previousBalance.Value;
            //ViewState["leaves"] = dt;
            HideInsertButton(true);
            gView.EditIndex = 0;
            gView.PageIndex = 0;
            gView.DataSource = dt;
            gView.DataBind();
            
        }
          
         protected void Updatebtn_Click(object sender, EventArgs e)
        {
            Leaves lt = new Leaves();
            Payroll.Data.LeavesData ld = new Payroll.Data.LeavesData();
            
            TextBox credit = gView.Rows[0].FindControl("Credit") as TextBox;
            TextBox debit = gView.Rows[0].FindControl("Debit") as TextBox;
            TextBox comments = gView.Rows[0].FindControl("Comments") as TextBox;
            float? previousBalance = lt.GetCurrentBalance(empID);
            if (Request.Form[credit.UniqueID] != string.Empty)
            {
                ld.Credit = Convert.ToSingle(Request.Form[credit.UniqueID]);
                //ld.CurrentBalance = previousBalance.Value + ld.Credit;
            }
            if (Request.Form[debit.UniqueID] != string.Empty)
            {
                ld.Debit = Convert.ToSingle(Request.Form[debit.UniqueID]);
                //ld.CurrentBalance = previousBalance.Value - ld.Debit;
            }
            ld.CurrentBalance = previousBalance.Value + ld.Credit - ld.Debit;                                                     
            ld.Comments = Request.Form[comments.UniqueID];
            ld.DMID = Convert.ToInt32(SalaryHelper.GetDMIDfromMonthYear(DateTime.Now.Month, DateTime.Now.Year));
            ld.MonthName = DateTime.Now.ToString("MMMM");
            ld.iYear = DateTime.Now.Year;
            ld.EmpID = empID;
            ld.PreviousBalance = previousBalance.Value;
            ld.CreatedOn = DateTime.Now;  // this is the sort expression to get the top 1 added last
            if (lt.InsertLeaveTransaction(ld) > 0)
            {
                // worked fine
                currentBalance = ld.CurrentBalance;
                LeavesUpdated(this, new EventArgs()); 
            }
            else
            {
                //Show  error
            }
            gView.EditIndex = -1;
            HideInsertButton(false);
            //ViewState["leaves"] = null;
            PopulateData();

        }
         protected void CancelBtn_Click(object sender, EventArgs e)
        {
            HideInsertButton(false);
            gView.EditIndex = -1;
            //ViewState["leaves"] = null;
            PopulateData();
        }

        void HideInsertButton(bool value)
        {
            if (value)
            {
                AddNewbtn.Visible = false;
                Updatebtn.Visible = true;
                CancelBtn.Visible = true;
            }
            else
            {
                AddNewbtn.Visible = true;
                Updatebtn.Visible = false;
                CancelBtn.Visible = false;
            }
        }

        protected void gView_PageIndexChanged(object sender, GridViewPageEventArgs e)
        {
            gView.PageIndex = e.NewPageIndex;
            PopulateData();
        }
    }
}