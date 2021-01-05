using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Payroll.BusLogic;
using Payroll.Common;
using AllModules.Payroll.Reports;
using System.Configuration;

namespace AllModules.Payroll.Reports
{
    public partial class LeavesReport : System.Web.UI.Page
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
               
                    RRV.LocalReport.DataSources.Add(new ReportDataSource("SalaryDataSet_OverView", ds.OverView));
                    
                    int currentMonth = 0;
                    int currentYear = 0;
                    SalaryHelper.iGetMonthNameYearFromDMID(Convert.ToInt32(dmid), ref currentMonth, ref currentYear);

                    RRV.LocalReport.ReportPath = ConfigurationManager.AppSettings["LeavesReportPath"].ToString();
                   
                }
            }
            catch (ReportViewerException ex)
            {
                Errors.LogError(ex);
            }
            catch (Exception exc)
            {
                Errors.LogError(exc);
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
            byte[] bytes = RRV.LocalReport.Render(
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
