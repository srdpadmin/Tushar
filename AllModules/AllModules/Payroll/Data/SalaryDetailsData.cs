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
    public class SalaryDetailsData
    {
        public int      ID { get; set; }
        public int      EmpID { get; set; }
        public int      EmpType { get; set; }
        public float    BasicMonthly { get; set; }
        public float    BasicDaily { get; set; }
        public float    AllowanceMonthly { get; set; }
        public float    AllowanceDaily { get; set; }
        public int      OverTimeRate { get; set; }
        public float YearlyPaidLeaves { get; set; }
        
        //Extended items added from profile
        public bool     IsBasicDaily { get; set; }
        public bool     IsAllowanceDaily { get; set; }       
        public bool     DeductPF { get; set; }
        public bool     DeductESIC { get; set; }
        public bool     DeductProfTax { get; set; }
        public bool     CreditTA { get; set; }
        public bool     CreditDA { get; set; }
        public float    ProfessionalTaxPercent { get; set; }
        public float    ESICTaxPercent { get; set; }
        public float    ProvidentFundPercent { get; set; }
        public float    TravelAllowance{ get; set; }
        public float    DearnessAllowance { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        //Removed
        //public bool     HasAllowance { get; set; }
        //public bool     HasLeaves { get; set; }
        //public bool     HasOverTime { get; set; }
        //public float    BalanceLeaves { get; set; }
        //public float    AdvancePending { get; set; }       

    }
}
