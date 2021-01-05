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
using ENM = CoreAssemblies.EnumClass;
using Payroll.Common;
using Payroll.BusLogic;
using AllModules.Payroll.Reports;
namespace Payroll.Reports
{
    public partial class SalarySlipAllEmployees : System.Web.UI.Page
    {
        string dmid = string.Empty;
        
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                //string mainHeaderA = "SARANG AUTO AERONAUTICS PRIVATE LIMITED";
                //string mainHeaderB = "B-62, MIDC, Ambad, Nashik 422010";
                string mainHeaderA = "ABC ENTERPRISES PRIVATE LIMITED";
                string mainHeaderB = "A-1 BLock , The Business Centre, MIDC INDIA 411010";

                dmid= Request.QueryString["DMID"];
                DM.Value = dmid;
                int currentMonth = 0;
                int currentYear = 0;
                SalaryHelper.iGetMonthNameYearFromDMID(Convert.ToInt32(dmid), ref currentMonth, ref currentYear);


                string slipFor = "PaySlip for " + Enum.GetName(typeof(ENM.MonthsOfYear), Convert.ToInt32(currentMonth)) + " " + currentYear;
                ReportParameter p1 = new ReportParameter("Title1", mainHeaderA);
                ReportParameter p2 = new ReportParameter("Title2", mainHeaderB);
                ReportParameter p3 = new ReportParameter("PaySlipFor", slipFor);
                AllSalaries.LocalReport.ReportPath = ConfigurationManager.AppSettings["SSAEReportPath"].ToString();
                //AllSalaries.LocalReport.ReportPath = Server.MapPath("SalarySlipAllEmployeesSingleSalary.rdlc");
                AllSalaries.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                //using (System.IO.Stream report = System.IO.File.OpenRead("C:\\WebDevelopment\\AllModules\\AllModules\\Payroll\\Reports\\SalarySlipAllEmployeesSingleSalary.rdlc"))
                //using (System.IO.Stream report = System.IO.File.OpenRead("D:\\Intetpub\\vhosts\\ssols.in\\httpdocs\\AllModules\\Payroll\\Reports\\SalarySlipAllEmployeesSingleSalary.rdlc"))                
                //{
                //    AllSalaries.LocalReport.LoadSubreportDefinition(ConfigurationManager.AppSettings["SubReportValue"].ToString(), report);
                //}
                //AllSalaries.LocalReport.DataSources
                //ODS2.SelectParameters.Add("DMID", dmid);   
                //ODS2.SelectParameters.Add("MonthNumber", "1");
                //ODS2.SelectParameters.Add("YearNumber", "2011");
                //@"~\Payroll\Reports\SalarySlipAllEmployees.rdlc"
                
                //ReportDataSource rds1 = new ReportDataSource();
                //rds1.DataSourceId="ODS1";
                //rds1.Name="SalaryDataSet_EmployeeDetails";
                //ReportDataSource rds2 = new ReportDataSource();
                //rds2.DataSourceId = "ODS2";
                //rds2.Name = "SalaryDataSet_SalaryInformation";

                //AllSalaries.LocalReport.DataSources.Add(rds1);
                //AllSalaries.LocalReport.DataSources.Add(rds2);
                SalaryReports rps = new SalaryReports();
                SalaryDataSet.EmployeeDetailsDataTable dtable= rps.GetAllEmployeeDetails(dmid);
                AllSalaries.LocalReport.DataSources.Add(new ReportDataSource("SalaryDataSet_EmployeeDetails", dtable));
                AllSalaries.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

                //AllSalaries1.Drillthrough += new DrillthroughEventHandler(AllSalaries1_Drillthrough);
              

            }
            catch (ReportViewerException ex)
            {
                AllModules.Validate.WriteToEventLog(ex, "SalarySlipAllEmployees_writingReportLocally");
            }
            catch (Exception exc)
            {
                AllModules.Validate.WriteToEventLog(exc, "SalarySlipAllEmployees_writingReportLocally");
            }


            /*Response.AddHeader("content-disposition", "attachment; filename=Report." + extension);
            Response.BinaryWrite(bytes);*/

        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {

            SalaryReports srp = new SalaryReports();
            string empID = e.Parameters[0].Values[0];
            //string BasicSalary = e.Parameters[1].Values[0];
            //string Allowance = e.Parameters[2].Values[0];
            //string AdvBalance = e.Parameters[3].Values[0];
            //string PaidYearlyLeaves = e.Parameters[4].Values[0];
            //string BalanceLeaves = e.Parameters[5].Values[0];

            e.DataSources.Add(new ReportDataSource("SalaryDataSet_Salary", srp.GetSalaryDetails(empID,dmid)));
        }

        void AllSalaries1_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            LocalReport report = (LocalReport)e.Report;

            // The Second dataSet Or 
            //detailed Datasetreport.DataSources.Add(new ReportDataSource("AdventureWorksDataSet1_vEmployee", dsSubReport.Tables[0])); 
        }
        #region For Report Display properties only
        protected void Page_Load(object sender, EventArgs e)
        {
            AllSalaries.LocalReport.Refresh();
            // Entire code below is used to block the Export to Excel option
            CustomizeRV((System.Web.UI.Control)sender);
            //writingReportLocally();
            
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

        public void writingReportLocally()
        {

            Warning[] warnings;
            string[] streamids;
            string mimeType=string.Empty;
            string encoding=string.Empty;
            string extension=string.Empty;
            byte[] bytes = AllSalaries.LocalReport.Render(
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
        #endregion
        
    }
}
