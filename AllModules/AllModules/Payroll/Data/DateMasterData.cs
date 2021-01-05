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
    public class DateMasterData
    {
        public int ID { get; set; } 
        public string DMID { get; set; }       
        public int iMonth { get; set; }
        public int iYear { get; set; }
        public int sMonth { get; set; }
    }
}
