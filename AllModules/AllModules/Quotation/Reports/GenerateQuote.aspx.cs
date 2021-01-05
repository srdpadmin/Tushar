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
using Quotation.BusLogic;
using Contact.BusLogic;

namespace Quotation.Reports
{
    public partial class GenerateQuote : System.Web.UI.Page
    {        
        string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string bID = Request.QueryString["ID"];
            type= Request.QueryString["Type"];

            if (!string.IsNullOrEmpty(bID))
            {
                Quotation.BusLogic.Quote Quote = new Quotation.BusLogic.Quote();
                DataSet bds = Quote.GetQuote(bID);
                DataRow Drow = bds.Tables[0].Rows[0];
                ReportParameter rpt1 = new ReportParameter("TaxTotal",Drow["TaxAmount"].ToString());
                ReportParameter rpt2 = new ReportParameter("DiscountTotal", Drow["DiscountAmount"].ToString());
                ReportParameter rpt3 = new ReportParameter("AmountTotal", Drow["Total"].ToString());
                ReportParameter rpt4 = new ReportParameter("BID", bID); //"0.00"

                QuoteItem oi = new QuoteItem();
                QuoteTerm ot = new QuoteTerm();

                DataSet QuoteTable = Quote.GetQuote(bID);
                DataSet ItemsTable = oi.GetQuoteItems(bID);
                DataSet TermsTable = ot.GetQuoteTerms(bID);
                Contacts v = new Contacts();
                DataSet vTable = v.GetContactDataSetById(Drow["CustID"].ToString());


                RPV.LocalReport.ReportPath = ConfigurationManager.AppSettings["QuoteReportPath"].ToString();
                //Server.MapPath("OrderReport.rdlc").ToString();
                RPV.LocalReport.SetParameters(new ReportParameter[] { rpt1, rpt2, rpt3, rpt4 });
                RPV.LocalReport.DataSources.Add(new ReportDataSource("QDataSet_Quote", QuoteTable.Tables[0]));
                RPV.LocalReport.DataSources.Add(new ReportDataSource("QDataSet_QuoteItem", ItemsTable.Tables[0]));
                RPV.LocalReport.DataSources.Add(new ReportDataSource("QDataSet_QuoteTerm", TermsTable.Tables[0]));
                RPV.LocalReport.DataSources.Add(new ReportDataSource("QDataSet_Contacts", vTable.Tables[0]));
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
            byte[] bytes= RPV.LocalReport.Render(
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
