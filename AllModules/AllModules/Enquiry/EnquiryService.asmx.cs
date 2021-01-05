using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
 
namespace Enquiry
{
    /// <summary>
    /// Summary description for EnquiryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class EnquiryService : System.Web.Services.WebService
    {
        [WebMethod]
        public string PutData(string sName, string sCompany, string sEmail, string sTelephone, string sEnquiryType, string sSubject, string sMessage, string UID)
        {
            string outText = "Enquiry Successfully Added";
            Enquiry.BusLogic.Enquiries enquiries = new Enquiry.BusLogic.Enquiries();
            Enquiry.Data.EnquiriesData enqData = new Enquiry.Data.EnquiriesData();
            enqData.EName = sName;
            enqData.Company = sCompany;
            enqData.Email = sEmail;
            enqData.Telephone = sTelephone;
            enqData.EnquiryType = Convert.ToInt32(sEnquiryType);
            enqData.Subject = sSubject;
            enqData.EName = sMessage;
            enqData.CreatedBy = Convert.ToInt32(UID);
            enqData.ModifiedBy = Convert.ToInt32(UID);
            enqData.Status = (int)CoreAssemblies.EnumClass.EnquiryStatus.Open;
            enquiries.AddNewEnquiry(enqData);
            int id = enquiries.AddNewEnquiry(enqData);
            if (id < 0)
            {
                outText = "Problem submitting information. Please try again later.";
            }
             
            return outText;
        }
        
    }
}
