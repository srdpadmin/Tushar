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
namespace Payroll.Reports
{
    public partial class SalarySlip : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string mainHeaderA = "SARANG AUTO AERONAUTICS PRIVATE LIMITED";
                string mainHeaderB = "B-62, MIDC, Ambad, Nashik 422010";
                
                    //string mainHeaderA = "Sustainable Research Development Pvt. Ltd.";
                    //string mainHeaderB = "3rd Floor Library Building, VNIT, Nagpur 440010   Phone: 9403088130/7720040306";
                    string title = null;
                    string empId = Request.QueryString["EmpID"];
                    string DMID = Request.QueryString["DMID"];
                    string showHeader = Request.QueryString["showHeader"];

                    int year = 0, month = 0;
                    SalaryHelper.iGetMonthNameYearFromDMID(Convert.ToInt32(DMID), ref month, ref year);
                    bool displayHeader = false;
                    bool.TryParse(showHeader, out displayHeader);

                    //if (!displayHeader)
                    //{
                    //    mainHeaderA = string.Empty;
                    //    mainHeaderB = string.Empty;
                    //}

                    title = "PaySlip for " + Enum.GetName(typeof(ENM.MonthsOfYear), month) + " " + year;
                    //ReportToFile.WriteReportToFile();
                    ReportParameter p1 = new ReportParameter("Title", title);
                    ReportParameter p2 = new ReportParameter("mainHeader1", mainHeaderA);
                    ReportParameter p3 = new ReportParameter("mainHeader2", mainHeaderB);

                    //ReportParameter p2 = new ReportParameter("VendorName", "Shakti Trader");
                    //Sslip.LocalReport.ReportPath = Server.MapPath("SalarySlip.rdlc").ToString();
                    Sslip.LocalReport.ReportPath =  ConfigurationManager.AppSettings["SalarySlipReportPath"].ToString(); 
                    //Server.MapPath("SalarySlip.rdlc").ToString();// @"D:\Intetpub\vhosts\ssols.in\httpdocs\AllModules\Payroll\Reports\SalarySlip.rdlc";
                    Sslip.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });


                    //ReportDataSource rds1 = new ReportDataSource();
                    //rds1.DataSourceId = "ODS1";
                    //rds1.Name = "SalaryDataSet_EmployeeDetails";
                    //ReportDataSource rds2 = new ReportDataSource();
                    //rds2.DataSourceId = "ODS2";
                    //rds2.Name = "SalaryDataSet_Salary";

                    //Sslip.LocalReport.DataSources.Add(rds1);
                    //Sslip.LocalReport.DataSources.Add(rds2);

                    ODS1.SelectParameters.Add("EmpID", empId);
                    ODS2.SelectParameters.Add("EmpID", empId);
                    ODS2.SelectParameters.Add("DMID", DMID);
                 

            }
            catch (ReportViewerException ex )
            {
                AllModules.Validate.WriteToEventLog(ex, "SalarySlip_PageInit");
            }
            catch (Exception exc)
            {
                AllModules.Validate.WriteToEventLog(exc, "SalarySlip_PageInit");
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Sslip.LocalReport.Refresh();
            // Entire code below is used to block the Export to Excel option
           CustomizeRV((System.Web.UI.Control)sender);
            writingReportLocally();
        }
        public void writingReportLocally()
        {

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            try
            {

                byte[] bytes = Sslip.LocalReport.Render(
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
            catch (ReportViewerException ex)
            {
                AllModules.Validate.WriteToEventLog(ex, "SalarySlip_writingReportLocally");
            }
            catch (Exception exc)
            {
                AllModules.Validate.WriteToEventLog(exc, "SalarySlip_writingReportLocally");
            }
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
                    //foreach (System.Web.UI.WebControls.ListItem list in listItems)
                    //{
                    //    if (list.Text.Equals("Excel"))
                    //    { list.Enabled = false; }
                    //}
                }
            }
        }
    }
}
