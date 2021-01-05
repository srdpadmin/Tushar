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
    public class LeavesData
    {
        public int ID { get; set; }
        public int EmpID { get; set; }
        public int DMID { get; set; }
        public float Credit { get; set; }
        public float Debit { get; set; }
        public float PreviousBalance { get; set; }
        public float CurrentBalance { get; set; }
        public int SystemGenerated { get; set; }
        public string Comments { get; set; }
        public string MonthName { get; set; }
        public int iYear { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
