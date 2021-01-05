using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Payroll.Data
{
    public class EmployeeProfileData
    {
        public int      ID { get; set; }
        public string   EmpID { get; set; }
        public int      EmpType { get; set; }        
        public bool     IsBasicMonthly { get; set; }
        public bool     IsAllowanceMonthly { get; set; }
        public bool     HasAllowance { get; set; }
        public bool     HasLeaves { get; set; }
        public bool     HasOverTime { get; set; }
        public bool     DeductPF { get; set; }
        public bool     DeductESIC { get; set; }
        public bool     DeductProfTax { get; set; }
        public float    ProfessionalTaxPercent { get; set; }
        public float    ESICTaxPercent { get; set; }
        public float    ProvidentFundPercent { get; set; }
        //Extended for UI select query EmployeeTypeName, EmployeeName does not belong to db
        public string   EmpName { get; set; }
        public string   EmpTypeDescription { get; set; }
            
    }
}
