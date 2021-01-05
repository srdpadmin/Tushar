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
using Microsoft.Reporting.WebForms;
using Payroll.BusLogic;
using Payroll.Common;
using AllModules.Payroll.Reports;

namespace Payroll.Reports
{
    public partial class OverViewReport : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                string dmid = Request.QueryString["DMID"];
                
                SalaryReports rps = new SalaryReports();
                SalaryDataSet ds = rps.GetOverviewDetails(dmid);
                if (ds.OverView.Rows.Count > 0)
                {
                    float a = 0, b = 0, c = 0, d = 0, ef = 0,  h = 0, aa = 0, bb = 0;//cc=0,dd=0;

                    RRV.LocalReport.DataSources.Add(new ReportDataSource("SalaryDataSet_OverView", ds.OverView));
                    foreach (SalaryDataSet.OverViewRow rptRow in ds.OverView)
                    {
                        a += rptRow.NetSalary;
                        b += rptRow.OverTime;
                        c += rptRow.TotalSalary;
                        d += rptRow.AdvDeduction;
                        ef += rptRow.NewAdvance;
                        //g += rptRow.UnpaidAmount;
                        h += rptRow.NetPayableSalary;
                        aa += rptRow.UnpaidLeaves;
                        bb += rptRow.BalanceLeaves;
                        //cc += rptRow.IsBonusNull() ? 0 : rptRow.Bonus;
                        //dd += rptRow.IsExtraBonusNull() ? 0 : rptRow.ExtraBonus;
                    }
                    int currentMonth=0;
                    int currentYear = 0;
                    SalaryHelper.iGetMonthNameYearFromDMID(Convert.ToInt32(dmid), ref currentMonth, ref currentYear);
                    ReportParameter p1 = new ReportParameter("NetSalary", a.ToString("0.00"));
                    ReportParameter p2 = new ReportParameter("OverTime", b.ToString("0.00"));
                    ReportParameter p3 = new ReportParameter("TotalSalary", c.ToString("0.00"));
                    ReportParameter p4 = new ReportParameter("AdvDeduction", d.ToString("0.00"));
                    ReportParameter p5 = new ReportParameter("Title", "Salary Details for " + Enum.GetName(typeof(CoreAssemblies.EnumClass.MonthsOfYear),currentMonth) + " " + currentYear);
                    ReportParameter p6 = new ReportParameter("NewAdvance", ef.ToString("0.00"));
                    ReportParameter p7 = new ReportParameter("NetPayableSalary", h.ToString("0.00"));
                    
                    ReportParameter p9 = new ReportParameter("UnpaidLeaves", aa.ToString("0.00"));
                    ReportParameter p10 = new ReportParameter("BalanceLeaves", bb.ToString("0.00"));
                    //ReportParameter p8 = new ReportParameter("UnpaidAmount", g.ToString("0.00"));
                    //ReportParameter p11 = new ReportParameter("Bonus", cc.ToString("0.00"));
                    //ReportParameter p12 = new ReportParameter("ExtraBonus", dd.ToString("0.00"));
                    RRV.LocalReport.ReportPath = ConfigurationManager.AppSettings["OverViewReportPath"].ToString(); 
                    //Server.MapPath("OverViewReport.rdlc").ToString();
                    RRV.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p9, p10 });//,p11,p12 });
                }
            }
            catch (ReportViewerException ex)
            {
                AllModules.Validate.WriteToEventLog(ex,"OverViewReport_PageInit");
            }
            catch (Exception exc)
            {
                AllModules.Validate.WriteToEventLog(exc, "OverViewReport_PageInit");
            }
            finally
            {

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Entire code below is used to block the Export to Excel option
            //CustomizeRV((System.Web.UI.Control)sender);
            writingReportLocally();
        }

        public void writingReportLocally()
        {

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes =  RRV.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out extension,
            out streamids, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("Accept-Header", bytes.Length.ToString());
            Response.ContentType = mimeType;

            Response.OutputStream.Write(bytes, 0, Convert.ToInt32(bytes.Length));
            Response.Flush();
            Response.End();
        }

        // Patterned from Jon.   
        // Traverse all controls/child controls to get the dropdownlist.   
        // The first dropdown list is the ZoomGroup, followed by the ExportGroup.   
        // We just wanted the ExportGroup.   
        // When a dropdownlist is found, create a event handler to be used upon rendering.   
        private void CustomizeRV(System.Web.UI.Control reportControl)
        {
            foreach (System.Web.UI.Control childControl in reportControl.Controls)
            {
                if (childControl.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
                {
                    System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)childControl;
                    ddList.PreRender += new EventHandler(ddList_PreRender);
                }
                if (childControl.Controls.Count > 0)
                { CustomizeRV(childControl); }
            }
        }
        // This is the event handler added from CustomizeRV   
        // We just check the object type to get what we needed.   
        // Once the dropdownlist is found, we check if it is for the ExportGroup.  
        // Meaning, the "Excel" text should exists.   
        // Then, just traverse the list and disable the "Excel".   
        // When the report is shown, "Excel" will no longer be on the list.   
        // You can also do this to "PDF" or if you want to change the text.   
        void ddList_PreRender(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(System.Web.UI.WebControls.DropDownList))
            {
                System.Web.UI.WebControls.DropDownList ddList = (System.Web.UI.WebControls.DropDownList)sender;
                System.Web.UI.WebControls.ListItemCollection listItems = ddList.Items;
                if ((listItems != null) && (listItems.Count > 0) && (listItems.FindByText("Excel") != null))
                {
                    foreach (System.Web.UI.WebControls.ListItem list in listItems)
                    {
                        if (list.Text.Equals("Excel"))
                        { list.Enabled = false; }
                    }
                }
            }
        }
    }
}
