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
using Payroll.Reports;
using AllModules.Payroll.Reports;

namespace Payroll.Controls
{
    public partial class OverView : System.Web.UI.UserControl
    {
        float sumNetSalary = 0,
              sumOverTime = 0,
              sumTotalSalary = 0,
              sumAdvDeduction = 0,
              sumNewAdvance = 0,
              sumUnpaidLeaves = 0,
              sumUnpaidAmt = 0,
              sumBalanceLeaves = 0,
              sumNetPayable = 0,
              sumBonus = 0,
              sumExtraBonue = 0,
              sumPaidLeaves = 0,
              sumOfUnPaidLeaves = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateYearMonth();
                Page.DataBind();
                //AnySalaryPresent();
            }
           
        }

        private void PopulateYearMonth()
        {
            // First populate the Salary report stuff
            YearList.DataSource = SalaryHelper.GetListOfYear();
            YearList.SelectedValue = DateTime.Now.Year.ToString();
            YearList.DataTextField = "YearName";
            YearList.DataValueField = "YearValue";
            MonthList.DataSource = SalaryHelper.GetMonthsInAYear();
            MonthList.SelectedValue = (DateTime.Now.Month == 1 ? 11 : DateTime.Now.Month - 1).ToString();
            MonthList.DataTextField = "MonthName";
            MonthList.DataValueField = "MonthValue";
        }

        protected void Generate_Click(object sender, EventArgs e)
        {
            AnySalaryPresent();
        }
        protected void YearList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnySalaryPresent();
        }

        protected void MonthList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnySalaryPresent();
        }

        public void AnySalaryPresent()
        {
            string dmid = string.Empty,currentMonth,currentYear;
            currentMonth = MonthList.SelectedValue;
            currentYear = YearList.SelectedValue;
            dmid = SalaryHelper.GetDMIDfromMonthYear(Convert.ToInt32(currentMonth), Convert.ToInt32(currentYear));
            SalaryReports rps = new SalaryReports();
            SalaryDataSet ds = rps.GetOverviewDetails(dmid);
            
            if (ds.OverView.Rows.Count > 0)
            {
                PrintAll.Enabled = true;
                PrintReport.Enabled = true;
                //PrintChart.Enabled = true;
                PrintAll.OnClientClick = "javascript:window.open('" + Page.Request.Url.GetLeftPart(UriPartial.Authority) +
                                         ConfigurationManager.AppSettings["PayrollRootPath"].ToString() + "SalarySlipAllEmployees.aspx?DMID=" + dmid + "'); return false;";
                PrintReport.OnClientClick = "javascript:window.open('" + Page.Request.Url.GetLeftPart(UriPartial.Authority) +
                                         ConfigurationManager.AppSettings["PayrollRootPath"].ToString() + "OverViewReport.aspx?DMID=" + dmid + "'); return false;";
                //PrintChart.OnClientClick = "javascript:window.open('" + Page.Request.Url.GetLeftPart(UriPartial.Authority) +
                                        //ConfigurationManager.AppSettings["PayrollRootPath"].ToString() + "LeavesReport.aspx?DMID=" + dmid + "'); return false;";


                ResultLbl.Text = "Records Found " + ds.OverView.Rows.Count.ToString();

                CommonGrid.DataSource = ds.OverView;
                CommonGrid.DataBind();
            }
            else
            {
                PrintAll.Enabled = false;
                PrintReport.Enabled = false;
                //PrintChart.Enabled = false;
                ResultLbl.Text = "No Records Found ";
                CommonGrid.DataBind();
            }
        }
        protected void CommonGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            float a1, a2, a3, a4, a5, a6, a7, a8, a9, a10;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Single.TryParse(e.Row.Cells[2].Text, out a8);
                Single.TryParse(e.Row.Cells[3].Text, out a9);
                Single.TryParse(e.Row.Cells[4].Text, out a1);
                Single.TryParse(e.Row.Cells[5].Text, out a2);
                Single.TryParse(e.Row.Cells[6].Text, out a3);
                Single.TryParse(e.Row.Cells[7].Text, out a4);
                //Single.TryParse(e.Row.Cells[8].Text, out a5);
                Single.TryParse(e.Row.Cells[8].Text, out a10);
                Single.TryParse(e.Row.Cells[9].Text, out a6);
                Single.TryParse(e.Row.Cells[10].Text, out a7);



                sumNetSalary        = sumNetSalary      + a8;
                sumOverTime         = sumOverTime       + a9;
                sumTotalSalary      = sumTotalSalary    + a1;
                sumAdvDeduction     = sumAdvDeduction   + a2;
                sumNewAdvance       = sumNewAdvance     + a3;
                sumUnpaidLeaves     = sumUnpaidLeaves   + a4;
                //sumUnpaidAmt        = sumUnpaidAmt      + a5;
                sumPaidLeaves       = sumPaidLeaves     + a10;
                sumBalanceLeaves    = sumBalanceLeaves  + a6;
                sumNetPayable       = sumNetPayable     + a7;

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[2].Text = sumNetSalary.ToString();
                e.Row.Cells[3].Text = sumOverTime.ToString();
                e.Row.Cells[4].Text = sumTotalSalary.ToString();
                e.Row.Cells[5].Text = sumAdvDeduction.ToString();
                e.Row.Cells[6].Text = sumNewAdvance.ToString();
                //e.Row.Cells[7].Text = sumUnpaidLeaves.ToString();
                e.Row.Cells[7].Text = sumUnpaidAmt.ToString();
                e.Row.Cells[8].Text = sumPaidLeaves.ToString();
                e.Row.Cells[9].Text = sumBalanceLeaves.ToString();
                e.Row.Cells[10].Text = sumNetPayable.ToString();
            }
        }     
    }
}