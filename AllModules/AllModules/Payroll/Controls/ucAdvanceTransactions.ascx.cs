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
using Payroll.Common;
using Payroll.BusLogic;

namespace Payroll.Controls
{
    public partial class ucAdvanceTransactions : System.Web.UI.UserControl
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

        public event EventHandler AdvanceUpdated;
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
            if (!String.IsNullOrEmpty(Request.Form[AdvanceTransactionsField.UniqueID]))
            {
                ControlID = Request.Form[AdvanceTransactionsField.UniqueID];
            }
            if (!ControlID.Contains("Update"))
            {
                PopulateData();
            }
            else
            {
                AdvanceTransactionsField.Value = string.Empty;
            }
        }

        public DataTable PopulateData()
        {
            Advance l = new Advance();
            DataTable dt=null;
            //if (ViewState["Advance"] != null)
            //{
            //    dt = (DataTable)ViewState["Advance"];
            //}
            //else
            //{
            dt = l.GetAllAdvanceTransactions(empID);
            //    ViewState["Advance"] = dt;
            //}
           
            gView.DataSource = dt;
            gView.DataBind();
            return dt;
        }
        protected void AddNewButton_Click(object sender, EventArgs e)
        {
            Advance l = new Advance();
            float? previousBalance = l.GetCurrentBalance(empID);
            
            //DataTable dt = (DataTable)ViewState["Advance"];
            DataTable dt = PopulateData(); 
            if (dt == null)
            {
                return;
            }
            dt.Rows.InsertAt(dt.NewRow(),0);
            dt.Rows[0]["ID"] = -1;
            dt.Rows[0]["PreviousBalance"] = (previousBalance == null) ? 0 : previousBalance.Value;
            //ViewState["Advance"] = dt;
            HideInsertButton(true);
            gView.EditIndex = 0;
            gView.PageIndex = 0;
            gView.DataSource = dt;
            gView.DataBind();
            
            //PopulateData();
            
        }
          
         protected void Updatebtn_Click(object sender, EventArgs e)
        {
            Advance lt = new Advance();
            Payroll.Data.AdvanceData ld = new Payroll.Data.AdvanceData();

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
            if (lt.InsertAdvanceTransaction(ld) > 0)
            {
                // worked fine
                currentBalance = ld.CurrentBalance;
                AdvanceUpdated(this, new EventArgs()); 
            }
            else
            {
                //Show  error
            }
            gView.EditIndex = -1;
            HideInsertButton(false);
            //ViewState["Advance"] = null;
            PopulateData();

        }
         protected void CancelBtn_Click(object sender, EventArgs e)
        {
            HideInsertButton(false);
            gView.EditIndex = -1;
            //ViewState["Advance"] = null;
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
