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
using Billing.BusLogic;
using Contact.BusLogic;

namespace Billing.Reports
{
    public partial class GenerateBill : System.Web.UI.Page
    {        
        string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string bID = Request.QueryString["ID"];
            type= Request.QueryString["Type"];

            if (!string.IsNullOrEmpty(bID))
            {
                Billing.BusLogic.Bill bill = new Billing.BusLogic.Bill();
                DataSet bds = bill.GetBill(bID);
                DataRow Drow = bds.Tables[0].Rows[0];
                ReportParameter rpt1 = new ReportParameter("TaxTotal",Drow["TaxAmount"].ToString());
                ReportParameter rpt2 = new ReportParameter("DiscountTotal", Drow["DiscountAmount"].ToString());
                ReportParameter rpt3 = new ReportParameter("AmountTotal", Drow["SubTotal"].ToString());
                ReportParameter rpt4 = new ReportParameter("BID", bID); //"0.00"

                BillItem oi = new BillItem();
                BillTerm ot = new BillTerm();

                DataSet BillTable = bill.GetBill(bID);
                DataSet ItemsTable = oi.GetBillItems(bID);
                DataSet TermsTable = ot.GetBillTerms(bID);
                Contacts v = new Contacts();
                DataSet vTable = v.GetContactDataSetById(Drow["CustID"].ToString());

                RPV.LocalReport.ReportPath = ConfigurationManager.AppSettings["BillReportPath"].ToString();
                //Server.MapPath("OrderReport.rdlc").ToString();
                RPV.LocalReport.SetParameters(new ReportParameter[] { rpt1, rpt2, rpt3, rpt4});
                RPV.LocalReport.DataSources.Add(new ReportDataSource("BDataSet_Bill", BillTable.Tables[0]));
                RPV.LocalReport.DataSources.Add(new ReportDataSource("BDataSet_BillItem", ItemsTable.Tables[0]));
                RPV.LocalReport.DataSources.Add(new ReportDataSource("BDataSet_BillTerm", TermsTable.Tables[0]));
                RPV.LocalReport.DataSources.Add(new ReportDataSource("BDataSet_Contacts", vTable.Tables[0]));
                RPV.LocalReport.Refresh();
            }
            writingReportLocally();
        }

        public void writingReportLocally()
        {

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            byte[] bytes = RPV.LocalReport.Render(
            type, null, out mimeType, out encoding, out extension,
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
    }
   
}
